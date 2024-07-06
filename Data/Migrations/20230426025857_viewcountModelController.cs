using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class viewcountModelController : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ViewCount",
                columns: table => new
                {
                    ViewCountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Count = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ViewCount", x => x.ViewCountId);
                    table.ForeignKey(
                        name: "FK_ViewCount_FlashcardSet_SetId",
                        column: x => x.SetId,
                        principalTable: "FlashcardSet",
                        principalColumn: "SetId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ViewCount_SetId",
                table: "ViewCount",
                column: "SetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ViewCount");
        }
    }
}
