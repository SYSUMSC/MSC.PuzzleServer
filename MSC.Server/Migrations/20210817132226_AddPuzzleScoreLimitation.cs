using Microsoft.EntityFrameworkCore.Migrations;

namespace MSC.Server.Migrations
{
    public partial class AddPuzzleScoreLimitation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AwardCount",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ExpectMaxCount",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AwardCount",
                table: "Puzzles");

            migrationBuilder.DropColumn(
                name: "ExpectMaxCount",
                table: "Puzzles");
        }
    }
}