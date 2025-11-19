using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class ReplaceTurnAroundWithReturnByInHandpieces : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TurnAround",
                table: "Handpieces");

            migrationBuilder.AddColumn<Guid>(
                name: "ReturnById",
                table: "Handpieces",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Handpieces_ReturnById",
                table: "Handpieces",
                column: "ReturnById");

            migrationBuilder.AddForeignKey(
                name: "FK_Handpieces_ReturnEstimates_ReturnById",
                table: "Handpieces",
                column: "ReturnById",
                principalTable: "ReturnEstimates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Handpieces_ReturnEstimates_ReturnById",
                table: "Handpieces");

            migrationBuilder.DropIndex(
                name: "IX_Handpieces_ReturnById",
                table: "Handpieces");

            migrationBuilder.DropColumn(
                name: "ReturnById",
                table: "Handpieces");

            migrationBuilder.AddColumn<int>(
                name: "TurnAround",
                table: "Handpieces",
                nullable: false,
                defaultValue: 0);
        }
    }
}
