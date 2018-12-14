using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DungeonsAndDragons.Migrations
{
    public partial class AddSpeciesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "species_id",
                table: "playablecharacters",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<int>(
                name: "playablecharacterid",
                table: "gamesusers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.CreateTable(
                name: "species",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    species_type = table.Column<string>(type: "varchar(20)", nullable: true),
                    image_path = table.Column<string>(type: "varchar(100)", nullable: true),
                    base_hp = table.Column<int>(nullable: false),
                    base_attack = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_species", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "species");

            migrationBuilder.DropColumn(
                name: "species_id",
                table: "playablecharacters");

            migrationBuilder.AlterColumn<int>(
                name: "playablecharacterid",
                table: "gamesusers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
