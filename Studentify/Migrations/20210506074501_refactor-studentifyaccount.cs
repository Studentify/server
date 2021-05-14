using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class refactorstudentifyaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "StudentifyAccounts");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "StudentifyAccounts");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "StudentifyAccounts");

            migrationBuilder.DropColumn(
                name: "StudentifyUsername",
                table: "StudentifyAccounts");

            migrationBuilder.AddColumn<string>(
                name: "StudentifyUserId",
                table: "StudentifyAccounts",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_StudentifyAccounts_StudentifyUserId",
                table: "StudentifyAccounts",
                column: "StudentifyUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentifyAccounts_AspNetUsers_StudentifyUserId",
                table: "StudentifyAccounts",
                column: "StudentifyUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentifyAccounts_AspNetUsers_StudentifyUserId",
                table: "StudentifyAccounts");

            migrationBuilder.DropIndex(
                name: "IX_StudentifyAccounts_StudentifyUserId",
                table: "StudentifyAccounts");

            migrationBuilder.DropColumn(
                name: "StudentifyUserId",
                table: "StudentifyAccounts");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "StudentifyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "StudentifyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "StudentifyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StudentifyUsername",
                table: "StudentifyAccounts",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
