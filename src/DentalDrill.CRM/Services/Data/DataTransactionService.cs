using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DentalDrill.CRM.Services.Data
{
    public class DataTransactionService : IDataTransactionService
    {
        private readonly DbContext dbContext;

        public DataTransactionService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IDataTransaction> BeginTransactionAsync(IsolationLevel isolationLevel, CancellationToken cancellationToken)
        {
            var transaction = await this.dbContext.Database.BeginTransactionAsync(isolationLevel, cancellationToken);
            return new DataTransaction(transaction);
        }
    }
}