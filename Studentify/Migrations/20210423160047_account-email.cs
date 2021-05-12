using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class accountemail : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "StudentifyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "StudentifyAccounts");
        }
    }
}
