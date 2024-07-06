using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class setratingv3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SetRatingv3",
                columns: table => new
                {
                    SetRatingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RatingUserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    RatingUserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RatedSetName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SetRatingv3", x => x.SetRatingId);
                    table.ForeignKey(
                        name: "FK_SetRatingv3_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SetRatingv3_FlashcardSet_RatedSetId",
                        column: x => x.RatedSetId,
                        principalTable: "FlashcardSet",
                        principalColumn: "SetId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SetRatingv3_RatedSetId",
                table: "SetRatingv3",
                column: "RatedSetId");

            migrationBuilder.CreateIndex(
                name: "IX_SetRatingv3_UserId",
                table: "SetRatingv3",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SetRatingv3");
        }
    }
}
