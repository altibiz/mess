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

      migrationBuilder.Sql("""
        insert into "SchneiderDailyEnergyRanges" (
            "Tenant",
            "Source",
            "Timestamp",
            "ActiveEnergyImportTotalMin_Wh",
            "ActiveEnergyImportTotalMax_Wh",
            "ActiveEnergyExportTotalMin_Wh",
            "ActiveEnergyExportTotalMax_Wh",
            "ReactiveEnergyImportTotalMin_VARh",
            "ReactiveEnergyImportTotalMax_VARh",
            "ReactiveEnergyExportTotalMin_VARh",
            "ReactiveEnergyExportTotalMax_VARh"
        )
        select
            "Tenant",
            "Source",
            time_bucket('1 day', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh",
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh",
            min("ReactiveEnergyImportTotal_VARh") as "ReactiveEnergyImportTotalMin_VARh",
            max("ReactiveEnergyImportTotal_VARh") as "ReactiveEnergyImportTotalMax_VARh",
            min("ReactiveEnergyExportTotal_VARh") as "ReactiveEnergyExportTotalMin_VARh",
            max("ReactiveEnergyExportTotal_VARh") as "ReactiveEnergyExportTotalMax_VARh"
        from
            "SchneiderMeasurements"
        group by
            time_bucket('1 day', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "AbbDailyEnergyRanges" (
            "Tenant",
            "Source",
            "Timestamp",
            "ActiveEnergyImportTotalMin_Wh",
            "ActiveEnergyImportTotalMax_Wh",
            "ActiveEnergyExportTotalMin_Wh",
            "ActiveEnergyExportTotalMax_Wh",
            "ReactiveEnergyImportTotalMin_VARh",
            "ReactiveEnergyImportTotalMax_VARh",
            "ReactiveEnergyExportTotalMin_VARh",
            "ReactiveEnergyExportTotalMax_VARh"
        )
        select
            "Tenant",
            "Source",
            time_bucket('1 day', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh",
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh",
            min("ReactiveEnergyImportTotal_VARh") as "ReactiveEnergyImportTotalMin_VARh",
            max("ReactiveEnergyImportTotal_VARh") as "ReactiveEnergyImportTotalMax_VARh",
            min("ReactiveEnergyExportTotal_VARh") as "ReactiveEnergyExportTotalMin_VARh",
            max("ReactiveEnergyExportTotal_VARh") as "ReactiveEnergyExportTotalMax_VARh"
        from
            "AbbMeasurements"
        group by
            time_bucket('1 day', "Timestamp"),
            "Source",
            "Tenant";
      """);
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
