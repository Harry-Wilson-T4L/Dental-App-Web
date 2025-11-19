using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Domain.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class HandpieceStoreOrderDetailsModel
    {
        [BindNever]
        public HandpieceStoreOrder Order { get; set; }

        [BindNever]
        public IEmployeeAccess Access { get; set; }

        [BindNever]
        public List<HandpieceStoreOrderStatus> StatusTransitions { get; set; }
    }
}
