using System;

namespace DentalDrill.CRM.Services
{
    public interface INameNormalizationService
    {
        String NormalizeName(String originalName);
    }
}