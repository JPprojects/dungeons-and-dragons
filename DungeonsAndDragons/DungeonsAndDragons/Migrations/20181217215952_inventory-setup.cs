using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace DungeonsAndDragons.Migrations
{
    public partial class inventorysetup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "inventory",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    chracterid = table.Column<int>(nullable: false),
                    inventoryItemId = table.Column<int>(nullable: false),
                    quantity = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventory", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "inventoryitems",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn),
                    itemName = table.Column<string>(nullable: true),
                    itemType = table.Column<string>(nullable: true),
                    imagePath = table.Column<string>(nullable: true),
                    healingFactor = table.Column<int>(nullable: false),
                    attack = table.Column<int>(nullable: false),
                    monetaryValue = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_inventoryitems", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "inventory");

            migrationBuilder.DropTable(
                name: "inventoryitems");
        }
    }
}
