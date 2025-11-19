using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Emails.Models;
using DentalDrill.CRM.Models;
using MimeKit;

namespace DentalDrill.CRM.Emails
{
    public abstract class BaseEmployeeEmail : BaseEmail
    {
        public Employee Employee { get; set; }

        protected override void OverrideContactDetails(EmailContactDetails details)
        {
            if (this.Employee != null)
            {
                details.SenderName = $"{this.Employee.FirstName} {this.Employee.LastName}";
                if (this.IsValidEmail(this.Employee.ApplicationUser?.Email))
                {
                    details.Email = this.Employee.ApplicationUser?.Email;
                }
            }
        }

        protected override void ExtraMailMessageProcessing(MimeMessage message)
        {
            if (this.IsValidEmail(this.Employee?.ApplicationUser?.Email))
            {
                message.ReplyTo.Add(new MailboxAddress((String)null, this.Employee?.ApplicationUser?.Email));
            }
        }

        private Boolean IsValidEmail(String email)
        {
            if (String.IsNullOrEmpty(email))
            {
                return false;
            }

            var regex = new Regex(@"^(?<username>[-A-Za-z0-9._]+)@(?<hostname>[-A-Za-z0-9._]+)$");
            return regex.IsMatch(email);
        }
    }
}
