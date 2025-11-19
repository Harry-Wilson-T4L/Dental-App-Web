using System;
using System.Collections.Generic;
using System.Linq;
using DentalDrill.CRM.Services.Calculation;

namespace DentalDrill.CRM.Services.RandomData
{
    public class RandomValueGenerationService<TValueType> : IRandomValueGenerationService<TValueType>
    {
        private readonly IRandomDataSource randomDataSource;
        private readonly IGenericArithmeticService<TValueType> arithmeticService;

        public RandomValueGenerationService(IRandomDataSource randomDataSource, IGenericArithmeticService<TValueType> arithmeticService)
        {
            this.randomDataSource = randomDataSource;
            this.arithmeticService = arithmeticService;
        }

        public TValueType GenerateSingle(IReadOnlyList<RandomValueDescriptor<TValueType>> descriptors, Double deviation) => this.GenerateSequence(descriptors, deviation, 1).First();

        public IEnumerable<TValueType> GenerateSequence(IReadOnlyList<RandomValueDescriptor<TValueType>> descriptors, Double deviation, Int32 numberOfValues)
        {
            var prepared = this.WeightDescriptors(descriptors, deviation);
            for (var i = 0; i < numberOfValues; i++)
            {
                var descriptor = this.SelectRandomDescriptor(prepared);
                yield return this.GenerateValueFromDescriptor(descriptor);
            }
        }

        public IEnumerable<TValueType> SplitValueIntoRandomParts(RangedRandomValueDescriptor<TValueType> descriptor, Int32 numberOfParts)
        {
            var breakpoints = this.GenerateSequence(new[] { descriptor }, 0, numberOfParts - 1).ToList();
            breakpoints.Add(descriptor.MinValue);
            breakpoints.Add(descriptor.MaxValue);

            breakpoints = breakpoints.OrderBy(x => x, Comparer<TValueType>.Create((first, second) => this.arithmeticService.Compare(first, second))).ToList();
            for (var i = 0; i < breakpoints.Count - 1; i++)
            {
                yield return this.arithmeticService.Subtract(breakpoints[i + 1], breakpoints[i]);
            }
        }

        private TValueType GenerateValueFromDescriptor(RandomValueDescriptor<TValueType> descriptor)
        {
            switch (descriptor)
            {
                case RangedRandomValueDescriptor<TValueType> ranged:
                    var range = this.arithmeticService.Subtract(ranged.MaxValue, ranged.MinValue);
                    var quotient = this.arithmeticService.Quotient(range, ranged.Step);
                    var steps = (Int32)(this.randomDataSource.GenerateDouble() * quotient);
                    return this.arithmeticService.Add(ranged.MinValue, this.arithmeticService.Multiply(ranged.Step, steps));
                case SingleRandomValueDescriptor<TValueType> single:
                    return single.Value;
                default:
                    throw new NotImplementedException();
            }
        }

        private List<(RandomValueDescriptor<TValueType> Descriptor, Double NormalizedWeight)> WeightDescriptors(IReadOnlyList<RandomValueDescriptor<TValueType>> descriptors, Double deviation)
        {
            var adjustedDescriptors = descriptors.Select(x => new
            {
                Descriptor = x,
                Weight = x.Weight,
                AdjustedWeight = Math.Abs(x.Weight + (this.randomDataSource.GenerateDouble() * deviation - (deviation / 2))),
            }).ToList();

            var result = new List<(RandomValueDescriptor<TValueType> Descriptor, Double NormalizedWeight)>();
            var weightSum = adjustedDescriptors.Sum(x => x.AdjustedWeight);
            var accumulatedWeight = 0.0;
            foreach (var descriptor in adjustedDescriptors)
            {
                accumulatedWeight += descriptor.AdjustedWeight / weightSum;
                result.Add((descriptor.Descriptor, accumulatedWeight));
            }

            return result;
        }

        private RandomValueDescriptor<TValueType> SelectRandomDescriptor(List<(RandomValueDescriptor<TValueType> Descriptor, Double NormalizedWeight)> descriptors)
        {
            var randomValue = this.randomDataSource.GenerateDouble();
            foreach (var descriptor in descriptors)
            {
                if (randomValue <= descriptor.NormalizedWeight)
                {
                    return descriptor.Descriptor;
                }
            }

            return descriptors[descriptors.Count - 1].Descriptor;
        }
    }
}
