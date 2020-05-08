using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiProject.Migrations
{
    public partial class addCompanyAssetsToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CompanyAssets",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompniesId = table.Column<int>(nullable: false),
                    TransportationCategoriesId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CompanyAssets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CompanyAssets_Companies_CompniesId",
                        column: x => x.CompniesId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CompanyAssets_TransportationCategories_TransportationCategoriesId",
                        column: x => x.TransportationCategoriesId,
                        principalTable: "TransportationCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAssets_CompniesId",
                table: "CompanyAssets",
                column: "CompniesId");

            migrationBuilder.CreateIndex(
                name: "IX_CompanyAssets_TransportationCategoriesId",
                table: "CompanyAssets",
                column: "TransportationCategoriesId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CompanyAssets");
        }
    }
}
