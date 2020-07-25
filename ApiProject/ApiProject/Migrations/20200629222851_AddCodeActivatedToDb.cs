using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ApiProject.Migrations
{
    public partial class AddCodeActivatedToDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_CompanyAssets_CompanyId",
                table: "PromoCodes");

            migrationBuilder.CreateTable(
                name: "CodeActivated",
                columns: table => new
                {
                    CodeActivatedId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromoCodeId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(maxLength: 450, nullable: true),
                    DateActivated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CodeActivated", x => x.CodeActivatedId);
                    table.ForeignKey(
                        name: "FK_CodeActivated_PromoCodes_PromoCodeId",
                        column: x => x.PromoCodeId,
                        principalTable: "PromoCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CodeActivated_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CodeActivated_PromoCodeId",
                table: "CodeActivated",
                column: "PromoCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_CodeActivated_UserId",
                table: "CodeActivated",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_Companies_CompanyId",
                table: "PromoCodes",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PromoCodes_Companies_CompanyId",
                table: "PromoCodes");

            migrationBuilder.DropTable(
                name: "CodeActivated");

            migrationBuilder.AddForeignKey(
                name: "FK_PromoCodes_CompanyAssets_CompanyId",
                table: "PromoCodes",
                column: "CompanyId",
                principalTable: "CompanyAssets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
