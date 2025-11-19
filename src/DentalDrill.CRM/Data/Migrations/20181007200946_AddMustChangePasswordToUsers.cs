using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddMustChangePasswordToUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePasswordAtNextLogin",
                table: "Users",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MustChangePasswordAtNextLogin",
                table: "Users");
        }
    }
}
