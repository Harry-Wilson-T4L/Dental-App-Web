using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services.RandomData;

namespace DentalDrill.CRM.Services.Generation
{
    public class EntitiesGenerationSettings
    {
        public List<BrandAndModelOptions> Brands { get; set; }

        public List<RandomValueDescriptor<Int32>> CostValues { get; set; }

        public List<RandomValueDescriptor<Int32>> RatingValues { get; set; }

        public List<JobStatusOptions> Statuses { get; set; }

        public DateDistributionConfig Dates { get; set; }

        public static EntitiesGenerationSettings CreateDefault()
        {
            var settings = new EntitiesGenerationSettings();
            settings.Brands = new List<BrandAndModelOptions>
            {
                new BrandAndModelOptions(
                    "NSK",
                    "A500L", "A600", "A600L", "A700L", "AB600L", "EX-6", "EX-6B", "M500", "M500L", "M500KL", "M600", "M600L", "M600KL", "Machlite", "Machlite 2", "Machlite 2M", "Machlite 2S", "Machlite XTS", "Pana Max", "Ti25"),
                new BrandAndModelOptions(
                    "Kavo",
                    "2000L", "2000N", "20A", "20C", "20CH", "2307L", "2307LN", "2320LN", "628B", "630B", "632D", "635B", "637B", "655B", "660B", "8000B", "E680L", "K9", "M9000L", "Prophy Flex"),
                new BrandAndModelOptions(
                    "W&H",
                    "143", "896", "964", "898LE", "956LT", "999LT", "RQ-24 Coupler", "TA-96L", "TA-97CLED", "TA-98", "TA-98CLED", "TA-98L", "TA-98LCM", "WA-56A", "WA-56LT"),
                new BrandAndModelOptions(
                    "Morita",
                    "4HE", "4HE-O-NK", "4HEO", "4HEX", "4HEX-O-KV", "4HEX-O-NK", "4HEX-O-SR", "4HEX-O-WH", "4HX", "CA-DC"),
            };

            settings.CostValues = new List<RandomValueDescriptor<Int32>>
            {
                new RangedRandomValueDescriptor<Int32>(50, 150, 10, 0.5),
                new RangedRandomValueDescriptor<Int32>(150, 300, 10, 0.4),
                new RangedRandomValueDescriptor<Int32>(300, 750, 10, 0.1),
            };

            settings.RatingValues = new List<RandomValueDescriptor<Int32>>
            {
                new RangedRandomValueDescriptor<Int32>(1, 5, 1, 0.3),
                new RangedRandomValueDescriptor<Int32>(5, 8, 1, 0.6),
                new RangedRandomValueDescriptor<Int32>(8, 10, 1, 0.1),
            };

            settings.Statuses = new List<JobStatusOptions>
            {
                new JobStatusOptions
                {
                    JobStatus = JobStatus.Received,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Received, 1.0),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.BeingEstimated,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Received, 0.2),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.BeingEstimated, 0.1),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.WaitingForApproval, 0.7),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.WaitingForApproval,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.WaitingForApproval, 0.7),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TbcHoldOn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.EstimateSent, 0.2),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TradeIn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReturnUnrepaired, 0.04),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 0.02),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.EstimateSent,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TbcHoldOn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.EstimateSent, 0.5),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.BeingRepaired, 0.2),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TradeIn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReturnUnrepaired, 0.04),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 0.02),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.BeingRepaired,
                    Weight = 0.2,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TbcHoldOn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.BeingRepaired, 0.7),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReadyToReturn, 0.1),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.WaitingForParts, 0.05),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TradeIn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReturnUnrepaired, 0.04),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Unrepairable, 0.05),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.ReadyToReturn,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TbcHoldOn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.BeingRepaired, 0.2),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReadyToReturn, 0.4),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.SentComplete, 0.2),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.WaitingForParts, 0.05),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TradeIn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReturnUnrepaired, 0.04),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Unrepairable, 0.05),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.SentComplete,
                    Weight = 0.5,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TbcHoldOn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.BeingRepaired, 0.05),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReadyToReturn, 0.05),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.SentComplete, 0.7),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.WaitingForParts, 0.05),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.TradeIn, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.ReturnUnrepaired, 0.04),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 0.02),
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Unrepairable, 0.05),
                    },
                },
                new JobStatusOptions
                {
                    JobStatus = JobStatus.Cancelled,
                    Weight = 0.05,
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>
                    {
                        new SingleRandomValueDescriptor<HandpieceStatus>(HandpieceStatus.Cancelled, 1.0),
                    },
                },
            };

            settings.Dates = new DateDistributionConfig
            {
                QuarterlyWeights = new List<Double>() { 0.3, 0.15, 0.4, 0.15, },
                QuarterlyDeviation = 0.1,

                QuarterMonthlyWeights = new List<Double>() { 0.20, 0.20, 0.4, },
                QuarterMonthlyDeviation = 0.1,

                WeekDailyWeights = new List<Double>() { 0.05, 0.2, 0.15, 0.15, 0.2, 0.25, 0.1, },
                WeekDailyDeviation = 0.05,
            };

            return settings;
        }

        public String SerializeBrandsConfig()
        {
            if (this.Brands == null || this.Brands.Count == 0)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var brand in this.Brands)
            {
                sb.Append(brand.MakeName);
                foreach (var model in brand.ModelNames)
                {
                    sb.Append(";");
                    sb.Append(model);
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public (Boolean Success, String Error) TryParseBrands(String serialized)
        {
            var brands = new List<BrandAndModelOptions>();
            var lines = serialized.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return (false, "No data");
            }

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var entries = line.Split(";", StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList();
                if (entries.Count < 2)
                {
                    return (false, $"Line {i} has not enough items");
                }

                brands.Add(new BrandAndModelOptions(entries[0], entries.Skip(1).ToArray()));
            }

            this.Brands = brands;
            return (true, null);
        }

        public String SerializeCostsConfig()
        {
            if (this.CostValues == null || this.CostValues.Count == 0)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var cost in this.CostValues)
            {
                switch (cost)
                {
                    case RangedRandomValueDescriptor<Int32> ranged:
                        sb.Append(String.Format(CultureInfo.InvariantCulture, "{0};{1};{2};{3}", ranged.MinValue, ranged.MaxValue, ranged.Step, ranged.Weight));
                        break;
                    case SingleRandomValueDescriptor<Int32> single:
                        sb.Append(String.Format(CultureInfo.InvariantCulture, "{0};{1}", single.Value, single.Weight));
                        break;
                    default:
                        throw new NotSupportedException();
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public (Boolean Success, String Error) TryParseCosts(String serialized)
        {
            var costs = new List<RandomValueDescriptor<Int32>>();
            var lines = serialized.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return (false, "No data");
            }

            var rangedRegex = new Regex(@"^(?<min>\d+);(?<max>\d+);(?<step>\d+);(?<weight>\d+(?:\.\d+)?)$");
            var singleRegex = new Regex(@"^(?<value>\d+);(?<weight>\d+(?:\.\d+)?)$");
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                Match match;
                if ((match = rangedRegex.Match(line)).Success)
                {
                    costs.Add(new RangedRandomValueDescriptor<Int32>(
                        Int32.Parse(match.Groups["min"].Value, CultureInfo.InvariantCulture),
                        Int32.Parse(match.Groups["max"].Value, CultureInfo.InvariantCulture),
                        Int32.Parse(match.Groups["step"].Value, CultureInfo.InvariantCulture),
                        Double.Parse(match.Groups["weight"].Value, CultureInfo.InvariantCulture)));
                }
                else if ((match = singleRegex.Match(line)).Success)
                {
                    costs.Add(new SingleRandomValueDescriptor<Int32>(
                        Int32.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture),
                        Double.Parse(match.Groups["weight"].Value, CultureInfo.InvariantCulture)));
                }
                else
                {
                    return (false, $"Line {i} is invalid");
                }
            }

            this.CostValues = costs;
            return (true, null);
        }

        public String SerializeRatingsConfig()
        {
            if (this.RatingValues == null || this.RatingValues.Count == 0)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var cost in this.RatingValues)
            {
                switch (cost)
                {
                    case RangedRandomValueDescriptor<Int32> ranged:
                        sb.Append(String.Format(CultureInfo.InvariantCulture, "{0};{1};{2};{3}", ranged.MinValue, ranged.MaxValue, ranged.Step, ranged.Weight));
                        break;
                    case SingleRandomValueDescriptor<Int32> single:
                        sb.Append(String.Format(CultureInfo.InvariantCulture, "{0};{1}", single.Value, single.Weight));
                        break;
                    default:
                        throw new NotSupportedException();
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public (Boolean Success, String Error) TryParseRatings(String serialized)
        {
            var ratings = new List<RandomValueDescriptor<Int32>>();
            var lines = serialized.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return (false, "No data");
            }

            var rangedRegex = new Regex(@"^(?<min>\d+);(?<max>\d+);(?<step>\d+);(?<weight>\d+(?:\.\d+)?)$");
            var singleRegex = new Regex(@"^(?<value>\d+);(?<weight>\d+(?:\.\d+)?)$");
            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                Match match;
                if ((match = rangedRegex.Match(line)).Success)
                {
                    ratings.Add(new RangedRandomValueDescriptor<Int32>(
                        Int32.Parse(match.Groups["min"].Value, CultureInfo.InvariantCulture),
                        Int32.Parse(match.Groups["max"].Value, CultureInfo.InvariantCulture),
                        Int32.Parse(match.Groups["step"].Value, CultureInfo.InvariantCulture),
                        Double.Parse(match.Groups["weight"].Value, CultureInfo.InvariantCulture)));
                }
                else if ((match = singleRegex.Match(line)).Success)
                {
                    ratings.Add(new SingleRandomValueDescriptor<Int32>(
                        Int32.Parse(match.Groups["value"].Value, CultureInfo.InvariantCulture),
                        Double.Parse(match.Groups["weight"].Value, CultureInfo.InvariantCulture)));
                }
                else
                {
                    return (false, $"Line {i} is invalid");
                }
            }

            this.RatingValues = ratings;
            return (true, null);
        }

        public String SerializeStatusesConfig()
        {
            if (this.Statuses == null || this.Statuses.Count == 0)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            foreach (var status in this.Statuses)
            {
                sb.Append(String.Format(CultureInfo.InvariantCulture, "{0}:{1}", status.JobStatus, status.Weight));
                foreach (var handpieceStatus in status.HandpieceStatuses)
                {
                    switch (handpieceStatus)
                    {
                        case SingleRandomValueDescriptor<HandpieceStatus> single:
                            sb.Append(String.Format(CultureInfo.InvariantCulture, ";{0}:{1}", single.Value, single.Weight));
                            break;
                        default:
                            throw new NotSupportedException();
                    }
                }

                sb.AppendLine();
            }

            return sb.ToString();
        }

        public (Boolean Success, String Error) TryParseStatuses(String serialized)
        {
            var statuses = new List<JobStatusOptions>();
            var lines = serialized.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length == 0)
            {
                return (false, "No data");
            }

            for (var i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var entries = line.Split(";", StringSplitOptions.RemoveEmptyEntries);
                if (entries.Length < 2)
                {
                    return (false, $"Line {i} has too few entries");
                }

                var jobParts = entries[0].Split(":");
                var jobStatusConfig = new JobStatusOptions
                {
                    JobStatus = Enum.Parse<JobStatus>(jobParts[0], true),
                    Weight = Double.Parse(jobParts[1], CultureInfo.InvariantCulture),
                    HandpieceStatuses = new List<RandomValueDescriptor<HandpieceStatus>>(),
                };

                for (var j = 1; j < entries.Length; j++)
                {
                    var handpieceParts = entries[j].Split(":");
                    jobStatusConfig.HandpieceStatuses.Add(new SingleRandomValueDescriptor<HandpieceStatus>(
                        Enum.Parse<HandpieceStatus>(handpieceParts[0], true),
                        Double.Parse(handpieceParts[1], CultureInfo.InvariantCulture)));
                }

                statuses.Add(jobStatusConfig);
            }

            this.Statuses = statuses;
            return (true, null);
        }

        public String SerializeDatesConfig()
        {
            if (this.Dates == null)
            {
                return String.Empty;
            }

            var sb = new StringBuilder();
            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "QW:{0};{1};{2};{3}",
                this.Dates.QuarterlyWeights[0],
                this.Dates.QuarterlyWeights[1],
                this.Dates.QuarterlyWeights[2],
                this.Dates.QuarterlyWeights[3]));
            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "QD:{0}",
                this.Dates.QuarterlyDeviation));

            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "MW:{0};{1};{2}",
                this.Dates.QuarterMonthlyWeights[0],
                this.Dates.QuarterMonthlyWeights[1],
                this.Dates.QuarterMonthlyWeights[2]));
            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "MD:{0}",
                this.Dates.QuarterMonthlyDeviation));

            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "DW:{0};{1};{2};{3};{4};{5};{6}",
                this.Dates.WeekDailyWeights[0],
                this.Dates.WeekDailyWeights[1],
                this.Dates.WeekDailyWeights[2],
                this.Dates.WeekDailyWeights[3],
                this.Dates.WeekDailyWeights[4],
                this.Dates.WeekDailyWeights[5],
                this.Dates.WeekDailyWeights[6]));
            sb.AppendLine(String.Format(CultureInfo.InvariantCulture, "DD:{0}",
                this.Dates.WeekDailyDeviation));

            return sb.ToString();
        }

        public (Boolean Success, String Error) TryParseDates(String serialized)
        {
            var config = new DateDistributionConfig();
            var lines = serialized.Replace("\r\n", "\n").Replace("\r", "\n").Split("\n", StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 6)
            {
                return (false, "Invalid format");
            }

            Match match;
            if ((match = Regex.Match(lines[0], @"^QW:(?<q1>\d+(?:\.\d+)?);(?<q2>\d+(?:\.\d+)?);(?<q3>\d+(?:\.\d+)?);(?<q4>\d+(?:\.\d+)?)$")).Success)
            {
                config.QuarterlyWeights = new List<Double>
                {
                    Double.Parse(match.Groups["q1"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["q2"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["q3"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["q4"].Value, CultureInfo.InvariantCulture),
                };
            }
            else
            {
                return (false, "Invalid quarterly weights");
            }

            if ((match = Regex.Match(lines[1], @"^QD:(?<dev>\d+(?:\.\d+)?)$")).Success)
            {
                config.QuarterlyDeviation = Double.Parse(match.Groups["dev"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                return (false, "Invalid quarterly deviation");
            }

            if ((match = Regex.Match(lines[2], @"^MW:(?<m1>\d+(?:\.\d+)?);(?<m2>\d+(?:\.\d+)?);(?<m3>\d+(?:\.\d+)?)$")).Success)
            {
                config.QuarterMonthlyWeights = new List<Double>
                {
                    Double.Parse(match.Groups["m1"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["m2"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["m3"].Value, CultureInfo.InvariantCulture),
                };
            }
            else
            {
                return (false, "Invalid monthly weights");
            }

            if ((match = Regex.Match(lines[3], @"^MD:(?<dev>\d+(?:\.\d+)?)$")).Success)
            {
                config.QuarterMonthlyDeviation = Double.Parse(match.Groups["dev"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                return (false, "Invalid monthly deviation");
            }

            if ((match = Regex.Match(lines[4], @"^DW:(?<d1>\d+(?:\.\d+)?);(?<d2>\d+(?:\.\d+)?);(?<d3>\d+(?:\.\d+)?);(?<d4>\d+(?:\.\d+)?);(?<d5>\d+(?:\.\d+)?);(?<d6>\d+(?:\.\d+)?);(?<d7>\d+(?:\.\d+)?)$")).Success)
            {
                config.WeekDailyWeights = new List<Double>
                {
                    Double.Parse(match.Groups["d1"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d2"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d3"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d4"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d5"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d6"].Value, CultureInfo.InvariantCulture),
                    Double.Parse(match.Groups["d7"].Value, CultureInfo.InvariantCulture),
                };
            }
            else
            {
                return (false, "Invalid daily weights");
            }

            if ((match = Regex.Match(lines[5], @"^DD:(?<dev>\d+(?:\.\d+)?)$")).Success)
            {
                config.WeekDailyDeviation = Double.Parse(match.Groups["dev"].Value, CultureInfo.InvariantCulture);
            }
            else
            {
                return (false, "Invalid monthly deviation");
            }

            this.Dates = config;
            return (true, null);
        }
    }
}
