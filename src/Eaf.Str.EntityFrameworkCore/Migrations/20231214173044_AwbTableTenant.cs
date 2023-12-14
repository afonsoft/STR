using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eaf.Str.Migrations
{
    /// <inheritdoc />
    public partial class AwbTableTenant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AwbItens",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "AwbAddress",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Awb",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AwbItens");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "AwbAddress");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Awb");
        }
    }
}
