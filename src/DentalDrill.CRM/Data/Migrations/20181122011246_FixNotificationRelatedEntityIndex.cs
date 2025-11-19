using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class FixNotificationRelatedEntityIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationRelatedEntity_NotificationId",
                table: "NotificationRelatedEntity");

            migrationBuilder.DropIndex(
                name: "IX_NotificationRelatedEntity_EntityType_EntityId",
                table: "NotificationRelatedEntity");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRelatedEntity_NotificationId_EntityType_EntityId",
                table: "NotificationRelatedEntity",
                columns: new[] { "NotificationId", "EntityType", "EntityId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_NotificationRelatedEntity_NotificationId_EntityType_EntityId",
                table: "NotificationRelatedEntity");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRelatedEntity_NotificationId",
                table: "NotificationRelatedEntity",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationRelatedEntity_EntityType_EntityId",
                table: "NotificationRelatedEntity",
                columns: new[] { "EntityType", "EntityId" },
                unique: true);
        }
    }
}
