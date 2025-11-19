namespace DentalDrill.CRM.Models
{
    public enum ClientRepairedItemStatus
    {
        RequiresMaintenance = 0,
        RemindedRecently = 1,
        Complete = 2,
        Active = 3,
        ReminderExpired = 4,
        Cancelled = 5,
        Disabled = 6,
    }
}
