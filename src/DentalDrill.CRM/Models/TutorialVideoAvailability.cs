using System;

namespace DentalDrill.CRM.Models
{
    [Flags]
    public enum TutorialVideoAvailability
    {
        None = 0,
        Client = 1,
        Corporate = 2,
        Staff = 4,

        All = TutorialVideoAvailability.Client |
              TutorialVideoAvailability.Corporate |
              TutorialVideoAvailability.Staff,
    }
}
