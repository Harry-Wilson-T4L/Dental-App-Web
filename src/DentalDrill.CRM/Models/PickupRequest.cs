using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class PickupRequest
    {
        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public Guid? CreatedById { get; set; }

        public ApplicationUser CreatedBy { get; set; }

        public Guid? EmployeeId { get; set; }

        public Employee Employee { get; set; }

        public Guid? ClientId { get; set; }

        public Client Client { get; set; }

        public Guid? CorporateId { get; set; }

        public Corporate Corporate { get; set; }

        public PickupRequestType Type { get; set; }

        public PickupRequestStatus Status { get; set; }

        [Required]
        [MaxLength(100)]
        public String PracticeName { get; set; }

        [Required]
        [MaxLength(100)]
        public String ContactPerson { get; set; }

        [Required]
        [MaxLength(256)]
        public String Email { get; set; }

        [Required]
        [MaxLength(20)]
        public String Phone { get; set; }

        [Required]
        [MaxLength(30)]
        public String AddressLine1 { get; set; }

        [MaxLength(30)]
        public String AddressLine2 { get; set; }

        [Required]
        [MaxLength(50)]
        public String Suburb { get; set; }

        [Required]
        [MaxLength(25)]
        public String State { get; set; }

        [Required]
        [MaxLength(10)]
        public String Postcode { get; set; }

        [Required]
        [MaxLength(30)]
        public String Country { get; set; }

        public Int32 HandpiecesCount { get; set; }

        [MaxLength(90)]
        public String Comment { get; set; }

        public ICollection<PickupRequestConsignment> Consignments { get; set; }
    }
}
