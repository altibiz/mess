using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class CachingV2 : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropPrimaryKey(
          name: "PK_SchneiderMeasurements",
          table: "SchneiderMeasurements");

      migrationBuilder.DropIndex(
          name: "IX_SchneiderMeasurements_Tenant_Source_Timestamp",
          table: "SchneiderMeasurements");

      migrationBuilder.DropPrimaryKey(
          name: "PK_AbbMeasurements",
          table: "AbbMeasurements");

      migrationBuilder.DropIndex(
          name: "IX_AbbMeasurements_Tenant_Source_Timestamp",
          table: "AbbMeasurements");

      migrationBuilder.AddPrimaryKey(
          name: "PK_SchneiderMeasurements",
          table: "SchneiderMeasurements",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.AddPrimaryKey(
          name: "PK_AbbMeasurements",
          table: "AbbMeasurements",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderMeasurements_Timestamp_Source_Tenant",
          table: "SchneiderMeasurements",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbMeasurements_Timestamp_Source_Tenant",
          table: "AbbMeasurements",
          columns: new[] { "Timestamp", "Source", "Tenant" });

      migrationBuilder.Sql("""
        create materialized view "AbbQuarterHourlyEnergyRange"
        with (timescaledb.continuous)
        as
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
            "Tenant"
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "AbbQuarterHourlyEnergyRange" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        select add_continuous_aggregate_policy(
          '"AbbQuarterHourlyEnergyRange"',
          start_offset => NULL,
          end_offset => INTERVAL '15 minutes',
          schedule_interval => INTERVAL '15 minutes'
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "SchneiderQuarterHourlyEnergyRange"
        with (timescaledb.continuous)
        as
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
            "Tenant"
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "SchneiderQuarterHourlyEnergyRange" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        select add_continuous_aggregate_policy(
          '"SchneiderQuarterHourlyEnergyRange"',
          start_offset => NULL,
          end_offset => INTERVAL '15 minutes',
          schedule_interval => INTERVAL '15 minutes'
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "AbbMonthlyEnergyRange"
        with (timescaledb.continuous)
        as
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
            "Tenant"
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "AbbMonthlyEnergyRange" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        select add_continuous_aggregate_policy(
          '"AbbMonthlyEnergyRange"',
          start_offset => NULL,
          end_offset => INTERVAL '1 month',
          schedule_interval => INTERVAL '1 month'
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "SchneiderMonthlyEnergyRange"
        with (timescaledb.continuous)
        as
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
            "Tenant"
        with no data;
      """);

      migrationBuilder.Sql("""
        create index on "SchneiderMonthlyEnergyRange" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        select add_continuous_aggregate_policy(
          '"SchneiderMonthlyEnergyRange"',
          start_offset => NULL,
          end_offset => INTERVAL '1 month',
          schedule_interval => INTERVAL '1 month'
        );
      """);

      migrationBuilder.Sql("""
        drop materialized view "MonthlyRangeEnergy";
      """);

      migrationBuilder.Sql("""
        drop materialized view "QuarterHourAveragePower";
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

      migrationBuilder.Sql("""
        drop materialized view "AbbQuarterHourlyEnergyRange";
      """);

      migrationBuilder.Sql("""
        drop materialized view "SchneiderQuarterHourlyEnergyRange";
      """);

      migrationBuilder.Sql("""
        drop materialized view "AbbMonthlyEnergyRange";
      """);

      migrationBuilder.Sql("""
        drop materialized view "SchneiderMonthlyEnergyRange";
      """);

      migrationBuilder.DropPrimaryKey(
          name: "PK_SchneiderMeasurements",
          table: "SchneiderMeasurements");

      migrationBuilder.DropIndex(
          name: "IX_SchneiderMeasurements_Timestamp_Source_Tenant",
          table: "SchneiderMeasurements");

      migrationBuilder.DropPrimaryKey(
          name: "PK_AbbMeasurements",
          table: "AbbMeasurements");

      migrationBuilder.DropIndex(
          name: "IX_AbbMeasurements_Timestamp_Source_Tenant",
          table: "AbbMeasurements");

      migrationBuilder.AddPrimaryKey(
          name: "PK_SchneiderMeasurements",
          table: "SchneiderMeasurements",
          columns: new[] { "Tenant", "Source", "Timestamp" });

      migrationBuilder.AddPrimaryKey(
          name: "PK_AbbMeasurements",
          table: "AbbMeasurements",
          columns: new[] { "Tenant", "Source", "Timestamp" });

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderMeasurements_Tenant_Source_Timestamp",
          table: "SchneiderMeasurements",
          columns: new[] { "Tenant", "Source", "Timestamp" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbMeasurements_Tenant_Source_Timestamp",
          table: "AbbMeasurements",
          columns: new[] { "Tenant", "Source", "Timestamp" });
    }
  }
}
