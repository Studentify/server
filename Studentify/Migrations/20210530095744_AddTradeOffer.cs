using Microsoft.EntityFrameworkCore.Migrations;

namespace Studentify.Migrations
{
    public partial class AddTradeOffer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "Events",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Offer",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Price",
                table: "Events",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Offer",
                table: "Events");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Events");
        }
    }
}
