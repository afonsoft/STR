using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eaf.Str.Migrations
{
    /// <inheritdoc />
    public partial class AirportsTableIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Airports_IATACode",
                table: "Airports",
                column: "IATACode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Airports_IATACode",
                table: "Airports");
        }
    }
}
