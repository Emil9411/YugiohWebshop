using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yugioh.Server.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedBaseClass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpellAndTrapCards_Name",
                table: "SpellAndTrapCards");

            migrationBuilder.DropIndex(
                name: "IX_MonsterCards_Name",
                table: "MonsterCards");

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "SpellAndTrapCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CardId",
                table: "MonsterCards",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_SpellAndTrapCards_Name_CardId",
                table: "SpellAndTrapCards",
                columns: new[] { "Name", "CardId" },
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterCards_Name_CardId",
                table: "MonsterCards",
                columns: new[] { "Name", "CardId" },
                unique: true,
                filter: "[Name] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SpellAndTrapCards_Name_CardId",
                table: "SpellAndTrapCards");

            migrationBuilder.DropIndex(
                name: "IX_MonsterCards_Name_CardId",
                table: "MonsterCards");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "SpellAndTrapCards");

            migrationBuilder.DropColumn(
                name: "CardId",
                table: "MonsterCards");

            migrationBuilder.CreateIndex(
                name: "IX_SpellAndTrapCards_Name",
                table: "SpellAndTrapCards",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_MonsterCards_Name",
                table: "MonsterCards",
                column: "Name",
                unique: true,
                filter: "[Name] IS NOT NULL");
        }
    }
}
