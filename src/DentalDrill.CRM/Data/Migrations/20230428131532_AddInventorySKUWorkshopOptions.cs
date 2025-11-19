using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddInventorySKUWorkshopOptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InventorySKUWorkshopOptions",
                columns: table => new
                {
                    SKUId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WarningQuantity = table.Column<decimal>(type: "decimal(18,3)", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InventorySKUWorkshopOptions", x => new { x.SKUId, x.WorkshopId });
                    table.ForeignKey(
                        name: "FK_InventorySKUWorkshopOptions_InventorySKUs_SKUId",
                        column: x => x.SKUId,
                        principalTable: "InventorySKUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_InventorySKUWorkshopOptions_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InventorySKUWorkshopOptions_WorkshopId",
                table: "InventorySKUWorkshopOptions",
                column: "WorkshopId");

            migrationBuilder.Sql(@"insert into [InventorySKUWorkshopOptions] ([SKUId], [WorkshopId], [WarningQuantity])
select [InventorySKUs].[Id] as [SKUId], [Workshops].[Id] as [WorkshopId], [InventorySKUs].[WarningQuantity]
from [InventorySKUs] cross join [Workshops]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InventorySKUWorkshopOptions");
        }
    }
}
