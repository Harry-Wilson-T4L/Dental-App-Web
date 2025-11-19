using System;

namespace DentalDrill.CRM.Models.ViewModels
{
    public class ErrorViewModel
    {
        public String RequestId { get; set; }

        public Boolean ShowRequestId => !string.IsNullOrEmpty(this.RequestId);
    }
}