using Microsoft.EntityFrameworkCore.Migrations;

namespace DentalDrill.CRM.Data.Migrations
{
    public partial class AddFullNameToClients : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "Clients",
                maxLength: 500,
                nullable: true);

            migrationBuilder.Sql(
                @"declare @id uniqueidentifier
declare @dentist nvarchar(100)
declare @name nvarchar(200)
declare @suburb nvarchar(200)
declare @clientNo int

declare @clientsCursor cursor
set @clientsCursor = cursor for select [Id], coalesce([PrincipalDentist], ''), coalesce([Name], ''), coalesce([Suburb], ''), [ClientNo] from [Clients]

open @clientsCursor
fetch next from @clientsCursor into @id, @dentist, @name, @suburb, @clientNo
while @@fetch_status = 0 begin
declare @fullname nvarchar(500)

set @fullname = trim(@name)
if len(@fullname) > 0 and len(@dentist) > 0 begin
	set @fullname = @fullname + ' - '
end
set @fullname = @fullname + trim(@dentist)
if len(@fullname) > 0 and len(@suburb) > 0 begin
	set @fullname = @fullname + ' - '
end
set @fullname = @fullname + @suburb

if exists(select 1 from [Clients] where [FullName] = @fullname) begin
	set @fullname = @fullname + ' - Client' + cast(@clientNo as nvarchar(10))
end

update [Clients] set [FullName] = @fullname where [Id] = @id
	
fetch next from @clientsCursor into @id, @dentist, @name, @suburb, @clientNo
end

close @clientsCursor
deallocate @clientsCursor", suppressTransaction: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "Clients",
                maxLength: 500,
                nullable: false,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Clients_FullName",
                table: "Clients",
                column: "FullName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Clients_FullName",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "Clients");
        }
    }
}
