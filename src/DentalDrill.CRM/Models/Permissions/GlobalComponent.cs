using System;

namespace DentalDrill.CRM.Models.Permissions
{
    [Flags]
    public enum GlobalComponent : Int64
    {
        None = 0,

        User = 1 << 0,
        Employee = 1 << 1,
        EmployeeRole = 1 << 2,
        Workshop = 1 << 3,
        Corporate = 1 << 4,

        PickupRequest = 1 << 5,
        CorporatePricing = 1 << 6,
        Feedback = 1 << 7,
        Report = 1 << 8,

        EmailTemplate = 1 << 9,
        EmailReminder = 1 << 10,

        HandpiecesDirectory = 1 << 11,
        HandpiecesStore = 1 << 12,
        HandpiecesOrder = 1 << 13,

        State = 1 << 14,
        Zone = 1 << 15,
        DiagnosticChecklist = 1 << 16,
        ProblemOption = 1 << 17,
        ReturnEstimate = 1 << 18,
        ServiceLevel = 1 << 19,

        Tutorial = 1 << 20,

        All = (1 << 21) - 1,
    }
}
