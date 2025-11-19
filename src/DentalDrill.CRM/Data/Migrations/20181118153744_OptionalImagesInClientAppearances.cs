using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class OptionalImagesInClientAppearances : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAppearances_UploadedImages_BackgroundImageId",
                table: "ClientAppearances");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAppearances_UploadedImages_LogoId",
                table: "ClientAppearances");

            migrationBuilder.AlterColumn<Guid>(
                name: "LogoId",
                table: "ClientAppearances",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<Guid>(
                name: "BackgroundImageId",
                table: "ClientAppearances",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAppearances_UploadedImages_BackgroundImageId",
                table: "ClientAppearances",
                column: "BackgroundImageId",
                principalTable: "UploadedImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAppearances_UploadedImages_LogoId",
                table: "ClientAppearances",
                column: "LogoId",
                principalTable: "UploadedImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientAppearances_UploadedImages_BackgroundImageId",
                table: "ClientAppearances");

            migrationBuilder.DropForeignKey(
                name: "FK_ClientAppearances_UploadedImages_LogoId",
                table: "ClientAppearances");

            migrationBuilder.AlterColumn<Guid>(
                name: "LogoId",
                table: "ClientAppearances",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "BackgroundImageId",
                table: "ClientAppearances",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAppearances_UploadedImages_BackgroundImageId",
                table: "ClientAppearances",
                column: "BackgroundImageId",
                principalTable: "UploadedImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ClientAppearances_UploadedImages_LogoId",
                table: "ClientAppearances",
                column: "LogoId",
                principalTable: "UploadedImages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
