using System.IO;
using System.Text;
using System.Threading.Tasks;
using DevGuild.AspNetCore.Services.Data;
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.Mail.Models;
using DevGuild.AspNetCore.Services.Storage;

namespace DentalDrill.CRM.Services.Emails
{
    public class SeparateContentEmailServiceRepository : IEmailServiceRepository
    {
        private readonly IRepositoryFactory repositoryFactory;
        private readonly IStorageHub storageHub;

        public SeparateContentEmailServiceRepository(IRepositoryFactory repositoryFactory, IStorageHub storageHub)
        {
            this.repositoryFactory = repositoryFactory;
            this.storageHub = storageHub;
        }

        public async Task StorePreparedMessageAsync(EmailMessage message)
        {
            var content = message.Content;
            message.Content = null;
            using var repository = this.repositoryFactory.CreateRepository();
            await repository.InsertAsync(message);
            await repository.SaveChangesAsync();

            var emailsStorage = this.storageHub.GetContainer("Emails");
            await using var contentStream = new MemoryStream(Encoding.UTF8.GetBytes(content));
            await emailsStorage.StoreFileAsync($"{message.MessageType}/Message-{message.Id:00000000}.eml", contentStream);
        }

        public async Task StoreSendingResultAsync(EmailMessage message, EmailSendingResult result)
        {
            using var repository = this.repositoryFactory.CreateRepository();
            if (result.Success)
            {
                message.Status = EmailMessageStatus.Sent;
                message.MessageId = result.MessageId;
            }
            else
            {
                message.Status = EmailMessageStatus.Failed;
                message.SendingError = result.Error;
            }

            await repository.UpdateAsync(message);
            await repository.SaveChangesAsync();
        }
    }
}
