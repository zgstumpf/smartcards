using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class flashcardsMVC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlashcardSet",
                columns: table => new
                {
                    SetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Category = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlashcardSet", x => x.SetId);
                    table.ForeignKey(
                        name: "FK_FlashcardSet_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Flashcard",
                columns: table => new
                {
                    FlashcardId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Front = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Back = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FlashcardSetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Flashcard", x => x.FlashcardId);
                    table.ForeignKey(
                        name: "FK_Flashcard_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Flashcard_FlashcardSet_FlashcardSetId",
                        column: x => x.FlashcardSetId,
                        principalTable: "FlashcardSet",
                        principalColumn: "SetId");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_FlashcardSetId",
                table: "Flashcard",
                column: "FlashcardSetId");

            migrationBuilder.CreateIndex(
                name: "IX_Flashcard_UserId",
                table: "Flashcard",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_FlashcardSet_UserId",
                table: "FlashcardSet",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Flashcard");

            migrationBuilder.DropTable(
                name: "FlashcardSet");
        }
    }
}
