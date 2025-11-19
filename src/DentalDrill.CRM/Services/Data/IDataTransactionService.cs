using System.Data;
using System.Threading;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Services.Data
{
    public interface IDataTransactionService
    {
        Task<IDataTransaction> BeginTransactionAsync(IsolationLevel isolationLevel = IsolationLevel.ReadCommitted, CancellationToken cancellationToken = default);
    }
}
