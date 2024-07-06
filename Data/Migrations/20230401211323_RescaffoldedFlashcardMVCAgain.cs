using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace assignment4.Data.Migrations
{
    public partial class RescaffoldedFlashcardMVCAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Flashcard",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Flashcard");
        }
    }
}
