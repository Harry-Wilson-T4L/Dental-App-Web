using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services.RandomData;
using DevGuild.AspNetCore.Contracts;
using DevGuild.AspNetCore.Services.EntitySequences;

namespace DentalDrill.CRM.Services.Generation
{
    public class EntitiesGenerationService
    {
        private readonly IEntitySequenceService sequenceService;
        private readonly IRandomDataSource randomDataSource;
        private readonly IRandomValueGenerationService<Int32> integerRng;
        private readonly IRandomValueGenerationService<String> stringRng;
        private readonly IRandomValueGenerationService<JobStatus> jobStatusRng;
        private readonly IRandomValueGenerationService<HandpieceStatus> handpieceStatusRng;

        public EntitiesGenerationService(IEntitySequenceService sequenceService, IRandomDataSource randomDataSource, IRandomValueGenerationService<Int32> integerRng, IRandomValueGenerationService<String> stringRng, IRandomValueGenerationService<JobStatus> jobStatusRng, IRandomValueGenerationService<HandpieceStatus> handpieceStatusRng)
        {
            this.sequenceService = sequenceService;
            this.randomDataSource = randomDataSource;
            this.integerRng = integerRng;
            this.stringRng = stringRng;
            this.jobStatusRng = jobStatusRng;
            this.handpieceStatusRng = handpieceStatusRng;
        }

        public async Task<List<Job>> GenerateJobs(EntitiesGenerationSettings settings, EntitiesGenerationRuntimeSettings runtimeSettings, HandpieceImagesLoader imagesLoader)
        {
            var handpieces = this.GenerateHandpieces(settings, runtimeSettings);
            var jobs = new List<Job>();

            // Creating jobs
            foreach (var group in handpieces.GroupBy(x => new { x.JobStatus, x.Client, x.Received }))
            {
                var job = new Job
                {
                    CreatorId = runtimeSettings.Employee.Id,
                    Received = group.Key.Received,
                    ClientId = group.Key.Client.Id,
                    Status = group.Key.JobStatus,

                    Handpieces = new Collection<Handpiece>(group.Select(x => x.Handpiece).ToList()),
                };

                jobs.Add(job);
            }

            // Assigning jobs numbers and handpiece indexes
            var numbers = await this.sequenceService.TakeMultipleNumbersAsync("JobNumberSequenceKey", jobs.Count);
            for (var i = 0; i < jobs.Count; i++)
            {
                jobs[i].JobNumber = numbers[i];

                var position = 1;
                foreach (var handpiece in jobs[i].Handpieces)
                {
                    handpiece.JobPosition = position++;
                }
            }

            // Loading images
            foreach (var job in jobs)
            {
                foreach (var handpiece in job.Handpieces)
                {
                    var images = imagesLoader.SelectImage(handpiece.Brand, handpiece.MakeAndModel);
                    if (images.Length > 0)
                    {
                        var randomImage = this.randomDataSource.GenerateInt32() % images.Length;
                        var resolvedImage = await imagesLoader.ResolveImage(images[randomImage].FullName);

                        handpiece.Images.Add(new HandpieceImage
                        {
                            Date = job.Received,
                            Display = true,
                            Description = String.Empty,
                            ImageId = resolvedImage.Id,
                        });
                    }
                }
            }

            // Reassigning serials
            foreach (var client in jobs.GroupBy(x => x.ClientId))
            {
                var allHandpieces = client.SelectMany(x => x.Handpieces.Select(y => new { Job = x, Handpiece = y })).ToList();
                foreach (var combined in allHandpieces)
                {
                    var job = combined.Job;
                    var handpiece = combined.Handpiece;

                    if (handpiece.HandpieceStatus == HandpieceStatus.SentComplete && this.randomDataSource.GenerateDouble() < 0.8)
                    {
                        var potentialMatches = allHandpieces
                            .Where(x =>
                                x.Handpiece.Brand == handpiece.Brand &&
                                x.Handpiece.MakeAndModel == handpiece.MakeAndModel &&
                                x.Handpiece.HandpieceStatus != HandpieceStatus.SentComplete &&
                                x.Job.Received > job.Received)
                            .ToList();

                        if (potentialMatches.Count > 0)
                        {
                            var match = potentialMatches[this.randomDataSource.GenerateInt32() % potentialMatches.Count];
                            handpiece.Serial = match.Handpiece.Serial;
                        }
                    }
                }
            }

            return jobs;
        }

        private IEnumerable<(Client Client, Handpiece Handpiece, JobStatus JobStatus, DateTime Received)> GenerateHandpieces(EntitiesGenerationSettings settings, EntitiesGenerationRuntimeSettings runtimeSettings)
        {
            var clientsHandpieces = this.integerRng.SplitValueIntoRandomParts(new RangedRandomValueDescriptor<Int32>(0, runtimeSettings.NumberOfHandpieces, 1, 1.0), runtimeSettings.Clients.Count).ToList();
            var clients = runtimeSettings.Clients.Select((x, i) => Enumerable.Repeat(x, clientsHandpieces[i])).SelectMany(x => x).ToList();
            var brands = this.GenerateBrandsAndModelsSequence(settings.Brands, runtimeSettings.NumberOfHandpieces).ToList();
            var serial1 = this.integerRng.GenerateSequence(new List<RandomValueDescriptor<Int32>> { new RangedRandomValueDescriptor<Int32>(0, 999, 1, 1.0) }, 0, runtimeSettings.NumberOfHandpieces).ToList();
            var serial2 = this.integerRng.GenerateSequence(new List<RandomValueDescriptor<Int32>> { new RangedRandomValueDescriptor<Int32>(0, 999, 1, 1.0) }, 0, runtimeSettings.NumberOfHandpieces).ToList();

            var jobStatuses = this.jobStatusRng.GenerateSequence(settings.Statuses.Select(x => new SingleRandomValueDescriptor<JobStatus>(x.JobStatus, x.Weight)).ToList(), 0.02, runtimeSettings.NumberOfHandpieces).ToList();
            var handpieceStatusesDescriptors = settings.Statuses.ToDictionary(x => x.JobStatus, x => x.HandpieceStatuses);
            var handpieceStatuses = handpieceStatusesDescriptors.ToDictionary(x => x.Key, x => this.handpieceStatusRng.GenerateSequence(x.Value, 0.02, runtimeSettings.NumberOfHandpieces).ToList());

            var costs = this.integerRng.GenerateSequence(settings.CostValues, 0.1, runtimeSettings.NumberOfHandpieces).ToList();
            var ratings = this.integerRng.GenerateSequence(settings.RatingValues, 0.1, runtimeSettings.NumberOfHandpieces).ToList();

            var dates = this.GenerateDateSequence(runtimeSettings.Year, settings.Dates, runtimeSettings.NumberOfHandpieces).ToList();

            for (var i = 0; i < runtimeSettings.NumberOfHandpieces; i++)
            {
                var jobStatus = jobStatuses[i];
                var handpieceStatus = handpieceStatuses[jobStatus][i];

                var handpiece = new Handpiece
                {
                    Brand = brands[i].Brand,
                    MakeAndModel = brands[i].Model,
                    Serial = $"{brands[i].Brand.Substring(0, 1)}-{serial1[i]:000}-{serial2[i]:000}",
                    HandpieceStatus = handpieceStatus,

                    CreatorId = runtimeSettings.Employee.Id,
                    EstimatedById = runtimeSettings.Employee.Id,
                    RepairedById = runtimeSettings.Employee.Id,

                    CostOfRepair = costs[i],
                    Rating = ratings[i],

                    Images = new List<HandpieceImage>(),
                };

                if (handpieceStatus == HandpieceStatus.Received)
                {
                    handpiece.EstimatedById = null;
                }

                if (handpieceStatus != HandpieceStatus.ReadyToReturn && handpieceStatus != HandpieceStatus.SentComplete)
                {
                    handpiece.RepairedById = null;
                }

                yield return (clients[i], handpiece, jobStatus, dates[i]);
            }
        }

        private IEnumerable<DateTime> GenerateDateSequence(Int32 year, DateDistributionConfig config, Int32 numberOfItems)
        {
            Ensure.Argument.HasExactNumberOfElements(config.QuarterlyWeights, 4, nameof(config), "Quarter weights must contain exactly 4 elements");
            Ensure.Argument.HasExactNumberOfElements(config.QuarterMonthlyWeights, 3, "Months weights must contain exactly 3 elements");
            Ensure.Argument.HasExactNumberOfElements(config.WeekDailyWeights, 7, nameof(config), "Week day weights must contain exactly 7 elements");

            var quarterDescriptors = new List<RandomValueDescriptor<Int32>>
            {
                new SingleRandomValueDescriptor<Int32>(0, config.QuarterlyWeights[0]),
                new SingleRandomValueDescriptor<Int32>(1, config.QuarterlyWeights[1]),
                new SingleRandomValueDescriptor<Int32>(2, config.QuarterlyWeights[2]),
                new SingleRandomValueDescriptor<Int32>(3, config.QuarterlyWeights[3]),
            };

            var monthDescriptors = new List<RandomValueDescriptor<Int32>>
            {
                new SingleRandomValueDescriptor<Int32>(0, config.QuarterMonthlyWeights[0]),
                new SingleRandomValueDescriptor<Int32>(1, config.QuarterMonthlyWeights[1]),
                new SingleRandomValueDescriptor<Int32>(2, config.QuarterMonthlyWeights[2]),
            };

            var daysOfMonths = new List<SingleRandomValueDescriptor<Int32>>[12];
            for (var i = 0; i < 12; i++)
            {
                var month = i + 1;
                var startDay = new DateTime(year, month, 1);
                var endDay = month < 12 ? new DateTime(year, month + 1, 1) : new DateTime(year + 1, 1, 1);
                var days = (Int32)(endDay - startDay).TotalDays;
                daysOfMonths[i] = new List<SingleRandomValueDescriptor<Int32>>();
                for (var j = 0; j < days; j++)
                {
                    var day = j + 1;
                    var date = new DateTime(year, month, day);
                    daysOfMonths[i].Add(new SingleRandomValueDescriptor<Int32>(j, config.WeekDailyWeights[(Int32)date.DayOfWeek]));
                }
            }

            var quartersSequence = this.integerRng.GenerateSequence(quarterDescriptors, config.QuarterlyDeviation, numberOfItems).ToList();
            var monthsSequence = this.integerRng.GenerateSequence(monthDescriptors, config.QuarterMonthlyDeviation, numberOfItems).ToList();
            var daysSequences = daysOfMonths.Select(x => this.integerRng.GenerateSequence(x, config.WeekDailyDeviation, numberOfItems).ToList()).ToArray();

            for (var i = 0; i < numberOfItems; i++)
            {
                var quarter = quartersSequence[i];
                var month = quarter * 3 + monthsSequence[i] + 1;
                var day = daysSequences[month - 1][i] + 1;

                yield return new DateTime(year, month, day);
            }
        }

        private IEnumerable<(String Brand, String Model)> GenerateBrandsAndModelsSequence(List<BrandAndModelOptions> brandsSettings, Int32 numberOfItems)
        {
            var brandsDescriptors = brandsSettings.Select(x => new SingleRandomValueDescriptor<String>(x.MakeName, 1.0)).ToList();
            var brands = this.stringRng.GenerateSequence(brandsDescriptors, 0.2, numberOfItems).ToList();

            var modelsDescriptors = brandsSettings.ToDictionary(
                x => x.MakeName,
                x => x.ModelNames.Select(y => new SingleRandomValueDescriptor<String>(y, 1.0)).ToList());
            var models = modelsDescriptors.ToDictionary(x => x.Key, x => this.stringRng.GenerateSequence(x.Value, 0.2, numberOfItems).ToList());

            for (var i = 0; i < numberOfItems; i++)
            {
                var brand = brands[i];
                var model = models[brand][i];
                yield return (brand, model);
            }
        }
    }
}
