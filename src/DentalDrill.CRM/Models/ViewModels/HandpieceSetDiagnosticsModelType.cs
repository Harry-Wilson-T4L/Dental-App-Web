using System;
using System.Collections.Generic;
using DevGuild.AspNetCore.ObjectModel;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceSetDiagnosticsModelType
    {
        public Guid Id { get; set; }

        public String Name { get; set; }

        public List<SelectableEntity<NamedElement<Guid>>> Items { get; set; } = new List<SelectableEntity<NamedElement<Guid>>>();
    }
}