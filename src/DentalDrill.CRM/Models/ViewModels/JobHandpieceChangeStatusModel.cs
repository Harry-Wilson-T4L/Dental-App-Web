using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevGuild.AspNetCore.ObjectModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class JobHandpieceChangeStatusModel : IEditModelOriginalEntity<Handpiece>
    {
        [BindNever]
        public Handpiece Original { get; set; }

        public HandpieceStatus HandpieceStatus { get; set; }
    }
}
