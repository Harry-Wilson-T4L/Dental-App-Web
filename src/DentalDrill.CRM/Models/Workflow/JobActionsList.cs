using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models.Workflow
{
    public class JobActionsList
    {
        public JobActionsList(IReadOnlyList<WorkflowAction> actions, Boolean canSetStatus, IReadOnlyDictionary<HandpieceStatus, HandpieceStatusChangesList> statusChanges)
        {
            this.Actions = actions;
            this.CanSetStatus = canSetStatus;
            this.CanBulkChangeStatus = true;
            this.StatusChanges = statusChanges;
        }

        public JobActionsList(IReadOnlyList<WorkflowAction> actions, Boolean canSetStatus, Boolean canBulkChangeStatus, IReadOnlyDictionary<HandpieceStatus, HandpieceStatusChangesList> statusChanges)
        {
            this.Actions = actions;
            this.CanSetStatus = canSetStatus;
            this.CanBulkChangeStatus = canBulkChangeStatus;
            this.StatusChanges = statusChanges;
        }

        public IReadOnlyList<WorkflowAction> Actions { get; }

        public Boolean CanSetStatus { get; }

        public Boolean CanBulkChangeStatus { get; set; }

        public IReadOnlyDictionary<HandpieceStatus, HandpieceStatusChangesList> StatusChanges { get; }

        public static JobActionsList Create(Action<JobActionsListBuilder> builder)
        {
            var listBuilder = new JobActionsListBuilder();
            builder(listBuilder);

            return listBuilder.Build();
        }

        public Boolean CanChangeStatus(HandpieceStatus from, HandpieceStatus to)
        {
            return this.CanSetStatus && this.StatusChanges.TryGetValue(from, out var changes) && changes.Destinations.Contains(to);
        }

        public IReadOnlyList<HandpieceStatus> GetPossibleStatuses(HandpieceStatus from)
        {
            return this.CanSetStatus && this.StatusChanges.TryGetValue(from, out var changes) ? changes.Destinations : null;
        }
    }
}
