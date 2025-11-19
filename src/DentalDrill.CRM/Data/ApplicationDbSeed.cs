using System;
using System.Threading.Tasks;
using DentalDrill.CRM.Models;
using DevGuild.AspNetCore.Services.Data.Entity;
using DevGuild.AspNetCore.Services.Identity.Data;
using DevGuild.AspNetCore.Services.Identity.Models;

namespace DentalDrill.CRM.Data
{
    public class ApplicationDbSeed : DbSeed<ApplicationDbContext>
    {
        public ApplicationDbSeed(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override async Task SeedAsync()
        {
            await this.Context.SeedRolesAsync<ApplicationDbContext, Role, Guid>(
                ApplicationRoles.Administrator,
                ApplicationRoles.CompanyAdministrator,
                ApplicationRoles.OfficeAdministrator,
                ApplicationRoles.WorkshopTechnician,
                ApplicationRoles.CompanyManager,
                ApplicationRoles.Client,
                ApplicationRoles.Corporate);

            await this.Context.SeedUserAsync<ApplicationDbContext, ApplicationUser, Role, Guid>("admin", null, "mwM6l@Jrtp8uennLMAGJ", ApplicationRoles.Administrator);

            await this.Context.SeedEntityAsync(new CorporatePricingCategory { Id = Guid.Parse("{4552F050-6192-4BB2-B587-806BC149BAB2}"), OrderNo = 1, Name = "Regular" });
            await this.Context.SeedEntityAsync(new CorporatePricingCategory { Id = Guid.Parse("{EDDDD2D6-5A1E-47FF-90F0-7D6E251BF20A}"), OrderNo = 2, Name = "Corp 1" });
            await this.Context.SeedEntityAsync(new CorporatePricingCategory { Id = Guid.Parse("{20DA43F5-37D8-4C68-B23A-EC179263C4F1}"), OrderNo = 3, Name = "Corp 10" });
            await this.Context.SeedEntityAsync(new CorporatePricingCategory { Id = Guid.Parse("{F8C443BA-83FD-47B0-86D4-B760330CDAE2}"), OrderNo = 4, Name = "Corp 25" });

            await this.Context.SaveChangesAsync();

            await this.Context.SeedEntityAsync(new FeedbackConfiguration
            {
                Id = FeedbackConfiguration.Default,
                AutoSendFormEnabled = false,
                AutoSendFormDelayDays = 0,
                AutoSendSkipJobs = 0,
                PeriodLimitingEnabled = false,
                PeriodLimitingType = FeedbackConfigurationPeriodType.Day,
                PeriodLimitingLength = 0,
                PeriodLimitingQuantity = 0,
            });

            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{C5FE6195-6C9B-4A72-8800-C57C4EF8F48E}"),
                OrderNo = 1,
                Type = FeedbackFormQuestionType.Rating,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Staff",
                Name = "1. How was your interaction with our office staff?",
            });
            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{F93FA422-B76A-469B-AAB1-34EDAD5C9B8C}"),
                OrderNo = 2,
                Type = FeedbackFormQuestionType.Rating,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Estimate Time",
                Name = "2. How satisfied were you with the time it took to receive your estimate?",
            });
            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{9D7B8CD4-22B8-456D-969C-69C373EBAC72}"),
                OrderNo = 3,
                Type = FeedbackFormQuestionType.Rating,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Repair Time",
                Name = "3. How satisfied were you with the repair time?",
            });
            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{45B91787-A155-4B52-A434-B7EE413067FA}"),
                OrderNo = 4,
                Type = FeedbackFormQuestionType.Rating,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Service",
                Name = "4. How satisfied were you with our overall service?",
            });
            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{BF844DF2-766B-4118-972C-4E82D09888BF}"),
                OrderNo = 5,
                Type = FeedbackFormQuestionType.Rating,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Hub as a tool",
                Name = "5. Do you think the Handpiece Hub will be a useful tool for your practice?",
            });
            await this.Context.SeedEntityAsync(new FeedbackFormQuestion
            {
                Id = Guid.Parse("{4B8209D7-B668-4CFD-B142-B038FA95686B}"),
                OrderNo = 6,
                Type = FeedbackFormQuestionType.MultilineText,
                IsEnabled = true,
                IsDisplayed = true,
                ShortName = "Comment",
                Name = "Please leave any good or bad comments below:",
            });

            await this.Context.SaveChangesAsync();
        }
    }
}
