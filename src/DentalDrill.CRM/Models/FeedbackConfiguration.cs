using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public enum FeedbackConfigurationPeriodType
    {
        Day,
        Week,
        Month,
        Quarter,
        Year,
    }

    public class FeedbackConfiguration
    {
        public static Guid Default { get; } = Guid.Parse("{D99E3E14-3FB1-4ADB-8724-5A5EA76186EC}");

        public Guid Id { get; set; }

        public Boolean AutoSendFormEnabled { get; set; }

        public Int32 AutoSendFormDelayDays { get; set; }

        public Int32 AutoSendSkipJobs { get; set; }

        public Boolean PeriodLimitingEnabled { get; set; }

        public FeedbackConfigurationPeriodType PeriodLimitingType { get; set; }

        public Int32 PeriodLimitingLength { get; set; }

        public Int32 PeriodLimitingQuantity { get; set; }
    }
}
