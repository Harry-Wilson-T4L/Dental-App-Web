using System;
using System.Collections.Generic;
using System.IO;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Generation;
using DentalDrill.CRM.Services.RandomData;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Services.Bundling;
using DevGuild.AspNetCore.Services.Data.Entity;
using DevGuild.AspNetCore.Services.Identity.Models;
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.ModelMapping;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Sms;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Uploads.Files;
using DevGuild.AspNetCore.Services.Uploads.Images;
using DevGuild.AspNetCore.Testing;
using DevGuild.AspNetCore.Testing.Hosting;
using DevGuild.AspNetCore.Testing.Identity;
using DevGuild.AspNetCore.Testing.Mvc;
using DevGuild.AspNetCore.Testing.Storage;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace DentalDrill.CRM.Tests.Environment
{
    public class TestEnvironmentConfiguration : IMockConfiguration
    {
        public virtual void ConfigureServices(IServiceCollection services)
        {
            var configuration = this.BuildConfiguration();

            var solutionRoot = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(System.Environment.CurrentDirectory))));
            var mainProjectRoot = Path.Combine(solutionRoot, "DentalDrill.CRM");
            services.MockHostingEnvironment("DentalDrill.CRM", Environments.Development, mainProjectRoot);

            services.AddLogging(options => options.AddConsole());

            services.AddBundling(configuration.GetSection("Bundling"));

            services.AddStorage(configuration.GetSection("Storage"))
                .AddInMemoryProvider()
                .RegisterContainer("Images")
                .RegisterContainer("Files");

            services.AddDbContext<ApplicationDbContext>(
                options => options.UseInMemoryDatabase(configuration.GetConnectionString("DefaultConnection")),
                optionsLifetime: ServiceLifetime.Singleton);
            services.AddEntityDataServices<ApplicationDbContext>(options => new ApplicationDbContext(options));

            services.AddIdentity<ApplicationUser, Role>(options =>
                {
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 7;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .MockStateServicesGuidKey<ApplicationUser>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();

            services.AddPermissions(new Startup(null, null).ConfigurePermissions());

            services.AddMail(configuration.GetSection("Mail"))
                .RegisterConfiguration("System");

            services.AddSms(configuration.GetSection("Sms"))
                .RegisterConfiguration("System");

            services.AddImageUpload("/img/placeholder.png")
                .AddConfiguration("HandpieceImage", 1, image => image
                    .Container("Images", "HandpieceImage")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 200)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 450)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 768)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 1440))))
                .AddConfiguration("AppearancesImage", 1, image => image
                    .Container("Images", "AppearancesImage")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 200)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 450)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 768)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 1440)))
                    .Variation("3840", variation => variation.Scale(scale => scale.WidthAndHeight(3840, 2160))))
                .AddConfiguration("EmailImage", 1, image => image
                    .Container("Images", "EmailImage")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("Original", variation => variation));

            services.AddFileUpload()
                .AddConfiguration("Attachments", 1, file => file
                    .Container("Files", "attachments")
                    .AllowedFormats("txt", "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "zip", "jpg", "jpeg", "png"))
                .AddConfiguration("Invoices", 1, file => file
                    .Container("Files", "invoices")
                    .AllowedFormats("pdf"))
                .AddConfiguration("EmailAttachments", 1, file => file
                    .Container("Files", "emails")
                    .AllowedFormats("txt", "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "zip", "jpg", "jpeg", "png"));

            services.AddModelMapping();
            services.AddControllerServices();
            services.MockUrlHelper();

            services.AddSingleton<NotificationsHubConnectionsTracker>();

            services.AddScoped<IChangeTrackingService<Handpiece, HandpieceChange>, HandpieceChangeTrackingService>();
            services.AddScoped<IChangeTrackingService<Job, JobChange>, JobChangeTrackingService>();

            services.AddRandomServices();
            services.AddScoped<HandpieceImagesService>();
            services.AddScoped<EntitiesGenerationService>();

            services.AddScoped<ClientEmailsService>();
            services.AddScoped<UserEntityResolver>();
            services.AddScoped<NotificationsService>();
            services.AddScoped<RepairWorkflowService>();
            services.AddScoped<ImportService>();
        }

        public virtual IConfiguration BuildConfiguration()
        {
            var configurationData = new Dictionary<String, String>
            {
                { "ConnectionStrings:DefaultConnection", $"InMemory-{Guid.NewGuid():D}" },
                { "Bundling:Enabled", "false" },
                { "Storage:Images:Type", "InMemory" },
                { "Storage:Images:BaseUrl", "/images" },
                { "Storage:Files:Type", "InMemory" },
                { "Storage:Files:BaseUrl", "/files" },
                { "Mail:System:Type", "None" },
                { "Mail:System:Sender", "noreply@devguild.ru" },
                { "Mail:System:BlindCopy", "developer@devguild.ru" },
                { "Mail:System:DebugMode", "true" },
                { "Sms:System:Type", "None" },
                { "Sms:System:SenderName", "DevGuild" }
            };

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.AddInMemoryCollection(configurationData);
            return configurationBuilder.Build();
        }
    }
}
