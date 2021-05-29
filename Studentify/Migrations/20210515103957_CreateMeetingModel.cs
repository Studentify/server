using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class CreateMeetingModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxNumberOfParticipants",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "MeetingStudentifyAccount",
                columns: table => new
                {
                    MeetingsId = table.Column<int>(type: "int", nullable: false),
                    ParticipantsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MeetingStudentifyAccount", x => new { x.MeetingsId, x.ParticipantsId });
                    table.ForeignKey(
                        name: "FK_MeetingStudentifyAccount_Events_MeetingsId",
                        column: x => x.MeetingsId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_MeetingStudentifyAccount_StudentifyAccounts_ParticipantsId",
                        column: x => x.ParticipantsId,
                        principalTable: "StudentifyAccounts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MeetingStudentifyAccount_ParticipantsId",
                table: "MeetingStudentifyAccount",
                column: "ParticipantsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MeetingStudentifyAccount");

            migrationBuilder.DropColumn(
                name: "MaxNumberOfParticipants",
                table: "Events");
        }
    }
}
