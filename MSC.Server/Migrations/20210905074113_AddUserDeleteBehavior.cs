using Microsoft.EntityFrameworkCore.Migrations;

namespace MSC.Server.Migrations
{
    public partial class AddUserDeleteBehavior : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_AspNetUsers_UserId",
                table: "Ranks");

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_AspNetUsers_UserId",
                table: "Ranks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_AspNetUsers_UserId",
                table: "Ranks");

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_AspNetUsers_UserId",
                table: "Ranks",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
