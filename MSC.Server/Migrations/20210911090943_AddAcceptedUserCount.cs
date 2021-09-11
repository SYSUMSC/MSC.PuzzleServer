using Microsoft.EntityFrameworkCore.Migrations;

namespace MSC.Server.Migrations
{
    public partial class AddAcceptedUserCount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AcceptedUserCount",
                table: "Puzzles",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AcceptedUserCount",
                table: "Puzzles");
        }
    }
}
