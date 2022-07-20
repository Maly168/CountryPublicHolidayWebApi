using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CountryPublicHolidayWebApi.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SupportedCountry",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CountryName = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    CountryCode = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    HolidayTypes = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    FromDate = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    ToDate = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportedCountry", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Region",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    SupportedCountryId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Region", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Region_SupportedCountry_SupportedCountryId",
                        column: x => x.SupportedCountryId,
                        principalTable: "SupportedCountry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Holiday",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SearchCountry = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    SearchYear = table.Column<int>(type: "INT", maxLength: 50, nullable: false),
                    Name = table.Column<string>(type: "NVARCHAR(500)", maxLength: 500, nullable: true),
                    Type = table.Column<string>(type: "NVARCHAR(100)", maxLength: 100, nullable: true),
                    DayOfWeek = table.Column<int>(type: "INT", maxLength: 10, nullable: false),
                    Date = table.Column<string>(type: "NVARCHAR(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "NVARCHAR(200)", maxLength: 200, nullable: true),
                    RegionId = table.Column<int>(type: "int", nullable: true),
                    SupportedCountryId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Holiday", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Holiday_Region_RegionId",
                        column: x => x.RegionId,
                        principalTable: "Region",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Holiday_SupportedCountry_SupportedCountryId",
                        column: x => x.SupportedCountryId,
                        principalTable: "SupportedCountry",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_RegionId",
                table: "Holiday",
                column: "RegionId",
                unique: true,
                filter: "[RegionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Holiday_SupportedCountryId",
                table: "Holiday",
                column: "SupportedCountryId",
                unique: true,
                filter: "[SupportedCountryId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Region_SupportedCountryId",
                table: "Region",
                column: "SupportedCountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Holiday");

            migrationBuilder.DropTable(
                name: "Region");

            migrationBuilder.DropTable(
                name: "SupportedCountry");
        }
    }
}
