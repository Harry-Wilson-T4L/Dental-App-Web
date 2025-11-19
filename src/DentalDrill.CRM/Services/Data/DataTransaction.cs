using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;

namespace DentalDrill.CRM.Services.Data
{
    public sealed class DataTransaction : IDataTransaction, IDisposable, IAsyncDisposable
    {
        private readonly IDbContextTransaction transaction;

        public DataTransaction(IDbContextTransaction transaction)
        {
            this.transaction = transaction;
        }

        public Task CommitAsync(CancellationToken cancellationToken)
        {
            return this.transaction.CommitAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken)
        {
            return this.transaction.RollbackAsync(cancellationToken);
        }

        public void Dispose()
        {
            this.transaction.Dispose();
        }

        public ValueTask DisposeAsync()
        {
            return this.transaction.DisposeAsync();
        }
    }
}