using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddOrderNoToDiagnosticCheckItemType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiagnosticCheckItemTypes_TypeId",
                table: "DiagnosticCheckItemTypes");

            migrationBuilder.AddColumn<int>(
                name: "OrderNo",
                table: "DiagnosticCheckItemTypes",
                type: "int",
                nullable: true);

            migrationBuilder.Sql(@"update
  [DiagnosticCheckItemTypes]
set
  [OrderNo] = q.[OrderNo]
from (
  select
    [DiagnosticCheckItemTypes].[ItemId],
    [DiagnosticCheckItemTypes].[TypeId],
    row_number() over (partition by [DiagnosticCheckItemTypes].[TypeId] order by [DiagnosticCheckItems].[Name]) as [OrderNo]
  from [DiagnosticCheckItemTypes]
  inner join [DiagnosticCheckItems] on [DiagnosticCheckItemTypes].[ItemId] = [DiagnosticCheckItems].[Id]) q
where q.[ItemId] = [DiagnosticCheckItemTypes].[ItemId] and q.[TypeId] = [DiagnosticCheckItemTypes].[TypeId]");

            migrationBuilder.AlterColumn<int>(
                name: "OrderNo",
                table: "DiagnosticCheckItemTypes",
                type: "int",
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticCheckItemTypes_TypeId_OrderNo",
                table: "DiagnosticCheckItemTypes",
                columns: new[] { "TypeId", "OrderNo" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DiagnosticCheckItemTypes_TypeId_OrderNo",
                table: "DiagnosticCheckItemTypes");

            migrationBuilder.DropColumn(
                name: "OrderNo",
                table: "DiagnosticCheckItemTypes");

            migrationBuilder.CreateIndex(
                name: "IX_DiagnosticCheckItemTypes_TypeId",
                table: "DiagnosticCheckItemTypes",
                column: "TypeId");
        }
    }
}
