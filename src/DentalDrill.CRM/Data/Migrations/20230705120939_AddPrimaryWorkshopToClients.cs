using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddPrimaryWorkshopToClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PrimaryWorkshopId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("update [Clients] set [PrimaryWorkshopId] = '3a3fd5ae-b473-4846-8eec-64f7830a9408'");

            migrationBuilder.AlterColumn<Guid>(
                name: "PrimaryWorkshopId",
                table: "Clients",
                type: "uniqueidentifier",
                oldNullable: true,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_PrimaryWorkshopId",
                table: "Clients",
                column: "PrimaryWorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Workshops_PrimaryWorkshopId",
                table: "Clients",
                column: "PrimaryWorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Workshops_PrimaryWorkshopId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_PrimaryWorkshopId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "PrimaryWorkshopId",
                table: "Clients");
        }
    }
}
