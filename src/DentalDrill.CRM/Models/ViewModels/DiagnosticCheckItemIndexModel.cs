using System.Collections.Generic;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class DiagnosticCheckItemIndexModel
    {
        public List<DiagnosticCheckType> Types { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
