using System;
using System.Linq;
using System.Linq.Expressions;
using DentalDrill.CRM.Models;
using DentalDrill.CRM.Models.QueryModels;
using DentalDrill.CRM.Models.ViewModels;
using DentalDrill.CRM.Models.ViewModels.Inventory;
using DentalDrill.CRM.Models.ViewModels.InventoryMovements;
using DentalDrill.CRM.Models.ViewModels.Reports.Corporate;
using DentalDrill.CRM.Models.ViewModels.Reports.Global;
using DentalDrill.CRM.Models.ViewModels.Reports.Surgery;
using DevGuild.AspNetCore.Services.EntitySequences;
using DevGuild.AspNetCore.Services.Identity.Models;
using DevGuild.AspNetCore.Services.Logging.Data;
using DevGuild.AspNetCore.Services.Mail.Models;
using DevGuild.AspNetCore.Services.Uploads.Files.Models;
using DevGuild.AspNetCore.Services.Uploads.Images.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HandpieceModel = DentalDrill.CRM.Models.HandpieceModel;

namespace DentalDrill.CRM.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, Role, Guid>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<EmailMessage> EmailMessages { get; set; }

        public DbSet<Zone> Zones { get; set; }

        public DbSet<State> States { get; set; }

        public DbSet<ServiceLevel> ServiceLevels { get; set; }

        public DbSet<DiagnosticCheckType> DiagnosticCheckTypes { get; set; }

        public DbSet<DiagnosticCheckItem> DiagnosticCheckItems { get; set; }

        public DbSet<DiagnosticCheckItemType> DiagnosticCheckItemTypes { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ClientNote> ClientNotes { get; set; }

        public DbSet<ClientCallbackNotification> ClientCallbackNotifications { get; set; }

        public DbSet<ClientRepairedItemOverride> ClientRepairedItemOverrides { get; set; }

        public DbSet<Corporate> Corporates { get; set; }

        public DbSet<Workshop> Workshops { get; set; }

        public DbSet<JobType> JobTypes { get; set; }

        public DbSet<Job> Jobs { get; set; }

        public DbSet<JobInvoice> JobInvoices { get; set; }

        public DbSet<JobChange> JobChanges { get; set; }

        public DbSet<Handpiece> Handpieces { get; set; }

        public DbSet<HandpieceComponent> HandpieceComponents { get; set; }

        public DbSet<HandpieceDiagnostic> HandpieceDiagnostics { get; set; }

        public DbSet<HandpieceChange> HandpieceChanges { get; set; }

        public DbSet<ClientHandpiece> ClientHandpieces { get; set; }

        public DbSet<ClientHandpieceComponent> ClientHandpieceComponents { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<UploadedImage> UploadedImages { get; set; }

        public DbSet<UploadedFile> UploadedFiles { get; set; }

        public DbSet<EntitySequence> EntitySequences { get; set; }

        public DbSet<HandpieceImage> HandpieceImages { get; set; }

        public DbSet<ClientAppearance> ClientAppearances { get; set; }

        public DbSet<ClientAttachment> ClientAttachments { get; set; }

        public DbSet<ProblemDescriptionOption> ProblemDescriptionOptions { get; set; }

        public DbSet<ReturnEstimate> ReturnEstimates { get; set; }

        public DbSet<EmailTemplate> EmailTemplates { get; set; }

        public DbSet<Notification> Notifications { get; set; }

        public DbSet<CorporateAppearance> CorporateAppearances { get; set; }

        public DbSet<PickupRequest> PickupRequests { get; set; }

        public DbSet<PickupRequestConsignment> PickupRequestConsignments { get; set; }

        public DbSet<PickupRequestAnonymousRecipient> PickupRequestAnonymousRecipients { get; set; }

        public DbSet<CalendarYear> CalendarYears { get; set; }

        public DbSet<CalendarWeek> CalendarWeeks { get; set; }

        public DbSet<StockControlEntry> StockControlEntries { get; set; }

        public DbSet<HandpieceBrand> HandpieceBrands { get; set; }

        public DbSet<HandpieceModel> HandpieceModels { get; set; }

        public DbSet<HandpieceModelSchematic> HandpieceModelSchematics { get; set; }

        public DbSet<CorporatePricingCategory> CorporatePricingCategories { get; set; }

        public DbSet<CorporatePricingServiceLevel> CorporatePricingServiceLevels { get; set; }

        public DbSet<HandpieceStoreListing> HandpieceStoreListings { get; set; }

        public DbSet<HandpieceStoreListingImage> HandpieceStoreListingImages { get; set; }

        public DbSet<HandpieceStoreOrder> HandpieceStoreOrders { get; set; }

        public DbSet<HandpieceStoreOrderItem> HandpieceStoreOrderItems { get; set; }

        public DbSet<FeedbackConfiguration> FeedbackConfiguration { get; set; }

        public DbSet<FeedbackFormQuestion> FeedbackFormQuestions { get; set; }

        public DbSet<FeedbackForm> FeedbackForms { get; set; }

        public DbSet<FeedbackFormAnswer> FeedbackFormAnswers { get; set; }

        public DbSet<ClientFeedbackOptions> ClientFeedbackOptions { get; set; }

        public DbSet<InventorySKUType> InventorySKUTypes { get; set; }

        public DbSet<InventorySKU> InventorySKUs { get; set; }

        public DbSet<InventorySKUWorkshopOptions> InventorySKUWorkshopOptions { get; set; }

        public DbSet<InventoryMovement> InventoryMovements { get; set; }

        public DbSet<InventoryMovementChange> InventoryMovementChanges { get; set; }

        public DbSet<HandpieceRequiredPart> HandpieceRequiredParts { get; set; }

        public DbSet<HandpieceRequiredPartMovement> HandpieceRequiredPartMovements { get; set; }

        public DbSet<LoggedEvent> LoggedEvents { get; set; }

        public DbSet<GenericFlag> GenericFlags { get; set; }

        public DbSet<TutorialPage> TutorialPages { get; set; }

        public DbSet<TutorialVideo> TutorialVideos { get; set; }

        public DbSet<EmployeeRole> EmployeeRoles { get; set; }

        public DbSet<WorkshopRole> WorkshopRoles { get; set; }

        public DbSet<WorkshopRoleJobType> WorkshopRoleJobTypes { get; set; }

        public DbSet<WorkshopRoleJobTypeStatus> WorkshopRoleJobTypeStatuses { get; set; }

        public DbSet<WorkshopRoleJobTypeJobException> WorkshopRoleJobTypeJobExceptions { get; set; }

        public DbSet<WorkshopRoleJobTypeHandpieceException> WorkshopRoleJobTypeHandpieceExceptions { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<ApplicationUser>().ToTable("Users");
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserRole<Guid>>().ToTable("UserRoles");
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("UserClaims");
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("UserLogins");
            builder.Entity<IdentityUserToken<Guid>>().ToTable("UserTokens");

            builder.Entity<Employee>(entity =>
            {
                entity.Property(x => x.AppearanceOpacity).HasColumnType("decimal(9,4)");
                entity.HasOne(x => x.Role).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<DiagnosticCheckType>(entity =>
            {
                entity.HasIndex(x => x.OrderNo).IsUnique();
            });

            builder.Entity<DiagnosticCheckItemType>(entity =>
            {
                entity.HasKey(x => new { x.ItemId, x.TypeId });
                entity.HasIndex(x => new { x.TypeId, x.OrderNo }).IsUnique();
            });

            builder.Entity<Workshop>(entity =>
            {
                entity.HasIndex(x => x.Name).HasFilter("[DeletionStatus] = 0").IsUnique();
                entity.HasIndex(x => x.OrderNo).HasFilter("[DeletionStatus] = 0").IsUnique();
            });
            builder.Entity<JobType>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();
            });
            builder.Entity<Job>(entity =>
            {
                entity.HasIndex(x => new { x.JobTypeId, x.JobNumber }).IsUnique();
                entity.HasOne(x => x.JobType).WithMany(x => x.Jobs).IsRequired().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Creator).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Workshop).WithMany(x => x.Jobs).IsRequired().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<JobInvoice>(entity =>
            {
                entity.HasIndex(x => new { x.JobId, x.InvoiceNumber }).IsUnique();
                entity.HasOne(x => x.Employee).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<JobChange>(entity =>
            {
                entity.HasOne(x => x.ChangedBy).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Handpiece>(entity =>
            {
                entity.HasOne(x => x.Creator).WithMany().IsRequired(true).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.EstimatedBy).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.RepairedBy).WithMany().IsRequired(false).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.ClientHandpiece).WithMany(x => x.Handpieces).OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.CostOfRepair).HasColumnType("decimal(18,2)");
                entity.HasIndex(x => new { x.JobId, x.JobPosition }).IsUnique();
                entity.HasIndex(x => x.Brand);
                entity.HasIndex(x => x.MakeAndModel);
            });
            builder.Entity<HandpieceComponent>(entity =>
            {
                entity.HasOne(x => x.Handpiece).WithMany(x => x.Components).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => new { x.HandpieceId, x.OrderNo }).IsUnique();
            });
            builder.Entity<HandpieceDiagnostic>(entity =>
            {
                entity.HasKey(x => new { x.HandpieceId, x.ItemId });
                entity.HasIndex(x => new { x.HandpieceId, x.OrderNo });
            });
            builder.Entity<HandpieceChange>(entity =>
            {
                entity.HasOne(x => x.ChangedBy).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientHandpiece>(entity =>
            {
            });
            builder.Entity<ClientHandpieceComponent>(entity =>
            {
            });

            builder.Entity<Client>(entity =>
            {
                entity.HasIndex(x => x.UrlPath).IsUnique();
                entity.HasIndex(x => x.FullName).IsUnique();
                entity.HasIndex(x => x.ClientNo).IsUnique();

                entity.HasOne(x => x.PrimaryWorkshop).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientAppearance>(entity =>
            {
                entity.HasIndex(x => x.ClientId).IsUnique();
                entity.HasOne(x => x.Logo).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.BackgroundImage).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientAttachment>(entity =>
            {
                entity.HasOne(x => x.Employee).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientEmailMessage>(entity =>
            {
                entity.HasKey(x => new { x.ClientId, x.EmailMessageId });
                entity.HasOne(x => x.EmailMessage).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientCallbackNotification>(entity =>
            {
                entity.HasOne(x => x.CreatedBy).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.AssignedTo).WithMany().OnDelete(DeleteBehavior.Restrict);
            });
            builder.Entity<ClientRepairedItemOverride>(entity =>
            {
                entity.HasIndex(x => new { x.ClientId, x.Brand, x.Model, x.Serial }).IsUnique();
            });

            builder.Entity<Corporate>(entity =>
            {
                entity.HasIndex(x => x.CorporateNo).IsUnique();
                entity.HasIndex(x => x.UrlPath).IsUnique();
            });
            builder.Entity<CorporateAppearance>(entity =>
            {
                entity.HasIndex(x => x.CorporateId).IsUnique();
                entity.HasOne(x => x.Logo).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.BackgroundImage).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<NotificationRelatedEntity>(entity =>
            {
                entity.HasIndex(x => new { x.NotificationId, x.EntityType, x.EntityId }).IsUnique();
            });

            builder.Entity<PickupRequestAnonymousRecipient>(entity =>
            {
                entity.HasIndex(x => x.AccountNumber).IsUnique();
                entity.HasIndex(x => x.UniqueKey).IsUnique();
            });

            builder.Entity<CalendarYear>(entity =>
            {
                entity.HasIndex(x => x.Year).IsUnique();
            });

            builder.Entity<CalendarWeek>(entity =>
            {
                entity.HasIndex(x => new { x.YearId, x.Week }).IsUnique();
            });

            builder.Entity<StockControlEntry>(entity =>
            {
                entity.HasIndex(x => x.HandpieceId).IsUnique();
            });

            builder.Entity<HandpieceBrand>(entity =>
            {
                entity.HasIndex(x => x.Name).IsUnique();
                entity.HasIndex(x => x.NormalizedName).IsUnique();
            });

            builder.Entity<HandpieceModel>(entity =>
            {
                entity.HasOne(x => x.Brand).WithMany(x => x.Models).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(x => new { x.BrandId, x.Name }).IsUnique();
                entity.HasIndex(x => new { x.BrandId, x.NormalizedName }).IsUnique();
                entity.Property(x => x.PriceNew).HasColumnType("decimal(18,2)");
            });

            builder.Entity<HandpieceModelSchematic>(entity =>
            {
                entity.HasOne(x => x.Model).WithMany(x => x.Schematics).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => new { x.ModelId, x.OrderNo }).IsUnique();
            });

            builder.Entity<CorporatePricingCategory>(entity =>
            {
                entity.HasIndex(x => x.OrderNo).IsUnique();
            });

            builder.Entity<CorporatePricingServiceLevel>(entity =>
            {
                entity.HasIndex(x => new { x.ServiceLevelId, x.CategoryId }).IsUnique();
                entity.Property(x => x.CostOfRepair).HasColumnType("decimal(18,2)");
            });

            builder.Entity<HandpieceStoreListing>(entity =>
            {
                entity.HasOne(x => x.Model).WithMany(x => x.StoreListings).OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(x => x.SerialNumber);
                entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
            });

            builder.Entity<HandpieceStoreListingImage>(entity =>
            {
                entity.HasOne(x => x.Listing).WithMany(x => x.Images).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => new { x.ListingId, x.OrderNo }).IsUnique();
            });

            builder.Entity<HandpieceStoreOrder>(entity =>
            {
                entity.HasIndex(x => x.OrderNumber).IsUnique();
            });

            builder.Entity<HandpieceStoreOrderItem>(entity =>
            {
                entity.HasOne(x => x.Order).WithMany(x => x.Items).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Listing).WithMany().OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<FeedbackConfiguration>(entity =>
            {
            });

            builder.Entity<FeedbackFormQuestion>(entity =>
            {
                entity.HasIndex(x => x.OrderNo).IsUnique();
            });

            builder.Entity<FeedbackForm>(entity =>
            {
            });

            builder.Entity<FeedbackFormAnswer>(entity =>
            {
                entity.HasIndex(x => new { x.FormId, x.QuestionId }).IsUnique();
            });

            builder.Entity<ClientFeedbackOptions>(entity =>
            {
                entity.HasIndex(x => x.ClientId).IsUnique();
            });

            builder.Entity<InventorySKUType>(entity =>
            {
                entity.HasIndex(x => x.OrderNo).HasFilter("[DeletionStatus] = 0").IsUnique();
            });

            builder.Entity<InventorySKU>(entity =>
            {
                entity.HasOne(x => x.Type).WithMany(x => x.SKUs).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Parent).WithMany(x => x.Children).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.DefaultChild).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.WarningQuantity).HasColumnType("decimal(18,3)");
                entity.Property(x => x.AveragePrice).HasColumnType("decimal(18,2)");

                entity.HasIndex(x => new { x.TypeId, x.OrderNo }).HasFilter("[ParentId] is null and [DeletionStatus] = 0").IsUnique();
                entity.HasIndex(x => new { x.TypeId, x.ParentId, x.OrderNo }).HasFilter("[ParentId] is not null and [DeletionStatus] = 0").IsUnique();
            });

            builder.Entity<InventorySKUWorkshopOptions>(entity =>
            {
                entity.HasKey(x => new { x.SKUId, x.WorkshopId });
                entity.HasOne(x => x.SKU).WithMany(x => x.WorkshopOptions).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Workshop).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.WarningQuantity).HasColumnType("decimal(18,3)");
            });

            builder.Entity<InventoryMovement>(entity =>
            {
                entity.HasOne(x => x.SKU).WithMany(x => x.Movements).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.Workshop).WithMany(x => x.InventoryMovements).OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.Quantity).HasColumnType("decimal(18,3)");
                entity.Property(x => x.QuantityAbsolute).HasColumnType("decimal(18,3)");
                entity.Property(x => x.Price).HasColumnType("decimal(18,2)");
                entity.HasIndex(x => new { x.SKUId, x.Direction, x.Type, x.Status });
            });

            builder.Entity<InventoryMovementChange>(entity =>
            {
                entity.HasOne(x => x.SKU).WithMany(x => x.MovementChanges).OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.OldQuantity).HasColumnType("decimal(18,3)");
                entity.Property(x => x.NewQuantity).HasColumnType("decimal(18,3)");
                entity.Property(x => x.OldPrice).HasColumnType("decimal(18,2)");
                entity.Property(x => x.NewPrice).HasColumnType("decimal(18,2)");
            });

            builder.Entity<HandpieceRequiredPart>(entity =>
            {
                entity.HasOne(x => x.Handpiece).WithMany(x => x.PartsRequired).OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.SKU).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.Property(x => x.Quantity).HasColumnType("decimal(18,3)");
                entity.HasIndex(x => new { x.HandpieceId, x.SKUId }).IsUnique();
            });

            builder.Entity<HandpieceRequiredPartMovement>(entity =>
            {
                entity.HasKey(x => new { x.RequiredPartId, x.MovementId });
                entity.HasIndex(x => x.MovementId).IsUnique();
            });

            builder.Entity<TutorialPage>(entity =>
            {
                entity.HasKey(x => x.Id);
            });

            builder.Entity<TutorialVideo>(entity =>
            {
                entity.HasOne(x => x.Page).WithMany(x => x.Videos).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => new { x.PageId, x.OrderNo }).IsUnique();
            });

            builder.Entity<EmployeeRole>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<WorkshopRole>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.Name).IsUnique();
            });

            builder.Entity<EmployeeRoleWorkshop>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.EmployeeRole).WithMany(x => x.WorkshopRoles).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.Workshop).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasOne(x => x.WorkshopRole).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(x => new { x.EmployeeRoleId, x.WorkshopId }).IsUnique();
            });

            builder.Entity<WorkshopRoleJobType>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.WorkshopRole).WithMany(x => x.JobTypePermissions).OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(x => x.JobType).WithMany().OnDelete(DeleteBehavior.Restrict);
                entity.HasIndex(x => new { x.WorkshopRoleId, x.JobTypeId }).IsUnique();
            });

            builder.Entity<WorkshopRoleJobTypeStatus>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.WorkshopRoleJobType).WithMany(x => x.StatusPermissions).OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(x => new { x.WorkshopRoleJobTypeId, x.JobStatus }).IsUnique();
            });

            builder.Entity<WorkshopRoleJobTypeJobException>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.WorkshopRoleJobType).WithMany(x => x.JobExceptions).OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<WorkshopRoleJobTypeHandpieceException>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasOne(x => x.WorkshopRoleJobType).WithMany(x => x.HandpieceExceptions).OnDelete(DeleteBehavior.Cascade);
            });

            // Comment this out when executing Add-Migration to prevent extra entities from getting into model snapshot
            this.OnViewsConfiguration(builder);
            this.OnQueriesConfiguration(builder);
        }

        protected void OnViewsConfiguration(ModelBuilder builder)
        {
            builder.Entity<InventorySKUHierarchy>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsHierarchy");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUAscendant>(view =>
            {
                view.HasNoKey();
                view.HasOne(x => x.Ascendant).WithMany();
                view.HasOne(x => x.Descendant).WithMany();
                view.ToView("InventorySKUsAscendants");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUQuantity>(view =>
            {
                view.HasNoKey();
                view.Property(x => x.TotalQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.TrayQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.OrderedQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.RequestedQuantity).HasColumnType("decimal(18,3)");
                view.ToView("InventorySKUsQuantity");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUWarning>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWarnings");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUDescendantsWarning>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsDescendantsWarnings");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUMissingQuantity>(view =>
            {
                view.HasNoKey();
                view.Property(x => x.MissingQuantity).HasColumnType("decimal(18,3)");
                view.ToView("InventorySKUsMissingQuantity");
                view.ToTable((String)null);
            });

            builder.Entity<InventoryMovementLinkedHandpiece>(view =>
            {
                view.HasNoKey();
                view.HasOne(x => x.Job).WithMany();
                view.HasOne(x => x.Handpiece).WithMany();
                view.ToView("InventoryMovementLinkedHandpiece");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUWorkshopQuantity>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWorkshopQuantity");
                view.ToTable((String)null);
                view.Property(x => x.TotalQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.TrayQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.OrderedQuantity).HasColumnType("decimal(18,3)");
                view.Property(x => x.RequestedQuantity).HasColumnType("decimal(18,3)");
            });

            builder.Entity<InventorySKUWorkshopRequiredQuantity>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWorkshopRequiredQuantity");
                view.ToTable((String)null);
                view.Property(x => x.RequiredQuantity).HasColumnType("decimal(18,3)");
            });

            builder.Entity<InventorySKUWorkshopMissingQuantity>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWorkshopMissingQuantity");
                view.ToTable((String)null);
                view.Property(x => x.MissingQuantity).HasColumnType("decimal(18,3)");
            });

            builder.Entity<InventorySKUWorkshopWarning>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWorkshopWarnings");
                view.ToTable((String)null);
            });

            builder.Entity<InventorySKUWorkshopDescendantsWarnings>(view =>
            {
                view.HasNoKey();
                view.ToView("InventorySKUsWorkshopDescendantsWarnings");
                view.ToTable((String)null);
            });
        }

        protected void OnQueriesConfiguration(ModelBuilder builder)
        {
            builder.Entity<GlobalHandpiecesReportItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.Cost).HasColumnType("decimal(18,2)");
                query.Property(x => x.Rating).HasColumnType("decimal(18,2)");
            });

            builder.Entity<GlobalTechWarrantyReportItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<GlobalBatchResultReportItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<CorporateSurgeryReportSurgeryItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.Cost).HasColumnType("decimal(18,2)");
                query.Property(x => x.Rating).HasColumnType("decimal(18,2)");
            });

            builder.Entity<CorporateSurgeryReportStatusItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<SurgeryReportSurgeryItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.Cost).HasColumnType("decimal(18,2)");
                query.Property(x => x.Rating).HasColumnType("decimal(18,2)");
            });

            builder.Entity<SurgeryReportStatusItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<ClientMaintenanceHandpiece>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<CorporateMaintenanceHandpiece>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<ClientActiveHandpiece>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<CorporateActiveHandpiece>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
            });

            builder.Entity<ClientIndexItem>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.AverageRating).HasColumnType("decimal(18,2)");
            });

            builder.Entity<InventorySKUIntermediateReadModel>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.AveragePrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.TotalQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.TrayQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.OrderedQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.RequestedQuantity).HasColumnType("decimal(18,3)");
            });

            builder.Entity<InventoryMovementReadModel>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.Quantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.QuantityAbsolute).HasColumnType("decimal(18,3)");
                query.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.AveragePrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.MovementPrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.FinalPrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.TotalPriceAbsolute).HasColumnType("decimal(18,2)");
            });

            builder.Entity<InventoryMovementGroupReadModel>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.Quantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.QuantityAbsolute).HasColumnType("decimal(18,3)");
                query.Property(x => x.QuantityAbsoluteWithPrice).HasColumnType("decimal(18,3)");
                query.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.OrderedQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.MissingQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.AverageFinalPrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.TotalPrice).HasColumnType("decimal(18,2)");
                query.Property(x => x.TotalPriceAbsolute).HasColumnType("decimal(18,2)");
            });

            builder.Entity<InventorySKUAvailableStatsModel>(query =>
            {
                query.HasNoKey();
                query.ToView((String)null);
                query.ToTable((String)null);
                query.Property(x => x.TotalQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.ShelfQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.TrayQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.OrderedQuantity).HasColumnType("decimal(18,3)");
                query.Property(x => x.RequestedQuantity).HasColumnType("decimal(18,3)");
            });
        }
    }
}
