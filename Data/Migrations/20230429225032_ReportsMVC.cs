using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class ReportsMVC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ReportId",
                table: "SetRatingv3",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Report",
                columns: table => new
                {
                    ReportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ViewCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Report", x => x.ReportId);
                    table.ForeignKey(
                        name: "FK_Report_FlashcardSet_SetId",
                        column: x => x.SetId,
                        principalTable: "FlashcardSet",
                        principalColumn: "SetId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Report_ViewCount_ViewCountId",
                        column: x => x.ViewCountId,
                        principalTable: "ViewCount",
                        principalColumn: "ViewCountId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetRatingv3_ReportId",
                table: "SetRatingv3",
                column: "ReportId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_SetId",
                table: "Report",
                column: "SetId");

            migrationBuilder.CreateIndex(
                name: "IX_Report_ViewCountId",
                table: "Report",
                column: "ViewCountId");

            migrationBuilder.AddForeignKey(
                name: "FK_SetRatingv3_Report_ReportId",
                table: "SetRatingv3",
                column: "ReportId",
                principalTable: "Report",
                principalColumn: "ReportId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SetRatingv3_Report_ReportId",
                table: "SetRatingv3");

            migrationBuilder.DropTable(
                name: "Report");

            migrationBuilder.DropIndex(
                name: "IX_SetRatingv3_ReportId",
                table: "SetRatingv3");

            migrationBuilder.DropColumn(
                name: "ReportId",
                table: "SetRatingv3");
        }
    }
}
