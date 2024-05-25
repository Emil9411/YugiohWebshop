using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Yugioh.Server.Migrations
{
    /// <inheritdoc />
    public partial class AttributeMoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Attribute",
                table: "SpellAndTrapCards",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Attribute",
                table: "SpellAndTrapCards");
        }
    }
}
