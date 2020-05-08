using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiProject.Migrations
{
    public partial class addTripsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Trips",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TripNum = table.Column<string>(maxLength: 500, nullable: true),
                    CitiesFromId = table.Column<int>(nullable: false),
                    CitiesToId = table.Column<int>(nullable: false),
                    DepTime = table.Column<DateTime>(nullable: false),
                    ArrivalTime = table.Column<DateTime>(nullable: false),
                    DepDate = table.Column<DateTime>(nullable: false),
                    ArrivalDate = table.Column<DateTime>(nullable: false),
                    Price = table.Column<int>(nullable: false),
                    AvailableSeats = table.Column<int>(nullable: false),
                    CompanyAssetId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Trips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Trips_Cities_CitiesFromId",
                        column: x => x.CitiesFromId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Trips_Cities_CitiesToId",
                        column: x => x.CitiesToId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Trips_CompanyAssets_CompanyAssetId",
                        column: x => x.CompanyAssetId,
                        principalTable: "CompanyAssets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CitiesFromId",
                table: "Trips",
                column: "CitiesFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CitiesToId",
                table: "Trips",
                column: "CitiesToId");

            migrationBuilder.CreateIndex(
                name: "IX_Trips_CompanyAssetId",
                table: "Trips",
                column: "CompanyAssetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Trips");
        }
    }
}
