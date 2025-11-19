using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using DentalDrill.CRM.Extensions;
using DentalDrill.CRM.Services.Csv;

namespace DentalDrill.CRM.Models.Import
{
    public class ClientImportModel
    {
        [CsvColumnName("Account details")]
        public String AccountDetails { get; set; }

        [CsvColumnName("Address 1")]
        public String Address1 { get; set; }

        [CsvColumnName("Address 2")]
        public String Address2 { get; set; }

        [CsvColumnName("Address 3")]
        public String Address3 { get; set; }

        [CsvColumnName("AEM Bounce Back")]
        public String AemBounceBack { get; set; }

        [CsvColumnName("AEM Opt Out")]
        public String AemOptOut { get; set; }

        [CsvColumnName("Alternate Extension")]
        public String AlternateExtension { get; set; }

        [CsvColumnName("Alternate Phone")]
        public String AlternatePhone { get; set; }

        [CsvColumnName("Birth Date")]
        public String BirthDate { get; set; }

        [CsvColumnName("Brand Used")]
        public String BrandUsed { get; set; }

        [CsvColumnName("Brands Used")]
        public String BrandsUsed { get; set; }

        [CsvColumnName("City")]
        public String City { get; set; }

        [CsvColumnName("Company")]
        public String Company { get; set; }

        [CsvColumnName("Contact")]
        public String Contact { get; set; }

        [CsvColumnName("Country")]
        public String Country { get; set; }

        [CsvColumnName("Create Date")]
        public String CreateDate { get; set; }

        [CsvColumnName("Customer Number")]
        public String CustomerNumber { get; set; }

        [CsvColumnName("DDS Customer Number")]
        public String DdsCustomerNumber { get; set; }

        [CsvColumnName("Department")]
        public String Department { get; set; }

        [CsvColumnName("Edit Date")]
        public String EditDate { get; set; }

        [CsvColumnName("E-mail")]
        public String EMail { get; set; }

        [CsvColumnName("Email 2")]
        public String Email2 { get; set; }

        [CsvColumnName("E-mail 2")]
        public String EMail2 { get; set; }

        [CsvColumnName("Email-3")]
        public String Email3 { get; set; }

        [CsvColumnName("Extension")]
        public String Extension { get; set; }

        [CsvColumnName("Favorite")]
        public String Favorite { get; set; }

        [CsvColumnName("Fax Extension")]
        public String FaxExtension { get; set; }

        [CsvColumnName("Fax Phone")]
        public String FaxPhone { get; set; }

        [CsvColumnName("First Name")]
        public String FirstName { get; set; }

        [CsvColumnName("Home Address 1")]
        public String HomeAddress1 { get; set; }

        [CsvColumnName("Home Address 2")]
        public String HomeAddress2 { get; set; }

        [CsvColumnName("Home Address 3")]
        public String HomeAddress3 { get; set; }

        [CsvColumnName("Home City")]
        public String HomeCity { get; set; }

        [CsvColumnName("Home Country")]
        public String HomeCountry { get; set; }

        [CsvColumnName("Home Extension")]
        public String HomeExtension { get; set; }

        [CsvColumnName("Home Phone")]
        public String HomePhone { get; set; }

        [CsvColumnName("Home Postcode")]
        public String HomePostcode { get; set; }

        [CsvColumnName("Home State")]
        public String HomeState { get; set; }

        [CsvColumnName("ID/Status")]
        public String IdStatus { get; set; }

        [CsvColumnName("Last Attempt")]
        public String LastAttempt { get; set; }

        [CsvColumnName("Last Edited By")]
        public String LastEditedBy { get; set; }

        [CsvColumnName("Last E-mail")]
        public String LastEMail { get; set; }

        [CsvColumnName("Last Meeting")]
        public String LastMeeting { get; set; }

        [CsvColumnName("Last Reach")]
        public String LastReach { get; set; }

        [CsvColumnName("Last Results")]
        public String LastResults { get; set; }

        [CsvColumnName("Letter Date")]
        public String LetterDate { get; set; }

        [CsvColumnName("Mailout No")]
        public String MailoutNo { get; set; }

        [CsvColumnName("Messenger ID")]
        public String MessengerId { get; set; }

        [CsvColumnName("Middle Name")]
        public String MiddleName { get; set; }

        [CsvColumnName("Mobile Extension")]
        public String MobileExtension { get; set; }

        [CsvColumnName("Mobile Phone")]
        public String MobilePhone { get; set; }

        [CsvColumnName("Name Prefix")]
        public String NamePrefix { get; set; }

        [CsvColumnName("Name Suffix")]
        public String NameSuffix { get; set; }

        [CsvColumnName("Opening Hours")]
        public String OpeningHours { get; set; }

        [CsvColumnName("Other Contact")]
        public String OtherContact { get; set; }

        [CsvColumnName("Pager Extension")]
        public String PagerExtension { get; set; }

        [CsvColumnName("Pager Phone")]
        public String PagerPhone { get; set; }

        [CsvColumnName("Personal E-mail")]
        public String PersonalEMail { get; set; }

        [CsvColumnName("Phone")]
        public String Phone { get; set; }

        [CsvColumnName("Postcode")]
        public String Postcode { get; set; }

        [CsvColumnName("Practice Manager")]
        public String PracticeManager { get; set; }

        [CsvColumnName("Private Contact")]
        public String PrivateContact { get; set; }

        [CsvColumnName("Record Creator")]
        public String RecordCreator { get; set; }

        [CsvColumnName("Record Manager")]
        public String RecordManager { get; set; }

        [CsvColumnName("Referred By")]
        public String ReferredBy { get; set; }

        [CsvColumnName("Salutation")]
        public String Salutation { get; set; }

        [CsvColumnName("Secondary Contacts")]
        public String SecondaryContacts { get; set; }

        [CsvColumnName("Snapshot Contact Grade")]
        public String SnapshotContactGrade { get; set; }

        [CsvColumnName("Snapshot Contact Rank")]
        public String SnapshotContactRank { get; set; }

        [CsvColumnName("Snapshot Favorites")]
        public String SnapshotFavorites { get; set; }

        [CsvColumnName("Snapshot Image")]
        public String SnapshotImage { get; set; }

        [CsvColumnName("Snapshot Lookup")]
        public String SnapshotLookup { get; set; }

        [CsvColumnName("Snapshot WhoIs")]
        public String SnapshotWhois { get; set; }

        [CsvColumnName("Spouse")]
        public String Spouse { get; set; }

        [CsvColumnName("State")]
        public String State { get; set; }

        [CsvColumnName("Surname")]
        public String Surname { get; set; }

        [CsvColumnName("Swiftpage Import Status")]
        public String SwiftpageImportStatus { get; set; }

        [CsvColumnName("Title")]
        public String Title { get; set; }

        [CsvColumnName("User 1")]
        public String User1 { get; set; }

        [CsvColumnName("User 10")]
        public String User10 { get; set; }

        [CsvColumnName("User 2")]
        public String User2 { get; set; }

        [CsvColumnName("User 3")]
        public String User3 { get; set; }

        [CsvColumnName("User 4")]
        public String User4 { get; set; }

        [CsvColumnName("User 5")]
        public String User5 { get; set; }

        [CsvColumnName("User 6")]
        public String User6 { get; set; }

        [CsvColumnName("User 7")]
        public String User7 { get; set; }

        [CsvColumnName("User 8")]
        public String User8 { get; set; }

        [CsvColumnName("User 9")]
        public String User9 { get; set; }

        [CsvColumnName("Web Site")]
        public String WebSite { get; set; }

        [CsvColumnIgnore]
        public String UniqueId => StringExtensions.CollapseSpaces($"{this.Contact} - {this.Company} - {this.City}".Trim());

        [CsvColumnIgnore]
        public Int32? ClientNumber { get; set; }

        public List<String> ParseEmails()
        {
            IEnumerable<String> ParseEmailsFromString(String emailsString)
            {
                var regex = new Regex("(?<email>[-A-Za-z0-9._+]+@[-A-Za-z0-9._]+)");
                var matches = regex.Matches(emailsString);
                foreach (var match in matches.Where(x => x.Success))
                {
                    yield return match.Groups["email"].Value;
                }
            }

            var emails = new List<String>();
            emails.AddRange(ParseEmailsFromString(this.EMail));
            emails.AddRange(ParseEmailsFromString(this.EMail2));
            emails.AddRange(ParseEmailsFromString(this.Email2));
            emails.AddRange(ParseEmailsFromString(this.Email3));
            emails.AddRange(ParseEmailsFromString(this.PersonalEMail));
            return emails;
        }
    }
}
