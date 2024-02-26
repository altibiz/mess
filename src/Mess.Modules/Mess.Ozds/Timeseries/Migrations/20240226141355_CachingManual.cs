using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
    /// <inheritdoc />
    public partial class CachingManual : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AbbMonthlyEnergyRanges",
                columns: table => new
                {
                    Tenant = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbbMonthlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
                });

            migrationBuilder.CreateTable(
                name: "AbbQuarterHourlyEnergyRanges",
                columns: table => new
                {
                    Tenant = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AbbQuarterHourlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
                });

            migrationBuilder.CreateTable(
                name: "SchneiderMonthlyEnergyRanges",
                columns: table => new
                {
                    Tenant = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchneiderMonthlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
                });

            migrationBuilder.CreateTable(
                name: "SchneiderQuarterHourlyEnergyRanges",
                columns: table => new
                {
                    Tenant = table.Column<string>(type: "text", nullable: false),
                    Source = table.Column<string>(type: "text", nullable: false),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
                    ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
                    ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
                    ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SchneiderQuarterHourlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
                });

            migrationBuilder.CreateIndex(
                name: "IX_AbbMonthlyEnergyRanges_Timestamp_Source_Tenant",
                table: "AbbMonthlyEnergyRanges",
                columns: new[] { "Timestamp", "Source", "Tenant" });

            migrationBuilder.CreateIndex(
                name: "IX_AbbQuarterHourlyEnergyRanges_Timestamp_Source_Tenant",
                table: "AbbQuarterHourlyEnergyRanges",
                columns: new[] { "Timestamp", "Source", "Tenant" });

            migrationBuilder.CreateIndex(
                name: "IX_SchneiderMonthlyEnergyRanges_Timestamp_Source_Tenant",
                table: "SchneiderMonthlyEnergyRanges",
                columns: new[] { "Timestamp", "Source", "Tenant" });

            migrationBuilder.CreateIndex(
                name: "IX_SchneiderQuarterHourlyEnergyRanges_Timestamp_Source_Tenant",
                table: "SchneiderQuarterHourlyEnergyRanges",
                columns: new[] { "Timestamp", "Source", "Tenant" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AbbMonthlyEnergyRanges");

            migrationBuilder.DropTable(
                name: "AbbQuarterHourlyEnergyRanges");

            migrationBuilder.DropTable(
                name: "SchneiderMonthlyEnergyRanges");

            migrationBuilder.DropTable(
                name: "SchneiderQuarterHourlyEnergyRanges");
        }
    }
}
