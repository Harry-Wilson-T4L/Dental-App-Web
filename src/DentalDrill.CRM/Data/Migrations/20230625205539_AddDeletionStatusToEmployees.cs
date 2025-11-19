using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddDeletionStatusToEmployees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Int32>(
                name: "DeletionStatus",
                table: "Employees",
                type: "int",
                nullable: true);

            migrationBuilder.Sql("update [Employees] set [DeletionStatus] = 0");

            migrationBuilder.AlterColumn<Int32>(
                name: "DeletionStatus",
                table: "Employees",
                type: "int",
                oldNullable: true,
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeletionStatus",
                table: "Employees");
        }
    }
}
