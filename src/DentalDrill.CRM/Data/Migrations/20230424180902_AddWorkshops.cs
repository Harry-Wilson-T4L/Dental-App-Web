using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddWorkshops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Workshops",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderNo = table.Column<int>(type: "int", nullable: false),
                    DeletionStatus = table.Column<int>(type: "int", nullable: false),
                    DeletionDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Workshops", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_Name",
                table: "Workshops",
                column: "Name",
                unique: true,
                filter: "[DeletionStatus] = 0");

            migrationBuilder.CreateIndex(
                name: "IX_Workshops_OrderNo",
                table: "Workshops",
                column: "OrderNo",
                unique: true,
                filter: "[DeletionStatus] = 0");

            migrationBuilder.InsertData(
                "Workshops",
                new String[] { "Id", "Name", "Description", "OrderNo", "DeletionStatus", "DeletionDate" },
                new Object[,]
                {
                    { new Guid("3a3fd5ae-b473-4846-8eec-64f7830a9408"), "Sydney", "Sydney workshop", 1, 0, null, },
                });

            migrationBuilder.AddColumn<Guid>(
                name: "WorkshopId",
                table: "Jobs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("update [Jobs] set [WorkshopId] = '3a3fd5ae-b473-4846-8eec-64f7830a9408'");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkshopId",
                table: "Jobs",
                type: "uniqueidentifier",
                oldNullable: true,
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "WorkshopId",
                table: "InventoryMovements",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql("update [InventoryMovements] set [WorkshopId] = '3a3fd5ae-b473-4846-8eec-64f7830a9408'");

            migrationBuilder.AlterColumn<Guid>(
                name: "WorkshopId",
                table: "InventoryMovements",
                type: "uniqueidentifier",
                oldNullable: true,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Jobs_WorkshopId",
                table: "Jobs",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_InventoryMovements_WorkshopId",
                table: "InventoryMovements",
                column: "WorkshopId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryMovements_Workshops_WorkshopId",
                table: "InventoryMovements",
                column: "WorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Jobs_Workshops_WorkshopId",
                table: "Jobs",
                column: "WorkshopId",
                principalTable: "Workshops",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryMovements_Workshops_WorkshopId",
                table: "InventoryMovements");

            migrationBuilder.DropForeignKey(
                name: "FK_Jobs_Workshops_WorkshopId",
                table: "Jobs");

            migrationBuilder.DropTable(
                name: "Workshops");

            migrationBuilder.DropIndex(
                name: "IX_Jobs_WorkshopId",
                table: "Jobs");

            migrationBuilder.DropIndex(
                name: "IX_InventoryMovements_WorkshopId",
                table: "InventoryMovements");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "Jobs");

            migrationBuilder.DropColumn(
                name: "WorkshopId",
                table: "InventoryMovements");
        }
    }
}
