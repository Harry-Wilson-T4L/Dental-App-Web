using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentalDrill.CRM.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        [Display(Name = "Client #")]
        public Int32 ClientNo { get; set; }

        [Required]
        [MaxLength(200)]
        [Display(Name = "Surgery Name")]
        public String Name { get; set; }

        [Required]
        [MaxLength(500)]
        public String FullName { get; set; }

        [Required]
        [MaxLength(200)]
        public String UrlPath { get; set; }

        [MaxLength(100)]
        [Display(Name = "Other contact")]
        public String OtherContact { get; set; }

        [MaxLength(256)]
        [Display(Name = "Main email")]
        public String Email { get; set; }

        [Display(Name = "Other emails")]
        public String SecondaryEmails { get; set; }

        [MaxLength(50)]
        [Display(Name = "Main phone")]
        public String Phone { get; set; }

        [Display(Name = "Other phones")]
        public String SecondaryPhones { get; set; }

        [MaxLength(200)]
        [Display(Name = "Address")]
        public String Address { get; set; }

        [MaxLength(200)]
        [Display(Name = "Suburb")]
        public String Suburb { get; set; }

        [MaxLength(100)]
        [Display(Name = "Princ. dentist")]
        public String PrincipalDentist { get; set; }

        [MaxLength(100)]
        [Display(Name = "Opening hours")]
        public String OpeningHours { get; set; }

        [Display(Name = "Brands")]
        public String Brands { get; set; }

        [Display(Name = "Comment")]
        public String Comment { get; set; }

        [MaxLength(4)]
        [Display(Name = "Postal")]
        public String PostCode { get; set; }

        [Display(Name = "State")]
        public Guid StateId { get; set; }

        [Display(Name = "State")]
        public State State { get; set; }

        [Display(Name = "Zone")]
        public Guid ZoneId { get; set; }

        [Display(Name = "Zone")]
        public Zone Zone { get; set; }

        [Display(Name = "User")]
        public Guid? UserId { get; set; }

        [Display(Name = "User")]
        public ApplicationUser User { get; set; }

        [Display(Name = "Corporate")]
        public Guid? CorporateId { get; set; }

        [Display(Name = "Corporate")]
        public Corporate Corporate { get; set; }

        [Display(Name = "Pricing category")]
        public Guid? PricingCategoryId { get; set; }

        [Display(Name = "Pricing category")]
        public CorporatePricingCategory PricingCategory { get; set; }

        [Display(Name = "Primary workshop")]
        public Guid PrimaryWorkshopId { get; set; }

        [Display(Name = "Primary workshop")]
        public Workshop PrimaryWorkshop { get; set; }

        [MaxLength(200)]
        public String ImportKey { get; set; }

        public Boolean HideJobs { get; set; }

        public ClientNotificationsOptions NotificationsOptions { get; set; }

        public ICollection<ClientNote> Notes { get; set; }

        public ICollection<Job> Jobs { get; set; }

        public ICollection<ClientAttachment> Attachments { get; set; }

        public ICollection<FeedbackForm> Feedback { get; set; }

        public List<String> ParseSecondaryEmails()
        {
            if (String.IsNullOrWhiteSpace(this.SecondaryEmails))
            {
                return new List<String>();
            }

            return this.SecondaryEmails.Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(x => x.Trim())
                .Where(x => !String.IsNullOrEmpty(x))
                .ToList();
        }

        public String ComputeFullName()
        {
            var sb = new StringBuilder();
            sb.Append(this.Name?.Trim() ?? String.Empty);
            if (sb.Length > 0 && (this.PrincipalDentist?.Trim()?.Length ?? 0) > 0)
            {
                sb.Append(" - ");
            }

            sb.Append(this.PrincipalDentist?.Trim() ?? String.Empty);
            if (sb.Length > 0 && (this.Suburb?.Trim().Length ?? 0) > 0)
            {
                sb.Append(" - ");
            }

            sb.Append(this.Suburb?.Trim() ?? String.Empty);
            return sb.ToString().Trim();
        }
    }
}
