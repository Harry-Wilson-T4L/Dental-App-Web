using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddSpeedToHandpieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "Handpieces",
                newName: "SpeedType");

            migrationBuilder.AddColumn<int>(
                name: "Speed",
                table: "Handpieces",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql("update [Handpieces] set [SpeedType] = 0 where [SpeedType] > 1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Speed",
                table: "Handpieces");

            migrationBuilder.RenameColumn(
                name: "SpeedType",
                table: "Handpieces",
                newName: "Type");
        }
    }
}
