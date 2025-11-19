using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ClientRepairedItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }

        public String ClientName { get; set; }

        public String ClientEmail { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }

        public String LastRepair { get; set; }

        public String LastRepairUrl { get; set; }

        public HandpieceStatus LastRepairStatus { get; set; }

        public DateTime LastRepairDate { get; set; }

        public DateTime? RemindersLastDateTime { get; set; }

        public Int32 RemindersCount { get; set; }

        public Int32 TotalRemindersCount { get; set; }

        public ClientRepairedItemStatus Status { get; set; }
    }
}
