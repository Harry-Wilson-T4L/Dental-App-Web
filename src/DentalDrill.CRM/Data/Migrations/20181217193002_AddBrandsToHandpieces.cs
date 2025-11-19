using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddBrandsToHandpieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Brand",
                table: "Handpieces",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OldMakeAndModel",
                table: "Handpieces",
                maxLength: 200,
                nullable: true);

            migrationBuilder.Sql("update [dbo].[Handpieces] set [Brand] = 'N/A', [OldMakeAndModel] = [MakeAndModel]", suppressTransaction: true);

            migrationBuilder.AlterColumn<string>(
                name: "Brand",
                table: "Handpieces",
                maxLength: 100,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Handpieces_Brand",
                table: "Handpieces",
                column: "Brand");

            migrationBuilder.CreateIndex(
                name: "IX_Handpieces_MakeAndModel",
                table: "Handpieces",
                column: "MakeAndModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Handpieces_Brand",
                table: "Handpieces");

            migrationBuilder.DropIndex(
                name: "IX_Handpieces_MakeAndModel",
                table: "Handpieces");

            migrationBuilder.DropColumn(
                name: "Brand",
                table: "Handpieces");

            migrationBuilder.DropColumn(
                name: "OldMakeAndModel",
                table: "Handpieces");
        }
    }
}
