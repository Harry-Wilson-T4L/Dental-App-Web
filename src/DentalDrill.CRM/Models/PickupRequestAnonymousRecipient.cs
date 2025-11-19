using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class PickupRequestAnonymousRecipient
    {
        public PickupRequestAnonymousRecipient()
        {
            this.Id = Guid.NewGuid();
        }

        public PickupRequestAnonymousRecipient(PickupRequest request)
        {
            this.Id = Guid.NewGuid();
            this.AccountNumber = 0;
            this.PracticeName = PickupRequestAnonymousRecipient.NormalizeString(request.PracticeName);
            this.AddressLine1 = PickupRequestAnonymousRecipient.NormalizeString(request.AddressLine1);
            this.AddressLine2 = PickupRequestAnonymousRecipient.NormalizeString(request.AddressLine2);
            this.Suburb = PickupRequestAnonymousRecipient.NormalizeString(request.Suburb);
            this.State = PickupRequestAnonymousRecipient.NormalizeString(request.State);
            this.Postcode = PickupRequestAnonymousRecipient.NormalizeString(request.Postcode);
            this.Country = PickupRequestAnonymousRecipient.NormalizeString(request.Country);
            {
                var keyBuilder = new StringBuilder();
                keyBuilder.Append(this.PracticeName);
                keyBuilder.Append("/");
                keyBuilder.Append(this.AddressLine1);
                keyBuilder.Append("/");
                keyBuilder.Append(this.AddressLine2);
                keyBuilder.Append("/");
                keyBuilder.Append(this.Suburb);
                keyBuilder.Append("/");
                keyBuilder.Append(this.State);
                keyBuilder.Append("/");
                keyBuilder.Append(this.Postcode);
                keyBuilder.Append("/");
                keyBuilder.Append(this.Country);
                this.UniqueKey = keyBuilder.ToString();
            }
        }

        public Guid Id { get; set; }

        public Int32 AccountNumber { get; set; }

        [Required]
        [MaxLength(300)]
        public String UniqueKey { get; set; }

        [Required]
        [MaxLength(100)]
        public String PracticeName { get; set; }

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

        private static String NormalizeString(String value)
        {
            if (value == null)
            {
                return null;
            }

            return Regex.Replace(value, @"\s{2,}", " ").Trim().ToUpperInvariant();
        }
    }
}
