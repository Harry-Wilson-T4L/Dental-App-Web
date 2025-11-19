using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Import;
using DentalDrill.CRM.Services.Csv;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.EntitySequences;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace DentalDrill.CRM.Services
{
    public class ImportService
    {
        private readonly IRepository repository;
        private readonly IEntitySequenceService entitySequence;

        public ImportService(IRepository repository, IEntitySequenceService entitySequence)
        {
            this.repository = repository;
            this.entitySequence = entitySequence;
        }

        public IReadOnlyList<ClientImportModel> ParseClients(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return CsvParser.Parse<ClientImportModel>(reader, ',', '"').ToList();
            }
        }

        public IReadOnlyDictionary<String, Int32> ParseClientNumbers(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var entries = CsvParser.Parse(reader, ',', '"').ToList();
                return entries.ToDictionary(x => StringExtensions.CollapseSpaces(x["Key"]), x => Int32.Parse(x["Number"]));
            }
        }

        public IReadOnlyList<HandpieceImportModel> ParseHandpieces(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                return CsvParser.Parse<HandpieceImportModel>(reader, ';', '"').ToList();
            }
        }

        public IReadOnlyDictionary<String, String> ParseHandpiecesMapping(Stream stream)
        {
            using (var reader = new StreamReader(stream))
            {
                var entries = CsvParser.Parse(reader, ',', '"').ToList();
                return entries.ToDictionary(x => x["Name"], x => StringExtensions.CollapseSpaces(x["Matching Client"]));
            }
        }

        public async Task<IReadOnlyList<HandpieceImportModel>> ResolveClients(IReadOnlyList<HandpieceImportModel> handpieces, Stream mappingStream)
        {
            var mapping = this.ParseHandpiecesMapping(mappingStream);
            var existing = await this.repository.QueryWithoutTracking<Client>().Where(x => x.ImportKey != null && x.ImportKey != "").ToListAsync();
            var existingMap = existing.ToDictionary(x => x.ImportKey);

            foreach (var handpiece in handpieces)
            {
                if (mapping.TryGetValue(handpiece.Client, out var clientId))
                {
                    handpiece.ClientId = clientId;
                    handpiece.ClientEntity = existingMap.TryGetValue(clientId, out var entity) ? entity : null;
                }
            }

            return handpieces;
        }

        public async Task<IReadOnlyList<HandpieceImportModel>> ResolveTechs(IReadOnlyList<HandpieceImportModel> handpieces, Stream techsStream)
        {
            IDictionary<String, String> mapping;
            using (var reader = new StreamReader(techsStream))
            {
                var entries = CsvParser.Parse(reader, ',', '"').ToList();
                mapping = entries.ToDictionary(x => x["Key"], x => x["Value"]);
            }

            var existing = await this.repository.QueryWithoutTracking<Employee>().ToListAsync();
            var existingMap = existing.ToDictionary(x => $"{x.FirstName} {x.LastName}");

            foreach (var handpiece in handpieces)
            {
                if (mapping.TryGetValue(handpiece.Tech, out var techCode) && existingMap.TryGetValue(techCode, out var tech))
                {
                    handpiece.TechEntity = tech;
                }
            }

            return handpieces;
        }

        public Task<IReadOnlyList<HandpieceImportModel>> ResolveRating(IReadOnlyList<HandpieceImportModel> handpieces, Stream ratingsStream)
        {
            IDictionary<String, Int32> mapping;
            using (var reader = new StreamReader(ratingsStream))
            {
                var entries = CsvParser.Parse(reader, ',', '"').ToList();
                mapping = entries.ToDictionary(x => x["Key"].Trim(), x => Int32.Parse(x["Value"]));
            }

            foreach (var handpiece in handpieces)
            {
                if (mapping.TryGetValue(handpiece.Rating.Trim(), out var rating))
                {
                    handpiece.RatingParsed = rating;
                }
            }

            return Task.FromResult(handpieces);
        }

        public IReadOnlyList<HandpieceImportModel> ResolveDates(IReadOnlyList<HandpieceImportModel> handpieces)
        {
            DateTime? TryParseDate(String value)
            {
                var formats = new[]
                {
                    "MM/dd/yyyy", "MM/d/yyyy", "M/dd/yyyy", "M/d/yyyy",
                    "MM/dd/yy", "MM/d/yy", "M/dd/yy", "M/d/yy",
                    "dd.MM.yyyy", "d.MM.yyyy", "dd.M.yyyy", "d.M.yyyy",
                    "dd.MM.yy", "d.MM.yy", "dd.M.yy", "d.M.yy",
                };
                if (DateTime.TryParseExact(value, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var date))
                {
                    while (date.Date > DateTime.UtcNow.Date)
                    {
                        date = date.AddYears(-1);
                    }

                    return date;
                }

                return null;
            }

            foreach (var handpiece in handpieces)
            {
                handpiece.InvoicedDate = TryParseDate(handpiece.Invoiced);
                handpiece.BackDate = TryParseDate(handpiece.Back);

                handpiece.JobCreatedDate = handpiece.BackDate ?? handpiece.InvoicedDate;
            }

            foreach (var clientHandpieces in handpieces.Select((x, i) => new { Entity = x, Index = i }).GroupBy(x => x.Entity.Client))
            {
                foreach (var typeHandpieces in clientHandpieces.GroupBy(x => x.Entity.Type))
                {
                    var items = typeHandpieces.OrderBy(x => x.Index).Select(x => x.Entity).ToList();
                    for (var i = 0; i < items.Count; i++)
                    {
                        if (items[i].JobCreatedDate == null)
                        {
                            for (var j = i - 1; j >= 0; j--)
                            {
                                if (items[j].JobCreatedDate != null)
                                {
                                    items[i].JobCreatedDate = items[j].JobCreatedDate;
                                    break;
                                }
                            }
                        }

                        if (items[i].JobCreatedDate == null)
                        {
                            for (var j = i + 1; j < items.Count; j++)
                            {
                                if (items[j].JobCreatedDate != null)
                                {
                                    items[i].JobCreatedDate = items[j].JobCreatedDate;
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return handpieces;
        }

        public IReadOnlyList<HandpieceImportModel> FilterClientlessHandpieces(IReadOnlyList<HandpieceImportModel> handpieces)
        {
            return handpieces.Where(x => x.ClientEntity != null).ToList();
        }

        public IReadOnlyList<JobImportModel> GroupIntoJobs(IReadOnlyList<HandpieceImportModel> handpieces)
        {
            var jobIndex = 1;
            var result = new List<JobImportModel>();
            var dateless = handpieces.Where(x => x.JobCreatedDate == null).OrderBy(x => x.ClientEntity.ClientNo).ToList();
            foreach (var handpiece in dateless)
            {
                var job = new JobImportModel
                {
                    RelativeNo = jobIndex++,
                    Received = new DateTime(2010, 1, 1),
                    Client = handpiece.ClientEntity,
                    Handpieces = new List<HandpieceImportModel> { handpiece },
                };

                result.Add(job);
            }

            var grouped = handpieces.Where(x => x.JobCreatedDate != null).GroupBy(x => new
            {
                ClientNo = x.ClientEntity.ClientNo,
                Received = x.JobCreatedDate.Value,
            }).OrderBy(x => x.Key.Received).ThenBy(x => x.Key.ClientNo);

            foreach (var group in grouped)
            {
                var jobHandpieces = group.ToList();
                var job = new JobImportModel
                {
                    RelativeNo = jobIndex++,
                    Received = group.Key.Received,
                    Client = jobHandpieces[0].ClientEntity,
                    Handpieces = jobHandpieces,
                };

                result.Add(job);
            }

            return result;
        }

        public async Task ExecuteHandpiecesImport(IReadOnlyList<JobImportModel> jobs)
        {
            var importKeys = await this.repository.Query<Handpiece>()
                .Where(x => x.ImportKey != null && x.ImportKey != "")
                .Select(x => x.ImportKey)
                .ToListAsync();
            var importKeysSet = new HashSet<String>();

            var firstEmployee = await this.repository.Query<Employee>().FirstOrDefaultAsync();
            foreach (var importedJob in jobs)
            {
                var job = new Job
                {
                    ClientId = importedJob.Client.Id,
                    CreatorId = firstEmployee.Id,
                    Received = importedJob.Received,
                    ApprovedBy = String.Empty,
                    ApprovedOn = null,
                    Comment = String.Empty,
                    HasWarning = false,
                    RatePlan = JobRatePlan.Default,
                    Status = JobStatus.SentComplete,
                    Handpieces = new List<Handpiece>(),
                };

                var newHandpieces = false;
                var index = 1;
                foreach (var importedHandpiece in importedJob.Handpieces)
                {
                    if (!importKeysSet.Add(importedHandpiece.ID))
                    {
                        continue;
                    }

                    newHandpieces = true;

                    var speedType = HandpieceSpeed.Other;
                    if (importedHandpiece.Type.Equals("LowSpeed", StringComparison.InvariantCulture))
                    {
                        speedType = HandpieceSpeed.LowSpeed;
                    }
                    else if (importedHandpiece.Type.Equals("HighSpeed", StringComparison.InvariantCulture))
                    {
                        speedType = HandpieceSpeed.HighSpeed;
                    }

                    var handpiece = new Handpiece
                    {
                        JobPosition = index++,
                        ImportKey = importedHandpiece.ID,
                        MakeAndModel = importedHandpiece.Model?.Trim() ?? String.Empty,
                        Serial = importedHandpiece.Serial?.Trim() ?? String.Empty,
                        CreatorId = firstEmployee.Id,
                        DiagnosticOther = importedHandpiece.Diagnostic,
                        DiagnosticReport = importedHandpiece.Diagnostic,
                        EstimatedById = importedHandpiece.TechEntity?.Id,
                        RepairedById = importedHandpiece.TechEntity?.Id,
                        Rating = importedHandpiece.RatingParsed,
                        Parts = importedHandpiece.Parts,
                        SpeedType = speedType,
                        Speed = Int32.TryParse(importedHandpiece.RPM, out var rpm) ? rpm : 0,
                        HandpieceStatus = HandpieceStatus.SentComplete,
                        InternalComment = String.Empty,
                        PublicComment = String.Empty,
                        ProblemDescription = String.Empty,
                        PartsOutOfStock = HandpiecePartsStockStatus.InStock,
                        ServiceLevelId = null,
                        CostOfRepair = 0,
                        ReturnById = null,
                        CompletedOn = null,
                        Brand = "N/A",
                        OldMakeAndModel = importedHandpiece.Model?.Trim() ?? String.Empty,
                    };

                    job.Handpieces.Add(handpiece);
                }

                if (newHandpieces)
                {
                    job.JobNumber = await this.entitySequence.TakeNextNumberAsync("JobNumberSequenceKey");
                    await this.repository.InsertAsync(job);
                    await this.repository.SaveChangesAsync();
                }
            }
        }

        public async Task ExecuteClientsImport(IReadOnlyList<ClientImportModel> clients)
        {
            var zones = await this.repository.Query<Zone>().ToListAsync();
            var zonesMap = zones.ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);

            var states = await this.repository.Query<State>().ToListAsync();
            var statesMap = states.ToDictionary(x => x.Name, StringComparer.InvariantCultureIgnoreCase);

            Zone FindZone(String mailoutNo)
            {
                if (zonesMap.TryGetValue($"Zone {mailoutNo}", out var zone))
                {
                    return zone;
                }

                return zonesMap["N/A"];
            }

            State FindState(String name)
            {
                if (statesMap.TryGetValue(name, out var state))
                {
                    return state;
                }

                return statesMap["N/A"];
            }

            var imported = await this.repository.Query<Client>().ToListAsync();
            var importedIds = new HashSet<String>(imported.Select(x => x.ImportKey).Where(x => !String.IsNullOrEmpty(x)));
            var importedNumbers = new HashSet<Int32>(imported.Select(x => x.ClientNo));

            var maxDbNumber = imported.Any() ? imported.Max(x => x.ClientNo) : 0;
            var maxImportNumber = clients.Where(x => x.ClientNumber.HasValue).Max(x => x.ClientNumber.Value);

            var nextClientNumber = Math.Max(maxDbNumber, maxImportNumber) + 1;
            foreach (var client in clients)
            {
                if (importedIds.Contains(client.UniqueId))
                {
                    continue;
                }

                var clientNumber = client.ClientNumber ?? (nextClientNumber++);

                if (importedNumbers.Contains(clientNumber))
                {
                    continue;
                }

                var clientEmails = client.ParseEmails();
                var clientEntity = new Client
                {
                    ImportKey = client.UniqueId,
                    ClientNo = clientNumber,
                    Name = client.Company ?? $"Client {clientNumber}",
                    UrlPath = $"Client{clientNumber}",
                    PrincipalDentist = client.Contact,
                    OtherContact = client.OtherContact,
                    Suburb = client.City,
                    ZoneId = FindZone(client.MailoutNo).Id,
                    StateId = FindState(client.State).Id,
                    Address = $"{client.Address1} {client.Address2}".Trim(),
                    Brands = client.BrandsUsed,
                    Email = clientEmails.FirstOrDefault(),
                    SecondaryEmails = String.Join(",", clientEmails.Skip(1)),
                    Phone = client.Phone,
                    SecondaryPhones = $"{client.FaxPhone} {client.MobilePhone}".Trim(),
                    OpeningHours = client.OpeningHours,
                    PostCode = client.Postcode.Length > 4 ? client.Postcode.Substring(0, 4) : client.Postcode,
                };

                clientEntity.FullName = clientEntity.ComputeFullName();

                importedIds.Add(clientEntity.ImportKey);
                importedNumbers.Add(clientEntity.ClientNo);
                await this.repository.InsertAsync(clientEntity);
                await this.repository.SaveChangesAsync();
            }
        }
    }
}
