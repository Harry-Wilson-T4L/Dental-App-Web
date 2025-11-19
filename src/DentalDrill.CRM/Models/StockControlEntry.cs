using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum StockControlEntryStatus
    {
        Active = 0,
        Ignored = 1,
        Ordered = 2,
    }

    public class StockControlEntry
    {
        public Guid Id { get; set; }

        public DateTimeOffset CompletedAt { get; set; }

        public Guid HandpieceId { get; set; }

        public Handpiece Handpiece { get; set; }

        public Guid WeekId { get; set; }

        public CalendarWeek Week { get; set; }

        public StockControlEntryStatus Status { get; set; }
    }
}
