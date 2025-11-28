using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Data;
using DentalDrill.CRM.Domain;
using DentalDrill.CRM.Domain.Abstractions;
using DentalDrill.CRM.Extensions.ModelBinding;
using DentalDrill.CRM.Filters;
using DentalDrill.CRM.Hubs;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.Import;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Services;
using DentalDrill.CRM.Services.Data;
using DentalDrill.CRM.Services.Emails;
using DentalDrill.CRM.Services.Generation;
using DentalDrill.CRM.Services.GenericFlags;
using DentalDrill.CRM.Services.RandomData;
using DentalDrill.CRM.Services.Theming;
using DevGuild.AspNetCore.Controllers.Mvc.Crud;
using DevGuild.AspNetCore.Extensions.Security;
using DevGuild.AspNetCore.Services.Bundling;
using DevGuild.AspNetCore.Services.Data.Entity;
using DevGuild.AspNetCore.Services.Data.Relational.SqlServer;
using DevGuild.AspNetCore.Services.EntitySequences;
using DevGuild.AspNetCore.Services.Identity;
using DevGuild.AspNetCore.Services.Identity.Models;
using DevGuild.AspNetCore.Services.Logging;
using DevGuild.AspNetCore.Services.Mail;
using DevGuild.AspNetCore.Services.Mail.AmazonSes;
using DevGuild.AspNetCore.Services.ModelMapping;
using DevGuild.AspNetCore.Services.Permissions;
using DevGuild.AspNetCore.Services.Sms;
using DevGuild.AspNetCore.Services.Storage;
using DevGuild.AspNetCore.Services.Storage.AmazonS3;
using DevGuild.AspNetCore.Services.Storage.FileSystem;
using DevGuild.AspNetCore.Services.Uploads.Files;
using DevGuild.AspNetCore.Services.Uploads.Images;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json.Serialization;

namespace DentalDrill.CRM
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            this.Configuration = configuration;
            this.Environment = environment;
        }

        public IConfiguration Configuration { get; }

        public IWebHostEnvironment Environment { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureDataProtection(this.Configuration, providers => providers
                .AddEphemeralPersistence()
                .AddFileSystemPersistence()
                .AddRegistryPersistence()
                .AddDpapiEncryption()
                .AddCertificateEncryption());

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddBundling(this.Configuration.GetSection("Bundling"));

            services.AddStorage(this.Configuration.GetSection("Storage"))
                .AddFileSystemProvider()
                .AddAmazonS3Provider()
                .RegisterContainer("Images")
                .RegisterContainer("Files")
                .RegisterContainer("Emails");

            // FIX #1: Add correct SQL connection factory 
            services.AddSqlConnectionFactory(options =>
                options.ConnectionString = this.Configuration.GetConnectionString("DefaultConnection"));

            // FIX #2: DbContext must be Scoped + SQL Retry enabled
            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(
                    this.Configuration.GetConnectionString("DefaultConnection"),
                    sqlOptions => sqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 5,
                        maxRetryDelay: TimeSpan.FromSeconds(10),
                        errorNumbersToAdd: null
                    )
                );
            },
            optionsLifetime: ServiceLifetime.Scoped);
            services.AddEntityDataServices<ApplicationDbContext>(options => new ApplicationDbContext(options));
            services.AddScoped<IDataTransactionService, DataTransactionService>();

            services.AddScoped<IEntitySequenceService, EntitySequenceService>();

            services.AddIdentity<ApplicationUser, Role>(options =>
                {
                    options.User.RequireUniqueEmail = false;
                    options.Password.RequireLowercase = true;
                    options.Password.RequireUppercase = true;
                    options.Password.RequireDigit = true;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequiredLength = 7;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddStateServicesGuidKey<ApplicationUser>();

            services.AddScoped<IUserClaimsPrincipalFactory<ApplicationUser>, ApplicationClaimsPrincipalFactory>();

            services.AddPermissions(this.ConfigurePermissions());

            services.AddMail(this.Configuration.GetSection("Mail"))
                .AddCustomRepository<SeparateContentEmailServiceRepository>()
                .AddAmazonSes()
                .RegisterConfiguration("System");

            services.AddSms(this.Configuration.GetSection("Sms"))
                .RegisterConfiguration("System");

            services.AddImageUpload("/img/placeholder.png")
                .AddConfiguration("HandpieceBrand", 1, image => image
                    .Container("Images", "HandpieceBrands")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("40", variation => variation.Scale(scale => scale.WidthAndHeight(40, 40)))
                    .Variation("80", variation => variation.Scale(scale => scale.WidthAndHeight(80, 80)))
                    .Variation("160", variation => variation.Scale(scale => scale.WidthAndHeight(160, 160)))
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 300)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 600)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 1024)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 2560))))
                .AddConfiguration("HandpieceModel", 1, image => image
                    .Container("Images", "HandpieceModels")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("40", variation => variation.Scale(scale => scale.WidthAndHeight(40, 40)))
                    .Variation("80", variation => variation.Scale(scale => scale.WidthAndHeight(80, 80)))
                    .Variation("160", variation => variation.Scale(scale => scale.WidthAndHeight(160, 160)))
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 300)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 600)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 1024)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 2560))))
                .AddConfiguration("HandpieceStoreListing", 1, image => image
                    .Container("Images", "HandpieceStoreListings")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("40", variation => variation.Scale(scale => scale.WidthAndHeight(40, 40)))
                    .Variation("80", variation => variation.Scale(scale => scale.WidthAndHeight(80, 80)))
                    .Variation("160", variation => variation.Scale(scale => scale.WidthAndHeight(160, 160)))
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 300)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 600)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 1024)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 2560))))
                .AddConfiguration("HandpieceModelSchematic", 1, image => image
                    .Container("Images", "HandpieceModelSchematics")
                    .AllowedFormats("jpeg", "jpg", "png")
                    .Variation("40", variation => variation.Scale(scale => scale.WidthAndHeight(40, 40)))
                    .Variation("80", variation => variation.Scale(scale => scale.WidthAndHeight(80, 80)))
                    .Variation("160", variation => variation.Scale(scale => scale.WidthAndHeight(160, 160)))
                    .Variation("300", variation => variation.Scale(scale => scale.WidthAndHeight(300, 300)))
                    .Variation("600", variation => variation.Scale(scale => scale.WidthAndHeight(600, 600)))
                    .Variation("1024", variation => variation.Scale(scale => scale.WidthAndHeight(1024, 1024)))
                    .Variation("2560", variation => variation.Scale(scale => scale.WidthAndHeight(2560, 2560))))
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
                .AddConfiguration("HandpieceModelSchematic", 1, file => file
                    .Container("Files", "HandpieceModelSchematics")
                    .AllowedFormats("pdf"))
                .AddConfiguration("Attachments", 1, file => file
                    .Container("Files", "attachments")
                    .AllowedFormats("txt", "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "zip", "jpg", "jpeg", "png"))
                .AddConfiguration("Invoices", 1, file => file
                    .Container("Files", "invoices")
                    .AllowedFormats("pdf"))
                .AddConfiguration("EmailAttachments", 1, file => file
                    .Container("Files", "emails")
                    .AllowedFormats("txt", "pdf", "doc", "docx", "xls", "xlsx", "ppt", "pptx", "zip", "jpg", "jpeg", "png"))
                .AddConfiguration("EmployeeBackground", 1, file => file
                    .Container("Files", "EmployeesBackgrounds")
                    .MaximumSize(10 * 1024 * 1024)
                    .AllowedFormats("png", "jpg", "jpeg", "mp4", "webm"))
                .AddConfiguration("HandpieceVideo", 1, file => file
                    .Container("Files", "HandpieceVideos")
                    .AllowedFormats("mp4", "mov", "webm")
                    .MaximumSize(100 * 1024 * 1024));

            services.AddModelMapping();
            services.AddControllerServices();

            services.AddKendo();

            {
                var mvcBuilder = services.AddControllersWithViews(options =>
                {
                    options.Filters.Add<ForcedPasswordChangeFilter>();
                    options.AddPolymorphicModelBinder<FeedbackFormFillModelAnswer>(binder => binder
                        .AddDerived<FeedbackFormFillModelRatingAnswer>("Rating")
                        .AddDerived<FeedbackFormFillModelMultilineTextAnswer>("MultilineText"));
                });

                if (this.Environment.IsDevelopment())
                {
                    mvcBuilder = mvcBuilder.AddRazorRuntimeCompilation();
                }

                mvcBuilder.AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());
            }

            services.AddSignalR();

            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddScoped<IUrlHelper>(factory =>
            {
                var urlHelperFactory = factory.GetRequiredService<IUrlHelperFactory>();
                var actionContext = factory.GetRequiredService<IActionContextAccessor>().ActionContext;
                return urlHelperFactory.GetUrlHelper(actionContext);
            });

            services.AddSingleton<IRequestInformationProvider, RequestInformationProvider>();

            services.ConfigureHsts(this.Configuration);
            services.ConfigureHttpsRedirection(this.Configuration);

            services.AddSingleton<ISecureCodeGenerationService, SecureCodeGenerationService>();
            services.AddSingleton<INameNormalizationService, NameNormalizationService>();

            services.AddSingleton<NotificationsHubConnectionsTracker>();

            services.AddScoped<IChangeTrackingService<Handpiece, HandpieceChange>, HandpieceChangeTrackingService>();
            services.AddScoped<IChangeTrackingService<Job, JobChange>, JobChangeTrackingService>();

            services.Configure<ThemesOptions>(this.Configuration.GetSection("Themes"));
            services.AddScoped<IThemesService, ThemesService>();

            services.AddRandomServices();
            services.AddScoped<HandpieceImagesService>();
            services.AddScoped<EntitiesGenerationService>();

            services.AddSingleton<IDateTimeService, DateTimeService>();
            services.Configure<CalendarOptions>(this.Configuration.GetSection("Calendar"));
            services.AddScoped<CalendarService>();

            services.AddScoped<IWorkshopFactory, WorkshopFactory>();
            services.AddScoped<IWorkshopManager, WorkshopManager>();
            services.AddScoped<IClientFactory, ClientFactory>();
            services.AddScoped<IClientInternalsFactory, ClientInternalsFactory>();
            services.AddScoped<IClientManager, ClientManager>();
            services.AddScoped<IClientRepairedItemFactory, ClientRepairedItemFactory>();
            services.AddScoped<IClientRepairedHistoryReminderFactory, ClientRepairedHistoryReminderFactory>();

            services.AddScoped<IClientHandpieceFactory, ClientHandpieceFactory>();
            services.AddScoped<IClientHandpieceLoader, ClientHandpieceLoader>();
            services.AddScoped<IClientHandpieceManager, ClientHandpieceManager>();

            services.AddScoped<IJobFactory, JobFactory>();
            services.AddScoped<IJobLoader, JobLoader>();
            services.AddScoped<IJobManager, JobManager>();
            services.AddScoped<IJobTypeFactory, JobTypeFactory>();
            services.AddScoped<IJobTypeManager, JobTypeManager>();

            services.AddScoped<IHandpieceFactory, HandpieceFactory>();
            services.AddScoped<IHandpieceLoader, HandpieceLoader>();
            services.AddScoped<IHandpieceManager, HandpieceManager>();
            services.AddScoped<IHandpieceRequiredPartFactory, HandpieceRequiredPartFactory>();
            services.AddScoped<IHandpieceRequiredPartManager, HandpieceRequiredPartManager>();
            services.AddScoped<IInventorySKUTypeFactory, InventorySKUTypeFactory>();
            services.AddScoped<IInventorySKUTypeManager, InventorySKUTypeManager>();
            services.AddScoped<IInventorySKUFactory, InventorySKUFactory>();
            services.AddScoped<IInventorySKUManager, InventorySKUManager>();
            services.AddScoped<IInventoryMovementFactory, InventoryMovementFactory>();
            services.AddScoped<IInventoryMovementManager, InventoryMovementManager>();

            services.AddScoped<GenericFlagsService>();
            services.AddScoped<ClientEmailsService>();
            services.AddScoped<UserEntityResolver>();
            services.AddScoped<NotificationsService>();
            services.AddScoped<CallbackService>();
            services.AddScoped<RepairWorkflowService>();
            services.AddScoped<ImportService>();

            services.AddSingleton<ISuspendService, SuspendService>();
            services.Configure<PickupRequestEmailOptions>(this.Configuration.GetSection("PickupRequest:Email"));
            services.Configure<PickupRequestSmartFreightOptions>(this.Configuration.GetSection("PickupRequest:SmartFreight"));
            services.Configure<HandpieceStoreOrderEmailOptions>(this.Configuration.GetSection("HandpieceStoreOrders:Email"));

            services.AddSingleton<IHostedService, QuartzScheduler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            app.UseRequestLocalization(options =>
            {
                options.AddSupportedCultures("en-AU");
                options.AddSupportedUICultures("en-AU");
                options.SetDefaultCulture("en-AU");
            });

            if (this.Environment.IsDevelopment() || this.Configuration.GetValue<Boolean>("Debug:Enabled"))
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.TryUseHsts(this.Configuration);
                app.TryUseHttpsRedirection(this.Configuration);
            }

            app.UseHttpErrorLogging("DentalDrill.CRM.HttpError");

            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseAuthentication();

            app.UsePermissions(new PermissionsMiddlewaresOptions
            {
                InsufficientPermissionsHandling =
                {
                    ReturnNotFoundForAuthenticatedUsers = true,
                },
            });

            app.UseRouting();
            app.UseAuthorization();
            app.UseEndpoints(this.ConfigureEndpoints);
        }
    }
}
