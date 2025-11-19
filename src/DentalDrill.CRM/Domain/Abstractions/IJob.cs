using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;

namespace DentalDrill.CRM.Domain.Abstractions
{
    public interface IJob
    {
        Guid Id { get; }

        IWorkshop Workshop { get; }

        IClient Client { get; }

        IJobType JobType { get; }

        Int64 JobNumber { get; }

        DateTime Received { get; }

        String Comment { get; }

        JobStatus Status { get; }

        Boolean HasWarning { get; }

        String ApprovedBy { get; }

        DateTime? ApprovedOn { get; }

        JobRatePlan RatePlan { get; }

        IHandpieceCollection Handpieces { get; }

        Task ChangeClientAsync(IClient client);

        Task ChangeWorkshopAsync(IWorkshop workshop);
    }
}
