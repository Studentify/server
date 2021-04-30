using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class CreateEventModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "StudentifyAccounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentifyAccounts_EventId",
                table: "StudentifyAccounts",
                column: "EventId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StudentifyAccounts_Events_EventId",
                table: "StudentifyAccounts",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentifyAccounts_Events_EventId",
                table: "StudentifyAccounts");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropIndex(
                name: "IX_StudentifyAccounts_EventId",
                table: "StudentifyAccounts");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "StudentifyAccounts");
        }
    }
}
