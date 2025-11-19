using System;
using System.ComponentModel.DataAnnotations;

namespace DentalDrill.CRM.Models
{
    public class ClientHandpieceComponent
    {
        public Guid Id { get; set; }

        public Guid ClientHandpieceId { get; set; }

        public ClientHandpiece ClientHandpiece { get; set; }

        public Int32 OrderNo { get; set; }

        [Required]
        [MaxLength(100)]
        public String Brand { get; set; }

        [Required]
        [MaxLength(200)]
        public String Model { get; set; }

        [Required]
        [MaxLength(200)]
        public String Serial { get; set; }
    }
}
