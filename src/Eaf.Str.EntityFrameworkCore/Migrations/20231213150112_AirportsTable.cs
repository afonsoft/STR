using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eaf.Str.Migrations
{
    /// <inheritdoc />
    public partial class AirportsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Airports",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IATACode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    NamePT = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    NameES = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    NameEN = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ShortNamePT = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    ShortNameES = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ShortNameEN = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ICAOCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CountryCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    TimeZone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Latitude = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Longitude = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorUserId = table.Column<long>(type: "bigint", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeleterUserId = table.Column<long>(type: "bigint", nullable: true),
                    DeletionTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Airports", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Airports");
        }
    }
}
