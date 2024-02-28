using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class Aggregates : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(
          name: "AbbDailyEnergyRanges");

      migrationBuilder.DropTable(
          name: "AbbMonthlyEnergyRanges");

      migrationBuilder.DropTable(
          name: "AbbQuarterHourlyEnergyRanges");

      migrationBuilder.DropTable(
          name: "SchneiderDailyEnergyRanges");

      migrationBuilder.DropTable(
          name: "SchneiderMonthlyEnergyRanges");

      migrationBuilder.DropTable(
          name: "SchneiderQuarterHourlyEnergyRanges");

      migrationBuilder.CreateTable(
          name: "AbbDailyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL1Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL2Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL3Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbDailyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "AbbMonthlyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL1Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL2Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL3Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbMonthlyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "AbbQuarterHourlyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL1Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL2Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerL3Avg_VAR = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbQuarterHourlyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderDailyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerTotalAvg_VAR = table.Column<double>(type: "float8", nullable: false),
            ApparentPowerTotalAvg_VA = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderDailyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderMonthlyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerTotalAvg_VAR = table.Column<double>(type: "float8", nullable: false),
            ApparentPowerTotalAvg_VA = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderMonthlyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderQuarterHourlyAggregates",
          columns: table => new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            VoltageL1Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL2Avg_V = table.Column<double>(type: "float8", nullable: false),
            VoltageL3Avg_V = table.Column<double>(type: "float8", nullable: false),
            CurrentL1Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL2Avg_A = table.Column<double>(type: "float8", nullable: false),
            CurrentL3Avg_A = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL1Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL2Avg_W = table.Column<double>(type: "float8", nullable: false),
            ActivePowerL3Avg_W = table.Column<double>(type: "float8", nullable: false),
            ReactivePowerTotalAvg_VAR = table.Column<double>(type: "float8", nullable: false),
            ApparentPowerTotalAvg_VA = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            AggregateCount = table.Column<short>(type: "int8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderQuarterHourlyAggregates", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateIndex(
          name: "IX_AbbDailyAggregates_Timestamp_Source_Tenant",
          table: "AbbDailyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbMonthlyAggregates_Timestamp_Source_Tenant",
          table: "AbbMonthlyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbQuarterHourlyAggregates_Timestamp_Source_Tenant",
          table: "AbbQuarterHourlyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderDailyAggregates_Timestamp_Source_Tenant",
          table: "SchneiderDailyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderMonthlyAggregates_Timestamp_Source_Tenant",
          table: "SchneiderMonthlyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderQuarterHourlyAggregates_Timestamp_Source_Tenant",
          table: "SchneiderQuarterHourlyAggregates",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.Sql("""
        insert into "SchneiderQuarterHourlyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerTotalAvg_VAR",
            "ApparentPowerTotalAvg_VA",
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
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerTotal_VAR") as "ReactivePowerTotalAvg_VAR",
            avg("ApparentPowerTotal_VA") as "ApparentPowerTotalAvg_VA",
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
            time_bucket('15 minutes', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "AbbQuarterHourlyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerL1Avg_VAR",
            "ReactivePowerL2Avg_VAR",
            "ReactivePowerL3Avg_VAR",
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
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerL1_VAR") as "ReactivePowerL1Avg_VAR",
            avg("ReactivePowerL2_VAR") as "ReactivePowerL2Avg_VAR",
            avg("ReactivePowerL3_VAR") as "ReactivePowerL3Avg_VAR",
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
            time_bucket('15 minutes', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "SchneiderMonthlyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerTotalAvg_VAR",
            "ApparentPowerTotalAvg_VA",
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
            date_trunc('month', "Timestamp" at time zone 'UTC') at time zone 'UTC' as "Timestamp",
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerTotal_VAR") as "ReactivePowerTotalAvg_VAR",
            avg("ApparentPowerTotal_VA") as "ApparentPowerTotalAvg_VA",
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
            date_trunc('month', "Timestamp" at time zone 'UTC') at time zone 'UTC',
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "AbbMonthlyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerL1Avg_VAR",
            "ReactivePowerL2Avg_VAR",
            "ReactivePowerL3Avg_VAR",
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
            date_trunc('month', "Timestamp" at time zone 'UTC') at time zone 'UTC' as "Timestamp",
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerL1_VAR") as "ReactivePowerL1Avg_VAR",
            avg("ReactivePowerL2_VAR") as "ReactivePowerL2Avg_VAR",
            avg("ReactivePowerL3_VAR") as "ReactivePowerL3Avg_VAR",
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
            date_trunc('month', "Timestamp" at time zone 'UTC') at time zone 'UTC',
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "SchneiderDailyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerTotalAvg_VAR",
            "ApparentPowerTotalAvg_VA",
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
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerTotal_VAR") as "ReactivePowerTotalAvg_VAR",
            avg("ApparentPowerTotal_VA") as "ApparentPowerTotalAvg_VA",
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
        insert into "AbbDailyAggregates" (
            "Tenant",
            "Source",
            "Timestamp",
            "AggregateCount",
            "VoltageL1Avg_V",
            "VoltageL2Avg_V",
            "VoltageL3Avg_V",
            "CurrentL1Avg_A",
            "CurrentL2Avg_A",
            "CurrentL3Avg_A",
            "ActivePowerL1Avg_W",
            "ActivePowerL2Avg_W",
            "ActivePowerL3Avg_W",
            "ReactivePowerL1Avg_VAR",
            "ReactivePowerL2Avg_VAR",
            "ReactivePowerL3Avg_VAR",
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
            count(*) as "AggregateCount",
            avg("VoltageL1_V") as "VoltageL1Avg_V",
            avg("VoltageL2_V") as "VoltageL2Avg_V",
            avg("VoltageL3_V") as "VoltageL3Avg_V",
            avg("CurrentL1_A") as "CurrentL1Avg_A",
            avg("CurrentL2_A") as "CurrentL2Avg_A",
            avg("CurrentL3_A") as "CurrentL3Avg_A",
            avg("ActivePowerL1_W") as "ActivePowerL1Avg_W",
            avg("ActivePowerL2_W") as "ActivePowerL2Avg_W",
            avg("ActivePowerL3_W") as "ActivePowerL3Avg_W",
            avg("ReactivePowerL1_VAR") as "ReactivePowerL1Avg_VAR",
            avg("ReactivePowerL2_VAR") as "ReactivePowerL2Avg_VAR",
            avg("ReactivePowerL3_VAR") as "ReactivePowerL3Avg_VAR",
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
          name: "AbbDailyAggregates");

      migrationBuilder.DropTable(
          name: "AbbMonthlyAggregates");

      migrationBuilder.DropTable(
          name: "AbbQuarterHourlyAggregates");

      migrationBuilder.DropTable(
          name: "SchneiderDailyAggregates");

      migrationBuilder.DropTable(
          name: "SchneiderMonthlyAggregates");

      migrationBuilder.DropTable(
          name: "SchneiderQuarterHourlyAggregates");

      migrationBuilder.CreateTable(
          name: "AbbDailyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbDailyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "AbbMonthlyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbMonthlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "AbbQuarterHourlyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_AbbQuarterHourlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderDailyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderDailyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderMonthlyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderMonthlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateTable(
          name: "SchneiderQuarterHourlyEnergyRanges",
          columns: table => new
          {
            Timestamp = table.Column<DateTimeOffset>(type: "timestamptz", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Tenant = table.Column<string>(type: "text", nullable: false),
            ActiveEnergyExportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyExportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMax_Wh = table.Column<double>(type: "float8", nullable: false),
            ActiveEnergyImportTotalMin_Wh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyExportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMax_VARh = table.Column<double>(type: "float8", nullable: false),
            ReactiveEnergyImportTotalMin_VARh = table.Column<double>(type: "float8", nullable: false)
          },
          constraints: table =>
          {
            table.PrimaryKey("PK_SchneiderQuarterHourlyEnergyRanges", x => new { x.Timestamp, x.Source, x.Tenant });
          });

      migrationBuilder.CreateIndex(
          name: "IX_AbbDailyEnergyRanges_Timestamp_Source_Tenant",
          table: "AbbDailyEnergyRanges",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbMonthlyEnergyRanges_Timestamp_Source_Tenant",
          table: "AbbMonthlyEnergyRanges",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbQuarterHourlyEnergyRanges_Timestamp_Source_Tenant",
          table: "AbbQuarterHourlyEnergyRanges",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderDailyEnergyRanges_Timestamp_Source_Tenant",
          table: "SchneiderDailyEnergyRanges",
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
  }
}
