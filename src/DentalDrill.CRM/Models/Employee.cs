using System;
using System.ComponentModel.DataAnnotations;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;

namespace DentalDrill.CRM.Models
{
    public class Employee
    {
        public Guid Id { get; set; }

        [StringLength(100)]
        public String FirstName { get; set; }

        [StringLength(100)]
        public String LastName { get; set; }

        public Guid ApplicationUserId { get; set; }

        public ApplicationUser ApplicationUser { get; set; }

        public EmployeeType Type { get; set; }

        public Guid RoleId { get; set; }

        public EmployeeRole Role { get; set; }

        public String AppearanceTheme { get; set; }

        public Guid? AppearanceBackgroundId { get; set; }

        public UploadedFile AppearanceBackground { get; set; }

        public Decimal? AppearanceOpacity { get; set; }

        public DeletionStatus DeletionStatus { get; set; }

        public String GetFullName() => this.DeletionStatus switch
        {
            DeletionStatus.Normal => $"{this.FirstName} {this.LastName}",
            DeletionStatus.Deleted => $"{this.FirstName} {this.LastName} (DELETED)",
            _ => throw new ArgumentOutOfRangeException(),
        };
    }
}
