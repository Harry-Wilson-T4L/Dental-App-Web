using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobCreateModelHandpiece
    {
        public Int32? Position { get; set; }

        public Guid? ClientHandpieceId { get; set; }

        public String Brand { get; set; }

        public String Model { get; set; }

        public String Serial { get; set; }

        public List<JobCreateModelHandpieceComponent> Components { get; set; }

        public String ProblemDescription { get; set; }

        public static List<JobCreateModelHandpiece> ParseJson(String json)
        {
            return JsonConvert.DeserializeObject<List<JobCreateModelHandpiece>>(json);
        }
    }
}
