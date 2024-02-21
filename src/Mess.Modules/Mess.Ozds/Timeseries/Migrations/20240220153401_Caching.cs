using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class Caching : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropIndex(
          name: "IX_SchneiderMeasurements_Tenant_Source_Milliseconds",
          table: "SchneiderMeasurements");

      migrationBuilder.DropIndex(
          name: "IX_AbbMeasurements_Tenant_Source_Milliseconds",
          table: "AbbMeasurements");

      migrationBuilder.DropColumn(
          name: "Milliseconds",
          table: "SchneiderMeasurements");

      migrationBuilder.DropColumn(
          name: "Milliseconds",
          table: "AbbMeasurements");

      migrationBuilder.Sql("""
        create materialized view if not exists "MonthlyBoundsEnergy" as
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
          or timestamp_descending = 1;
      """);

      migrationBuilder.Sql("""
        create index on "MonthlyBoundsEnergy" (
          "Tenant",
          "Source",
          "Timestamp"
        );
      """);

      migrationBuilder.Sql("""
        create materialized view if not exists "QuarterHourAveragePower" as
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
          sum;
      """);

      migrationBuilder.Sql("""
        create index on "QuarterHourAveragePower" (
          "Tenant",
          "Source",
          "Timestamp"
        );
      """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql("""
        drop materialized view "MonthlyBoundsEnergy";
      """);

      migrationBuilder.Sql("""
        drop materialized view "QuarterHourAveragePower";
      """);

      migrationBuilder.AddColumn<long>(
          name: "Milliseconds",
          table: "SchneiderMeasurements",
          type: "bigint",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.AddColumn<long>(
          name: "Milliseconds",
          table: "AbbMeasurements",
          type: "bigint",
          nullable: false,
          defaultValue: 0L);

      migrationBuilder.CreateIndex(
          name: "IX_SchneiderMeasurements_Tenant_Source_Milliseconds",
          table: "SchneiderMeasurements",
          columns: new[] { "Tenant", "Source", "Milliseconds" });

      migrationBuilder.CreateIndex(
          name: "IX_AbbMeasurements_Tenant_Source_Milliseconds",
          table: "AbbMeasurements",
          columns: new[] { "Tenant", "Source", "Milliseconds" });
    }
  }
}
