using Microsoft.EntityFrameworkCore.Migrations;

namespace DungeonsAndDragons.Migrations
{
    public partial class GameClass : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_users_dmid",
                table: "games");

            migrationBuilder.RenameColumn(
                name: "dmid",
                table: "games",
                newName: "dm_id");

            migrationBuilder.RenameIndex(
                name: "IX_games_dmid",
                table: "games",
                newName: "IX_games_dm_id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_users_dm_id",
                table: "games",
                column: "dm_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_users_dm_id",
                table: "games");

            migrationBuilder.RenameColumn(
                name: "dm_id",
                table: "games",
                newName: "dmid");

            migrationBuilder.RenameIndex(
                name: "IX_games_dm_id",
                table: "games",
                newName: "IX_games_dmid");

            migrationBuilder.AddForeignKey(
                name: "FK_games_users_dmid",
                table: "games",
                column: "dmid",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
