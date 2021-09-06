using Microsoft.EntityFrameworkCore.Migrations;

namespace MSC.Server.Migrations
{
    public partial class AddSubmissionCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SolvedCount",
                table: "Puzzles",
                newName: "SubmissionCount");

            migrationBuilder.AddColumn<int>(
                name: "AcceptedCount",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedCount",
                table: "Puzzles");

            migrationBuilder.RenameColumn(
                name: "SubmissionCount",
                table: "Puzzles",
                newName: "SolvedCount");
        }
    }
}
