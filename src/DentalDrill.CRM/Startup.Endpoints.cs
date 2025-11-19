using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DentalDrill.CRM.Hubs;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace DentalDrill.CRM
{
    public partial class Startup
    {
        public void ConfigureEndpoints(IEndpointRouteBuilder routes)
        {
            routes.MapHub<NotificationsHub>("/hub/Notifications");

            routes.MapControllerRoute(
                name: "Surgeries-ReadMaintenanceHistory",
                pattern: "Surgeries/{parentId}/Maintenance/History",
                defaults: new { controller = "Surgeries", action = "ReadMaintenanceHistory" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadMaintenanceHandpieces",
                pattern: "Surgeries/{parentId}/Maintenance/Read",
                defaults: new { controller = "Surgeries", action = "ReadMaintenanceHandpieces" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadHandpieces",
                pattern: "Surgeries/{parentId}/Handpieces",
                defaults: new { controller = "Surgeries", action = "ReadHandpieces" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadJobNumbers",
                pattern: "Surgeries/{parentId}/JobNumbers",
                defaults: new { controller = "Surgeries", action = "ReadJobNumbers" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadSerials",
                pattern: "Surgeries/{parentId}/Serials",
                defaults: new { controller = "Surgeries", action = "ReadSerials" });

            routes.MapControllerRoute(
                name: "Surgeries-Handpiece",
                pattern: "Surgeries/{parentId}/Handpiece/{id}",
                defaults: new { controller = "Surgeries", action = "Handpiece" });

            routes.MapControllerRoute(
                name: "Surgeries-HandpieceImages",
                pattern: "Surgeries/{parentId}/Handpiece/{id}/Images",
                defaults: new { controller = "Surgeries", action = "HandpieceImages" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadReportBrands",
                pattern: "Surgeries/{parentId}/Reports/Brands",
                defaults: new { controller = "Surgeries", action = "ReadReportBrands" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadReportBrandsByModel",
                pattern: "Surgeries/{parentId}/Reports/Brands/ByModel",
                defaults: new { controller = "Surgeries", action = "ReadReportBrandsByModel" });

            routes.MapControllerRoute(
                name: "Surgeries-ExportBrandsToExcel",
                pattern: "Surgeries/{parentId}/Reports/Brands/Export/Excel",
                defaults: new { controller = "Surgeries", action = "ExportBrandsToExcel" });

            routes.MapControllerRoute(
                name: "Surgeries-ReadReportStatuses",
                pattern: "Surgeries/{parentId}/Reports/Statuses",
                defaults: new { controller = "Surgeries", action = "ReadReportStatuses" });

            routes.MapControllerRoute(
                name: "Surgeries-ExportStatusesToExcel",
                pattern: "Surgeries/{parentId}/Reports/Statuses/Export/Excel",
                defaults: new { controller = "Surgeries", action = "ExportStatusesToExcel" });

            routes.MapControllerRoute(
                name: "Surgeries-Reports",
                pattern: "Surgeries/{id}/Reports",
                defaults: new { controller = "Surgeries", action = "Reports" });

            routes.MapControllerRoute(
                name: "Surgeries",
                pattern: "Surgeries/{id}",
                defaults: new { controller = "Surgeries", action = "Index" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadMaintenanceHistory",
                pattern: "CorporateSurgeries/{parentId}/Maintenance/History",
                defaults: new { controller = "CorporateSurgeries", action = "ReadMaintenanceHistory" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadMaintenanceHandpieces",
                pattern: "CorporateSurgeries/{parentId}/Maintenance/Read",
                defaults: new { controller = "CorporateSurgeries", action = "ReadMaintenanceHandpieces" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadHandpieces",
                pattern: "CorporateSurgeries/{parentId}/Handpieces",
                defaults: new { controller = "CorporateSurgeries", action = "ReadHandpieces" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadJobNumbers",
                pattern: "CorporateSurgeries/{parentId}/JobNumbers",
                defaults: new { controller = "CorporateSurgeries", action = "ReadJobNumbers" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadSerials",
                pattern: "CorporateSurgeries/{parentId}/Serials",
                defaults: new { controller = "CorporateSurgeries", action = "ReadSerials" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-Handpiece",
                pattern: "CorporateSurgeries/{parentId}/Handpiece/{id}",
                defaults: new { controller = "CorporateSurgeries", action = "Handpiece" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-HandpieceImages",
                pattern: "CorporateSurgeries/{parentId}/Handpiece/{id}/Images",
                defaults: new { controller = "CorporateSurgeries", action = "HandpieceImages" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportHandpieces",
                pattern: "CorporateSurgeries/{parentId}/Reports/Handpieces",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportHandpieces" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ExportHandpiecesToExcel",
                pattern: "CorporateSurgeries/{parentId}/Reports/Handpieces/Export/Excel",
                defaults: new { controller = "CorporateSurgeries", action = "ExportHandpiecesToExcel" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportHandpiecesByModel",
                pattern: "CorporateSurgeries/{parentId}/Reports/Handpieces/ByModel",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportHandpiecesByModel" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportHandpiecesByBrands",
                pattern: "CorporateSurgeries/{parentId}/Reports/Handpieces/ByBrands",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportHandpiecesByBrands" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportBrands",
                pattern: "CorporateSurgeries/{parentId}/Reports/Brands",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportBrands" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ExportBrandsToExcel",
                pattern: "CorporateSurgeries/{parentId}/Reports/Brands/Export/Excel",
                defaults: new { controller = "CorporateSurgeries", action = "ExportBrandsToExcel" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportBrandsByModel",
                pattern: "CorporateSurgeries/{parentId}/Reports/Brands/ByModel",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportBrandsByModel" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportBrandsBySurgeries",
                pattern: "CorporateSurgeries/{parentId}/Reports/Brands/BySurgeries",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportBrandsBySurgeries" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ReadReportStatuses",
                pattern: "CorporateSurgeries/{parentId}/Reports/Statuses",
                defaults: new { controller = "CorporateSurgeries", action = "ReadReportStatuses" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-ExportStatusesToExcel",
                pattern: "CorporateSurgeries/{parentId}/Reports/Statuses/Export/Excel",
                defaults: new { controller = "CorporateSurgeries", action = "ExportStatusesToExcel" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries-Reports",
                pattern: "CorporateSurgeries/{id}/Reports",
                defaults: new { controller = "CorporateSurgeries", action = "Reports" });

            routes.MapControllerRoute(
                name: "CorporateSurgeries",
                pattern: "CorporateSurgeries/{id}",
                defaults: new { controller = "CorporateSurgeries", action = "Index" });

            routes.MapControllerRoute(
                name: "HomeHubTutorials",
                pattern: "HubTutorials",
                defaults: new { controller = "Home", action = "HubTutorial" });

            routes.MapControllerRoute(
                name: "HomeSurgeryTutorials",
                pattern: "SurgeryTutorials",
                defaults: new { controller = "Home", action = "SurgeryTutorials" });

            routes.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");
        }
    }
}
