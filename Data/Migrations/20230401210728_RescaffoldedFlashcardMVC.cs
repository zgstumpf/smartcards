using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class RescaffoldedFlashcardMVC : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcard_FlashcardSet_FlashcardSetId",
                table: "Flashcard");

            migrationBuilder.RenameColumn(
                name: "FlashcardSetId",
                table: "Flashcard",
                newName: "SetId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcard_FlashcardSetId",
                table: "Flashcard",
                newName: "IX_Flashcard_SetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcard_FlashcardSet_SetId",
                table: "Flashcard",
                column: "SetId",
                principalTable: "FlashcardSet",
                principalColumn: "SetId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Flashcard_FlashcardSet_SetId",
                table: "Flashcard");

            migrationBuilder.RenameColumn(
                name: "SetId",
                table: "Flashcard",
                newName: "FlashcardSetId");

            migrationBuilder.RenameIndex(
                name: "IX_Flashcard_SetId",
                table: "Flashcard",
                newName: "IX_Flashcard_FlashcardSetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Flashcard_FlashcardSet_FlashcardSetId",
                table: "Flashcard",
                column: "FlashcardSetId",
                principalTable: "FlashcardSet",
                principalColumn: "SetId");
        }
    }
}
