using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiProject.Migrations
{
    public partial class newAttributetoBookings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DemandeSeats",
                table: "Bookings",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DemandeSeats",
                table: "Bookings");
        }
    }
}
