using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddEmployeeRoles : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EmployeeRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<String>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    GlobalComponentRead = table.Column<Int64>(type: "bigint", nullable: false),
                    GlobalComponentWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    ClientComponentRead = table.Column<Int64>(type: "bigint", nullable: false),
                    ClientComponentWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    ClientFieldRead = table.Column<Int64>(type: "bigint", nullable: false),
                    ClientFieldWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    ClientFieldInit = table.Column<Int64>(type: "bigint", nullable: false),
                    InventoryAccess = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRoles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<String>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    WorkshopAccess = table.Column<Int64>(type: "bigint", nullable: false),
                    InventoryAccess = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeRoleWorkshop",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeType = table.Column<Int32>(type: "int", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeRoleWorkshop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleWorkshop_EmployeeRoles_EmployeeRoleId",
                        column: x => x.EmployeeRoleId,
                        principalTable: "EmployeeRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleWorkshop_WorkshopRoles_WorkshopRoleId",
                        column: x => x.WorkshopRoleId,
                        principalTable: "WorkshopRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EmployeeRoleWorkshop_Workshops_WorkshopId",
                        column: x => x.WorkshopId,
                        principalTable: "Workshops",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRoleJobTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobTypeId = table.Column<String>(type: "nvarchar(20)", nullable: true),
                    JobComponentRead = table.Column<Int64>(type: "bigint", nullable: false),
                    JobComponentWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    JobFieldRead = table.Column<Int64>(type: "bigint", nullable: false),
                    JobFieldWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    JobFieldInit = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceComponentRead = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceComponentWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceFieldRead = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceFieldWrite = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceFieldInit = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopRoleJobTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopRoleJobTypes_JobTypes_JobTypeId",
                        column: x => x.JobTypeId,
                        principalTable: "JobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WorkshopRoleJobTypes_WorkshopRoles_WorkshopRoleId",
                        column: x => x.WorkshopRoleId,
                        principalTable: "WorkshopRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRoleJobTypeHandpieceExceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopRoleJobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WhenJobStatus = table.Column<Int64>(type: "bigint", nullable: false),
                    WhenHandpieceStatus = table.Column<Int64>(type: "bigint", nullable: false),
                    ReadOnlyFields = table.Column<Int64>(type: "bigint", nullable: false),
                    HiddenFields = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopRoleJobTypeHandpieceExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopRoleJobTypeHandpieceExceptions_WorkshopRoleJobTypes_WorkshopRoleJobTypeId",
                        column: x => x.WorkshopRoleJobTypeId,
                        principalTable: "WorkshopRoleJobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRoleJobTypeJobExceptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopRoleJobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WhenJobStatus = table.Column<Int64>(type: "bigint", nullable: false),
                    ReadOnlyFields = table.Column<Int64>(type: "bigint", nullable: false),
                    HiddenFields = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopRoleJobTypeJobExceptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopRoleJobTypeJobExceptions_WorkshopRoleJobTypes_WorkshopRoleJobTypeId",
                        column: x => x.WorkshopRoleJobTypeId,
                        principalTable: "WorkshopRoleJobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "WorkshopRoleJobTypeStatuses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkshopRoleJobTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobStatus = table.Column<Int32>(type: "int", nullable: false),
                    JobTransitions = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceTransitionsFrom = table.Column<Int64>(type: "bigint", nullable: false),
                    HandpieceTransitionsTo = table.Column<Int64>(type: "bigint", nullable: false),
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WorkshopRoleJobTypeStatuses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WorkshopRoleJobTypeStatuses_WorkshopRoleJobTypes_WorkshopRoleJobTypeId",
                        column: x => x.WorkshopRoleJobTypeId,
                        principalTable: "WorkshopRoleJobTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "EmployeeRoles",
                columns: new String[] { "Id", "Name", "GlobalComponentRead", "GlobalComponentWrite", "ClientComponentRead", "ClientComponentWrite", "ClientFieldRead", "ClientFieldWrite", "ClientFieldInit", "InventoryAccess" },
                values: new Object[,]
                {
                    { new Guid("ecde7eca-80ea-4d75-9e36-ea2d4f18e71b"), "Workshop technician", 0, 0, 0, 0, 0, 0, 0, 0 },
                    { new Guid("f3557611-5386-4130-9aaa-6cac03ab1e4f"), "Office administrator", 0, 0, 0, 0, 0, 0, 0, 0 },
                    { new Guid("609b0def-6340-4643-b2ec-ce83d5873aa8"), "Company administrator", 0, 0, 0, 0, 0, 0, 0, 0 },
                    { new Guid("5ca8087f-3970-4db3-839c-25ccb7de3805"), "Company manager", 0, 0, 0, 0, 0, 0, 0, 0 },
                });

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Employees",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.Sql(@"update [Employees] set [RoleId] = case [Type]
when 0 then 'ecde7eca-80ea-4d75-9e36-ea2d4f18e71b'
when 1 then 'f3557611-5386-4130-9aaa-6cac03ab1e4f'
when 2 then '609b0def-6340-4643-b2ec-ce83d5873aa8'
when 3 then '5ca8087f-3970-4db3-839c-25ccb7de3805'
else null end");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoleId",
                table: "Employees",
                type: "uniqueidentifier",
                oldNullable: true,
                nullable: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_RoleId",
                table: "Employees",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoles_Name",
                table: "EmployeeRoles",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleWorkshop_EmployeeRoleId_WorkshopId",
                table: "EmployeeRoleWorkshop",
                columns: new[] { "EmployeeRoleId", "WorkshopId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleWorkshop_WorkshopId",
                table: "EmployeeRoleWorkshop",
                column: "WorkshopId");

            migrationBuilder.CreateIndex(
                name: "IX_EmployeeRoleWorkshop_WorkshopRoleId",
                table: "EmployeeRoleWorkshop",
                column: "WorkshopRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoleJobTypeHandpieceExceptions_WorkshopRoleJobTypeId",
                table: "WorkshopRoleJobTypeHandpieceExceptions",
                column: "WorkshopRoleJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoleJobTypeJobExceptions_WorkshopRoleJobTypeId",
                table: "WorkshopRoleJobTypeJobExceptions",
                column: "WorkshopRoleJobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoleJobTypes_JobTypeId",
                table: "WorkshopRoleJobTypes",
                column: "JobTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoleJobTypes_WorkshopRoleId_JobTypeId",
                table: "WorkshopRoleJobTypes",
                columns: new[] { "WorkshopRoleId", "JobTypeId" },
                unique: true,
                filter: "[JobTypeId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoleJobTypeStatuses_WorkshopRoleJobTypeId_JobStatus",
                table: "WorkshopRoleJobTypeStatuses",
                columns: new[] { "WorkshopRoleJobTypeId", "JobStatus" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkshopRoles_Name",
                table: "WorkshopRoles",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_EmployeeRoles_RoleId",
                table: "Employees",
                column: "RoleId",
                principalTable: "EmployeeRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_EmployeeRoles_RoleId",
                table: "Employees");

            migrationBuilder.DropTable(
                name: "EmployeeRoleWorkshop");

            migrationBuilder.DropTable(
                name: "WorkshopRoleJobTypeHandpieceExceptions");

            migrationBuilder.DropTable(
                name: "WorkshopRoleJobTypeJobExceptions");

            migrationBuilder.DropTable(
                name: "WorkshopRoleJobTypeStatuses");

            migrationBuilder.DropTable(
                name: "EmployeeRoles");

            migrationBuilder.DropTable(
                name: "WorkshopRoleJobTypes");

            migrationBuilder.DropTable(
                name: "WorkshopRoles");

            migrationBuilder.DropIndex(
                name: "IX_Employees_RoleId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Employees");
        }
    }
}
