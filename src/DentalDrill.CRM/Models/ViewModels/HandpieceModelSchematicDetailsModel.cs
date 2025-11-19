using System;
using DentalDrill.CRM.Domain.Abstractions;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceModelSchematicDetailsModel
    {
        public Guid Id => this.Entity.Id;

        public HandpieceModelSchematic Entity { get; set; }

        public IEmployeeAccess Access { get; set; }
    }
}
