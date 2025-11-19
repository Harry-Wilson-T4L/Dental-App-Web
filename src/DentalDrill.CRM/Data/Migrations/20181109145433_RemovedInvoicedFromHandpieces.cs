using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class RemovedInvoicedFromHandpieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Invoiced",
                table: "Handpieces");

            migrationBuilder.DropColumn(
                name: "InvoicedCost",
                table: "Handpieces");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Invoiced",
                table: "Handpieces",
                type: "date",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InvoicedCost",
                table: "Handpieces",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
