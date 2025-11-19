using System;
using System.Collections.Generic;
using System.Linq;

namespace DentalDrill.CRM.Services.Generation
{
    public class BrandAndModelOptions
    {
        public BrandAndModelOptions()
        {
        }

        public BrandAndModelOptions(String makeName, params String[] modelNames)
        {
            this.MakeName = makeName;
            this.ModelNames = modelNames.ToList();
        }

        public String MakeName { get; set; }

        public List<String> ModelNames { get; set; }
    }
}