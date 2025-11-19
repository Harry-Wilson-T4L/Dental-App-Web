using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.Workflow
{
    public class JobActionsListBuilder
    {
        private readonly List<WorkflowAction> actions = new List<WorkflowAction>();
        private readonly Dictionary<HandpieceStatus, HandpieceStatusChangesList> statusChanges = new Dictionary<HandpieceStatus, HandpieceStatusChangesList>();
        private IEmployeeAccessWorkshopJobType jobTypeAccess = null;
        private JobStatus? jobCurrentStatus = null;
        private Boolean canSetStatus = false;
        private Boolean canBulkChangeStatus = true;

        public JobActionsListBuilder UseAccessControl(IEmployeeAccessWorkshopJobType jobTypeAccess, JobStatus currentStatus)
        {
            this.jobTypeAccess = jobTypeAccess;
            this.jobCurrentStatus = currentStatus;
            return this;
        }

        public JobActionsListBuilder Action(String title, JobStatus destinationStatus, String url, Boolean enabled = true)
        {
            if (this.jobTypeAccess != null && !this.jobTypeAccess.CanPerformJobTransition(this.jobCurrentStatus!.Value, destinationStatus))
            {
                return this;
            }

            this.actions.Add(new WorkflowAction(title, url, enabled));
            return this;
        }

        public JobActionsListBuilder CanSetStatusFrom(HandpieceStatus source, params HandpieceStatus[] destination)
        {
            if (this.jobTypeAccess == null)
            {
                this.canSetStatus = true;
                this.statusChanges.Add(source, new HandpieceStatusChangesList(source, destination));
                return this;
            }

            var allowedDestinations = destination.Where(x => this.jobTypeAccess.CanPerformHandpieceTransition(this.jobCurrentStatus!.Value, source, x)).ToList();
            if (allowedDestinations.Count > 0 && !(allowedDestinations.Count == 1 && allowedDestinations[0] == source))
            {
                this.canSetStatus = true;
                this.statusChanges.Add(source, new HandpieceStatusChangesList(source, allowedDestinations));
                return this;
            }

            return this;
        }

        public JobActionsListBuilder CanBulkChangeStatus(Boolean canBulkChangeStatus)
        {
            this.canBulkChangeStatus = canBulkChangeStatus;
            return this;
        }

        public JobActionsList Build()
        {
            return new JobActionsList(this.actions, this.canSetStatus, this.canBulkChangeStatus, this.statusChanges);
        }
    }
}
