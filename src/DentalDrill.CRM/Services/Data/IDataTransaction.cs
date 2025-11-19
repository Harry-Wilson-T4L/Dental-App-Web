using System;
using System.Threading;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Data
{
    public interface IDataTransaction : IDisposable, IAsyncDisposable
    {
        Task CommitAsync(CancellationToken cancellationToken = default);

        Task RollbackAsync(CancellationToken cancellationToken = default);
    }
}