using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Services.Generation
{
    public class DateDistributionConfig
    {
        public List<Double> QuarterlyWeights { get; set; }

        public Double QuarterlyDeviation { get; set; }

        public List<Double> QuarterMonthlyWeights { get; set; }

        public Double QuarterMonthlyDeviation { get; set; }

        public List<Double> WeekDailyWeights { get; set; }

        public Double WeekDailyDeviation { get; set; }
    }
}