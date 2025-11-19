using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddImportKeyToHandpieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImportKey",
                table: "Handpieces",
                maxLength: 200,
                nullable: true);

            migrationBuilder.Sql(@"update [Handpieces] set [SpeedType] = case [SpeedType] when 0 then 1 when 1 then 2 end", suppressTransaction: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImportKey",
                table: "Handpieces");
        }
    }
}
