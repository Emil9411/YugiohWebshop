using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yugioh.Server.Migrations
{
    /// <inheritdoc />
    public partial class InventoryColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "SpellAndTrapCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Inventory",
                table: "MonsterCards",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "SpellAndTrapCards");

            migrationBuilder.DropColumn(
                name: "Inventory",
                table: "MonsterCards");
        }
    }
}
