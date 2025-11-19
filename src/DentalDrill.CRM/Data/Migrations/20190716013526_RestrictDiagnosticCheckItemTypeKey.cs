using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class RestrictDiagnosticCheckItemTypeKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagnosticCheckItems_DiagnosticCheckTypes_TypeId",
                table: "DiagnosticCheckItems");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosticCheckItems_DiagnosticCheckTypes_TypeId",
                table: "DiagnosticCheckItems",
                column: "TypeId",
                principalTable: "DiagnosticCheckTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DiagnosticCheckItems_DiagnosticCheckTypes_TypeId",
                table: "DiagnosticCheckItems");

            migrationBuilder.AddForeignKey(
                name: "FK_DiagnosticCheckItems_DiagnosticCheckTypes_TypeId",
                table: "DiagnosticCheckItems",
                column: "TypeId",
                principalTable: "DiagnosticCheckTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
