using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eaf.Str.Migrations
{
    /// <inheritdoc />
    public partial class AwbTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AwbAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ZipCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: false),
                    Neighborhood = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    City = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    State = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: false),
                    Complement = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Observation = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    PersonName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
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
                    table.PrimaryKey("PK_AwbAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Awb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TrackingNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RecipientId = table.Column<int>(type: "int", nullable: false),
                    SenderId = table.Column<int>(type: "int", nullable: false),
                    Origin = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Destiny = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ReceivedName = table.Column<string>(type: "nvarchar(512)", maxLength: 512, nullable: true),
                    ReceivedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ReceivedDocument = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
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
                    table.PrimaryKey("PK_Awb", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Awb_AwbAddress_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "AwbAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Awb_AwbAddress_SenderId",
                        column: x => x.SenderId,
                        principalTable: "AwbAddress",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AwbItens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Weight = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Quantities = table.Column<int>(type: "int", nullable: false),
                    Invoice = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PackagingType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    MaterialType = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: true),
                    AwbId = table.Column<int>(type: "int", nullable: true),
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
                    table.PrimaryKey("PK_AwbItens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AwbItens_Awb_AwbId",
                        column: x => x.AwbId,
                        principalTable: "Awb",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Awb_RecipientId",
                table: "Awb",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Awb_SenderId",
                table: "Awb",
                column: "SenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Awb_TrackingNumber",
                table: "Awb",
                column: "TrackingNumber");

            migrationBuilder.CreateIndex(
                name: "IX_AwbAddress_ZipCode",
                table: "AwbAddress",
                column: "ZipCode");

            migrationBuilder.CreateIndex(
                name: "IX_AwbItens_AwbId",
                table: "AwbItens",
                column: "AwbId");

            migrationBuilder.CreateIndex(
                name: "IX_AwbItens_Invoice",
                table: "AwbItens",
                column: "Invoice");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AwbItens");

            migrationBuilder.DropTable(
                name: "Awb");

            migrationBuilder.DropTable(
                name: "AwbAddress");
        }
    }
}
