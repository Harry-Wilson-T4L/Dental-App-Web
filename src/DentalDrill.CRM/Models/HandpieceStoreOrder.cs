using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class HandpieceStoreOrder
    {
        public Guid Id { get; set; }

        public Int32 OrderNumber { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ConfirmedOn { get; set; }

        public DateTime? ShippedOn { get; set; }

        public DateTime? DeliveredOn { get; set; }

        public Guid? CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public Guid? EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Guid? ClientId { get; set; }

        public Client Client { get; set; }

        public Guid? CorporateId { get; set; }

        public Corporate Corporate { get; set; }

        [Required]
        [MaxLength(200)]
        public String SurgeryName { get; set; }

        [Required]
        [MaxLength(200)]
        public String ContactName { get; set; }

        [Required]
        [MaxLength(256)]
        public String ContactEmail { get; set; }

        public HandpieceStoreOrderStatus Status { get; set; }

        public ICollection<HandpieceStoreOrderItem> Items { get; set; }
    }
}
