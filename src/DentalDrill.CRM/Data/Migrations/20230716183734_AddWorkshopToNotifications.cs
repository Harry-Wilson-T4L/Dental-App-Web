using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddWorkshopToNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkshopId",
                table: "Notifications",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"update [Notifications] set [WorkshopId] = '3a3fd5ae-b473-4846-8eec-64f7830a9408' where [Type] in (1, 2, 3, 4)");
            migrationBuilder.Sql(@"update [Notifications] set [Scope] = 8 where [Type] = 5");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_WorkshopId",
                table: "Notifications",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_Workshops_WorkshopId",
                table: "Notifications",
                column: "WorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_Workshops_WorkshopId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_WorkshopId",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Notifications");
        }
    }
}
