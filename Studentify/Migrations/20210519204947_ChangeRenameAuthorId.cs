using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class ChangeRenameAuthorId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_StudentifyAccounts_StudentifyAccountId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "StudentifyAccountId",
                table: "Events",
                newName: "AuthorId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_StudentifyAccountId",
                table: "Events",
                newName: "IX_Events_AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_StudentifyAccounts_AuthorId",
                table: "Events",
                column: "AuthorId",
                principalTable: "StudentifyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Events_StudentifyAccounts_AuthorId",
                table: "Events");

            migrationBuilder.RenameColumn(
                name: "AuthorId",
                table: "Events",
                newName: "StudentifyAccountId");

            migrationBuilder.RenameIndex(
                name: "IX_Events_AuthorId",
                table: "Events",
                newName: "IX_Events_StudentifyAccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Events_StudentifyAccounts_StudentifyAccountId",
                table: "Events",
                column: "StudentifyAccountId",
                principalTable: "StudentifyAccounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
