using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class FeedbackConfigurationEditModel
    {
        [BindNever]
        public FeedbackConfiguration Entity { get; set; }

        [Display(Name = "Automatic sending enabled")]
        public Boolean AutoSendFormEnabled { get; set; }

        [Display(Name = "Automatic sending delay (days)")]
        public Int32 AutoSendFormDelayDays { get; set; }

        [Display(Name = "Number of jobs to skip before sending feedback")]
        public Int32 AutoSendSkipJobs { get; set; }

        [Display(Name = "Period limiting enabled")]
        public Boolean PeriodLimitingEnabled { get; set; }

        [Display(Name = "Period type")]
        public FeedbackConfigurationPeriodType PeriodLimitingType { get; set; }

        [Display(Name = "Period length")]
        public Int32 PeriodLimitingLength { get; set; }

        [Display(Name = "Maximum number of forms per period")]
        public Int32 PeriodLimitingQuantity { get; set; }
    }
}
