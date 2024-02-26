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

      migrationBuilder.Sql("""
        SELECT create_hypertable('"AbbMonthlyEnergyRanges"', 'Timestamp');
      """);

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

      migrationBuilder.Sql("""
        SELECT create_hypertable('"AbbQuarterHourlyEnergyRanges"', 'Timestamp');
      """);

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

      migrationBuilder.Sql("""
        SELECT create_hypertable('"SchneiderMonthlyEnergyRanges"', 'Timestamp');
      """);

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

      migrationBuilder.Sql("""
        SELECT create_hypertable('"SchneiderQuarterHourlyEnergyRanges"', 'Timestamp');
      """);

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

      migrationBuilder.Sql("""
        drop materialized view "MonthlyRangeEnergy";
      """);

      migrationBuilder.Sql("""
        drop materialized view "QuarterHourAveragePower";
      """);

      migrationBuilder.Sql("""
        insert into "SchneiderQuarterHourlyEnergyRanges" (
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
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
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
        insert into "AbbQuarterHourlyEnergyRanges" (
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
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
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
        insert into "SchneiderMonthlyEnergyRanges" (
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
            time_bucket('1 month', "Timestamp") as "Timestamp",
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
            time_bucket('1 month', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        insert into "AbbMonthlyEnergyRanges" (
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
            time_bucket('1 month', "Timestamp") as "Timestamp",
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
            time_bucket('1 month', "Timestamp"),
            "Source",
            "Tenant";
      """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql("""
        create materialized view if not exists "MonthlyRangeEnergy"
        as
          with
            measurements as (
              select
                "Tenant" as tenant,
                "Source" as source,
                "Timestamp" as timestamp,
                "ActiveEnergyImportTotal_Wh" as energy
              from
                "AbbMeasurements"
              union
              select
                "Tenant" as tenant,
                "Source" as source,
                "Timestamp" as timestamp,
                "ActiveEnergyImportTotal_Wh" as energy
              from
                "SchneiderMeasurements"
            ),
            ranked as (
              select
                tenant,
                source,
                timestamp,
                energy,
                row_number() over (
                  partition by
                    tenant,
                    source,
                    time_bucket('1 month', timestamp)
                  order by
                    timestamp asc
                ) as timestamp_ascending,
                row_number() over (
                  partition by
                    tenant,
                    source,
                    time_bucket('1 month', timestamp)
                  order by
                    timestamp desc
                ) as timestamp_descending
              from
                measurements
            )
          select
            tenant as "Tenant",
            source as "Source",
            timestamp as "Timestamp",
            energy as "ActiveEnergyImportTotal_Wh"
          from
            ranked
          where
            timestamp_ascending = 1
            or timestamp_descending = 1
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "MonthlyRangeEnergy" (
          "Tenant",
          "Source",
          "Timestamp"
        );
      """);

      migrationBuilder.Sql("""
        create materialized view if not exists "QuarterHourAveragePower"
        as
          with
            measurements as (
              select
                "Tenant" as tenant,
                "Source" as source,
                "Timestamp" as timestamp,
                "ActiveEnergyImportTotal_Wh" as energy
              from
                "AbbMeasurements"
              union
              select
                "Tenant" as tenant,
                "Source" as source,
                "Timestamp" as timestamp,
                "ActiveEnergyImportTotal_Wh" as energy
              from
                "SchneiderMeasurements"
            ),
            buckets as (
              select
                distinct on (
                  tenant,
                  source,
                  time_bucket('15 minutes', timestamp)
                )
                tenant,
                source,
                time_bucket('15 minutes', timestamp) as timestamp,
                first_value(energy) over bucket_windows as begin_energy,
                last_value(energy) over bucket_windows as end_energy
              from
                measurements
              window bucket_windows as (
                partition by tenant, source, time_bucket('15 minutes', timestamp)
                order by timestamp asc
                range between unbounded preceding and unbounded following
              )
            ),
            calculation as (
              select
                tenant,
                source,
                timestamp,
                (end_energy - begin_energy) * 4 as power
              from
                buckets
            ),
            sum as (
              select
                tenant,
                source,
                timestamp,
                sum(power) as power
              from
                calculation
              group by
                tenant,
                source,
                timestamp
            )
          select
            tenant as "Tenant",
            source as "Source",
            timestamp as "Timestamp",
            power as "ActivePower_W"
          from
            sum
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "QuarterHourAveragePower" (
          "Tenant",
          "Source",
          "Timestamp"
        );
      """);

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
