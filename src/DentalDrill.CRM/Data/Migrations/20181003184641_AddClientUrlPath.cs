using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddClientUrlPath : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 100);

            migrationBuilder.AddColumn<string>(
                name: "UrlPath",
                table: "Clients",
                nullable: true);

            migrationBuilder.Sql("UPDATE [dbo].[Clients] SET [UrlPath] = TRIM([Name])", suppressTransaction: true);

            migrationBuilder.AlterColumn<string>(
                "UrlPath",
                "Clients",
                nullable: false,
                oldClrType: typeof(string),
                maxLength: 200);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_Name",
                table: "Clients",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UrlPath",
                table: "Clients",
                column: "UrlPath",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_Name",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_UrlPath",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UrlPath",
                table: "Clients");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "Clients",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 200);
        }
    }
}
