using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddHandpieceRequiredParts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PartsVersion",
                table: "Handpieces",
                nullable: true);

            migrationBuilder.Sql(@"update [Handpieces] set [PartsVersion] = 0", suppressTransaction: true);

            migrationBuilder.AlterColumn<int>(
                name: "PartsVersion",
                table: "Handpieces",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "HandpieceRequiredParts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    HandpieceId = table.Column<Guid>(nullable: false),
                    SKUId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HandpieceRequiredParts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HandpieceRequiredParts_Handpieces_HandpieceId",
                        column: x => x.HandpieceId,
                        principalTable: "Handpieces",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_HandpieceRequiredParts_InventorySKUs_SKUId",
                        column: x => x.SKUId,
                        principalTable: "InventorySKUs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HandpieceRequiredParts_HandpieceId",
                table: "HandpieceRequiredParts",
                column: "HandpieceId");

            migrationBuilder.CreateIndex(
                name: "IX_HandpieceRequiredParts_SKUId",
                table: "HandpieceRequiredParts",
                column: "SKUId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HandpieceRequiredParts");

            migrationBuilder.DropColumn(
                name: "PartsVersion",
                table: "Handpieces");
        }
    }
}
