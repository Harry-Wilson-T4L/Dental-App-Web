using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DentalDrill.SmartFreight.Client;
using DentalDrill.SmartFreight.Client.Models;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Quartz;

namespace DentalDrill.CRM.Scheduler
{
    [DisallowConcurrentExecution]
    public class PickupRequestSmartFreightSubmitJob : IJob
    {
        private readonly IRepository repository;
        private readonly ISuspendService suspendService;
        private readonly ISecureCodeGenerationService codeGenerationService;
        private readonly PickupRequestSmartFreightOptions options;

        public PickupRequestSmartFreightSubmitJob(IRepository repository, ISuspendService suspendService, ISecureCodeGenerationService codeGenerationService, IOptions<PickupRequestSmartFreightOptions> options)
        {
            this.repository = repository;
            this.suspendService = suspendService;
            this.codeGenerationService = codeGenerationService;
            this.options = options.Value;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            if (!this.options.Enabled)
            {
                return;
            }

            if (!this.suspendService.Test("PickupRequestSmartFreightSubmitJob"))
            {
                return;
            }

            var requests = await this.repository.Query<PickupRequest>("CreatedBy", "Employee", "Client", "Corporate")
                .Where(x => x.Status == PickupRequestStatus.EmailSent && (x.Type == PickupRequestType.Australia || x.Type == PickupRequestType.NewZealand))
                .ToListAsync();

            foreach (var request in requests)
            {
                ISmartFreightClient client = new SmartFreightClient(this.options.CreateClientOptions());

                Int32 accountNumber;
                if (request.ClientId.HasValue)
                {
                    accountNumber = request.Client.ClientNo;
                }
                else
                {
                    var anonymous = await this.GetAnonymousRecipient(request);
                    accountNumber = anonymous.AccountNumber;
                }

                var consignment = new Consignment
                {
                    ReturnConsignment = ConsignmentReturnType.Return,
                    Receiver = new Receiver
                    {
                        AccountNumber = accountNumber.ToString(),
                        Name = request.PracticeName,
                        Address = new AddressDetails
                        {
                            AddressLine1 = request.AddressLine1,
                            AddressLine2 = request.AddressLine2,
                            Town = request.Suburb,
                            Postcode = request.Postcode,
                            State = request.State,
                            Country = request.Country,
                        },
                        ContactName = request.ContactPerson,
                        EmailAddress = request.Email,
                        PhoneNumber = request.Phone,
                    },
                    Sender = new Sender
                    {
                        Name = "DENTAL DRILL SOLUTIONS",
                        Address = new AddressDetails
                        {
                            AddressLine1 = "33 Benares Crescent",
                            Town = "ACACIA GARDENS",
                            State = "NSW",
                            Postcode = "2763",
                            Country = "AUSTRALIA",
                        },
                        EmailAddress = this.options.SenderEmail,
                    },
                    Carrier = new Carrier { LeastCost = true, },
                    WebPrint = true,
                    Lines = new List<FreightLine>(),
                };

                if (request.HandpiecesCount < 10)
                {
                    consignment.Lines.Add(new FreightLine
                    {
                        Amount = 1,
                        Description = "SATCHEL",
                        Weight = 1,
                        Volume = 0.001m,
                    });
                }
                else
                {
                    consignment.Lines.Add(new FreightLine
                    {
                        Amount = 1,
                        Description = "CARTON",
                        Weight = (request.HandpiecesCount / 15) + (request.HandpiecesCount % 15 > 0 ? 1 : 0),
                        Volume = 0.001m,
                    });
                }

                if (this.options.Testing)
                {
                    consignment.Receiver.EmailAddress = this.options.SenderEmail;
                    consignment.SpecialInstructions = new SpecialInstructions { Line1 = "TEST CONSIGNMENT", Line2 = "PLEASE IGNORE", };
                }
                else
                {
                    consignment.SpecialInstructions = new SpecialInstructions { Full = request.Comment, };
                }

                var requestXml = consignment.SerializeToString();

                var requestConsignment = new PickupRequestConsignment
                {
                    RequestId = request.Id,
                    DateTime = DateTime.UtcNow,
                    Reference = this.codeGenerationService.GenerateAlphaNumericCode(20),
                    RequestXml = requestXml,
                    Status = PickupRequestConsignmentStatus.Created,
                };

                await this.repository.InsertAsync(requestConsignment);
                await this.repository.SaveChangesAsync();

                try
                {
                    var (result, _, responseXml) = await client.ImportExAsync(requestConsignment.Reference, consignment);

                    requestConsignment.Status = PickupRequestConsignmentStatus.Succeeded;
                    requestConsignment.ResponseXml = responseXml;
                    await this.repository.UpdateAsync(requestConsignment);

                    request.Status = PickupRequestStatus.ConsignmentCreated;
                    await this.repository.UpdateAsync(request);

                    await this.repository.SaveChangesAsync();
                }
                catch (TimeoutException timeout)
                {
                    requestConsignment.Status = PickupRequestConsignmentStatus.Failed;
                    requestConsignment.ExceptionClass = timeout.GetType().FullName;
                    requestConsignment.ExceptionMessage = timeout.Message;
                    requestConsignment.ExceptionDetails = timeout.ToString();
                    await this.repository.UpdateAsync(requestConsignment);

                    await this.repository.SaveChangesAsync();

                    this.suspendService.SuspendFor("PickupRequestSmartFreightSubmitJob", 15);
                    return;
                }
                catch (Exception exception) when (exception is System.ServiceModel.CommunicationException && exception.InnerException?.InnerException?.Message == "The operation timed out")
                {
                    requestConsignment.Status = PickupRequestConsignmentStatus.Failed;
                    requestConsignment.ExceptionClass = exception.GetType().FullName;
                    requestConsignment.ExceptionMessage = exception.Message;
                    requestConsignment.ExceptionDetails = exception.ToString();
                    await this.repository.UpdateAsync(requestConsignment);

                    await this.repository.SaveChangesAsync();

                    this.suspendService.SuspendFor("PickupRequestSmartFreightSubmitJob", 15);
                    return;
                }
                catch (Exception exception)
                {
                    requestConsignment.Status = PickupRequestConsignmentStatus.Failed;
                    requestConsignment.ExceptionClass = exception.GetType().FullName;
                    requestConsignment.ExceptionMessage = exception.Message;
                    requestConsignment.ExceptionDetails = exception.ToString();
                    await this.repository.UpdateAsync(requestConsignment);

                    request.Status = PickupRequestStatus.Failed;
                    await this.repository.UpdateAsync(request);

                    await this.repository.SaveChangesAsync();
                }
            }
        }

        private async Task<PickupRequestAnonymousRecipient> GetAnonymousRecipient(PickupRequest request)
        {
            var candidate = new PickupRequestAnonymousRecipient(request);
            var existing = await this.repository.Query<PickupRequestAnonymousRecipient>().SingleOrDefaultAsync(x => x.UniqueKey == candidate.UniqueKey);
            if (existing != null)
            {
                return existing;
            }

            var maxNumber = await this.repository.Query<PickupRequestAnonymousRecipient>().OrderByDescending(x => x.AccountNumber).Take(1).FirstOrDefaultAsync();
            var nextNumber = maxNumber != null ? (maxNumber.AccountNumber + 1) : 100000000;

            candidate.AccountNumber = nextNumber;
            await this.repository.InsertAsync(candidate);
            await this.repository.SaveChangesAsync();
            return candidate;
        }
    }
}
