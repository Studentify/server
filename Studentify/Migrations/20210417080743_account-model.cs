using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class accountmodel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccountInfo",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "StudentifyAccounts",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentifyUserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentifyAccounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentifyAccounts_AspNetUsers_StudentifyUserId",
                        column: x => x.StudentifyUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentifyAccounts_StudentifyUserId",
                table: "StudentifyAccounts",
                column: "StudentifyUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentifyAccounts");

            migrationBuilder.AddColumn<string>(
                name: "AccountInfo",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
