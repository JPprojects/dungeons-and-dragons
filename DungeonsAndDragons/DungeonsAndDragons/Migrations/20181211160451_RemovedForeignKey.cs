using Microsoft.EntityFrameworkCore.Migrations;

namespace DungeonsAndDragons.Migrations
{
    public partial class RemovedForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_games_users_dm_id",
                table: "games");

            migrationBuilder.DropIndex(
                name: "IX_games_dm_id",
                table: "games");

            migrationBuilder.DropColumn(
                name: "dm_id",
                table: "games");

            migrationBuilder.AddColumn<long>(
                name: "dm",
                table: "games",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "dm",
                table: "games");

            migrationBuilder.AddColumn<long>(
                name: "dm_id",
                table: "games",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_games_dm_id",
                table: "games",
                column: "dm_id");

            migrationBuilder.AddForeignKey(
                name: "FK_games_users_dm_id",
                table: "games",
                column: "dm_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
