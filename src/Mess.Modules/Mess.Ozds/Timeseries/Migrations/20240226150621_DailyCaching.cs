using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class DailyCaching : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
          name: "AbbDailyEnergyRanges",
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
            table.PrimaryKey("PK_AbbDailyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.Sql("""
        SELECT create_hypertable('"AbbDailyEnergyRanges"', 'Timestamp');
      """);

      migrationBuilder.CreateTable(
          name: "SchneiderDailyEnergyRanges",
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
            table.PrimaryKey("PK_SchneiderDailyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.Sql("""
        SELECT create_hypertable('"SchneiderDailyEnergyRanges"', 'Timestamp');
      """);

      migrationBuilder.CreateIndex(
          name: "IX_AbbDailyEnergyRanges_Timestamp_Source_Tenant",
          table: "AbbDailyEnergyRanges",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderDailyEnergyRanges_Timestamp_Source_Tenant",
          table: "SchneiderDailyEnergyRanges",
          columns: new[] { "Timestamp", "Source", "Tenant" });
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "AbbDailyEnergyRanges");

      migrationBuilder.DropTable(
          name: "SchneiderDailyEnergyRanges");
    }
  }
}
