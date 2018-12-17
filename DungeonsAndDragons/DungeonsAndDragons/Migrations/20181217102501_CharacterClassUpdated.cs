using Microsoft.EntityFrameworkCore.Migrations;

namespace DungeonsAndDragons.Migrations
{
    public partial class CharacterClassUpdated : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "hp",
                table: "playablecharacters",
                newName: "maxHp");

            migrationBuilder.RenameColumn(
                name: "hp",
                table: "nonplayablecharacters",
                newName: "maxHp");

            migrationBuilder.AddColumn<int>(
                name: "currentHp",
                table: "playablecharacters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "imagePath",
                table: "playablecharacters",
                type: "varchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "currentHp",
                table: "nonplayablecharacters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "imagePath",
                table: "nonplayablecharacters",
                type: "varchar(100)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "currentHp",
                table: "playablecharacters");

            migrationBuilder.DropColumn(
                name: "imagePath",
                table: "playablecharacters");

            migrationBuilder.DropColumn(
                name: "currentHp",
                table: "nonplayablecharacters");

            migrationBuilder.DropColumn(
                name: "imagePath",
                table: "nonplayablecharacters");

            migrationBuilder.RenameColumn(
                name: "maxHp",
                table: "playablecharacters",
                newName: "hp");

            migrationBuilder.RenameColumn(
                name: "maxHp",
                table: "nonplayablecharacters",
                newName: "hp");
        }
    }
}
