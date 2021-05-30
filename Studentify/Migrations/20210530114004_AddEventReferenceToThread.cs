using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class AddEventReferenceToThread : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "EventId",
                table: "Threads",
                newName: "ReferencedEventId");

            migrationBuilder.CreateIndex(
                name: "IX_Threads_ReferencedEventId",
                table: "Threads",
                column: "ReferencedEventId");

            migrationBuilder.AddForeignKey(
                name: "FK_Threads_Events_ReferencedEventId",
                table: "Threads",
                column: "ReferencedEventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Threads_Events_ReferencedEventId",
                table: "Threads");

            migrationBuilder.DropIndex(
                name: "IX_Threads_ReferencedEventId",
                table: "Threads");

            migrationBuilder.RenameColumn(
                name: "ReferencedEventId",
                table: "Threads",
                newName: "EventId");
        }
    }
}
