using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceSetDiagnosticsModel
    {
        [BindNever]
        public Handpiece Handpiece { get; set; }

        [BindNever]
        public HandpieceDetailsModel Details { get; set; }

        [BindNever]
        public List<DiagnosticCheckType> CheckTypes { get; set; }

        [BindNever]
        public String UpdatedReport { get; set; }

        public List<SelectableEntity<NamedElement<Guid>>> Diagnostics { get; set; } = new List<SelectableEntity<NamedElement<Guid>>>();

        public Boolean OtherSelected { get; set; }

        public String OtherText { get; set; }

        public List<HandpieceSetDiagnosticsModelType> DiagnosticTypes { get; set; }
    }
}
