using System;
using System.Collections.Generic;

namespace DentalDrill.CRM.Services.RandomData
{
    public interface IRandomValueGenerationService<TValueType>
    {
        TValueType GenerateSingle(IReadOnlyList<RandomValueDescriptor<TValueType>> descriptors, Double deviation);

        IEnumerable<TValueType> GenerateSequence(IReadOnlyList<RandomValueDescriptor<TValueType>> descriptors, Double deviation, Int32 numberOfValues);

        IEnumerable<TValueType> SplitValueIntoRandomParts(RangedRandomValueDescriptor<TValueType> descriptor, Int32 numberOfParts);
    }
}