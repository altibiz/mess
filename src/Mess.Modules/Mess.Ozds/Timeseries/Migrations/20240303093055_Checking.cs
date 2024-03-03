using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class Checking : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql("""
        update "AbbMeasurements"
          set
            "ReactivePowerL1_VAR" = 0,
            "ReactivePowerL2_VAR" = 0,
            "ReactivePowerL3_VAR" = 0,
            "ActivePowerExportL1_Wh" = 0,
            "ActivePowerExportL2_Wh" = 0,
            "ActivePowerExportL3_Wh" = 0,
            "ActiveEnergyExportTotal_Wh" = 0,
            "ReactivePowerImportL1_VARh" = 0,
            "ReactivePowerImportL2_VARh" = 0,
            "ReactivePowerImportL3_VARh" = 0,
            "ReactiveEnergyImportTotal_VARh" = 0,
            "ReactivePowerExportL1_VARh" = 0,
            "ReactivePowerExportL2_VARh" = 0,
            "ReactivePowerExportL3_VARh" = 0,
            "ReactiveEnergyExportTotal_VARh" = 0
          where
            "ReactivePowerL1_VAR" = 21474836.47
            and "ReactivePowerL2_VAR" = 21474836.47
            and "ReactivePowerL3_VAR" = 21474836.47
            and "ActivePowerExportL1_Wh" = 184467440737095500000
            and "ActivePowerExportL2_Wh" = 184467440737095500000
            and "ActivePowerExportL3_Wh" = 184467440737095500000
            and "ActiveEnergyExportTotal_Wh" = 184467440737095500000
            and "ReactivePowerImportL1_VARh" = 184467440737095500000
            and "ReactivePowerImportL2_VARh" = 184467440737095500000
            and "ReactivePowerImportL3_VARh" = 184467440737095500000
            and "ReactiveEnergyImportTotal_VARh" = 184467440737095500000
            and "ReactivePowerExportL1_VARh" = 184467440737095500000
            and "ReactivePowerExportL2_VARh" = 184467440737095500000
            and "ReactivePowerExportL3_VARh" = 184467440737095500000
            and "ReactiveEnergyExportTotal_VARh" = 184467440737095500000;
      """);

      migrationBuilder.Sql("""
        delete from "SchneiderMeasurements"
        where
          "VoltageL1_V" < 161 or "VoltageL1_V" >= 300
          or "VoltageL2_V" < 161 or "VoltageL2_V" >= 300
          or "VoltageL3_V" < 161 or "VoltageL3_V" >= 300
          or "CurrentL1_A" < 0 or "CurrentL1_A" >= 80
          or "CurrentL2_A" < 0 or "CurrentL2_A" >= 80
          or "CurrentL3_A" < 0 or "CurrentL3_A" >= 80
          or "ActivePowerL1_W" < -24000 or "ActivePowerL1_W" >= 24000
          or "ActivePowerL2_W" < -24000 or "ActivePowerL2_W" >= 24000
          or "ActivePowerL3_W" < -24000 or "ActivePowerL3_W" >= 24000
          or "ReactivePowerTotal_VAR" < -72000 or "ReactivePowerTotal_VAR" >= 72000
          or "ApparentPowerTotal_VA" < -72000 or "ApparentPowerTotal_VA" >= 72000;
      """);

      migrationBuilder.Sql("""
        delete from "SchneiderQuarterHourlyAggregates"
        where true;
      """);

      migrationBuilder.Sql("""
        delete from "SchneiderDailyAggregates"
        where true;
      """);

      migrationBuilder.Sql("""
        delete from "SchneiderMonthlyAggregates"
        where true;
      """);

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
        delete from "AbbMeasurements"
        where
          "VoltageL1_V" < 161 or "VoltageL1_V" >= 300
          or "VoltageL2_V" < 161 or "VoltageL2_V" >= 300
          or "VoltageL3_V" < 161 or "VoltageL3_V" >= 300
          or "CurrentL1_A" < 0 or "CurrentL1_A" >= 80
          or "CurrentL2_A" < 0 or "CurrentL2_A" >= 80
          or "CurrentL3_A" < 0 or "CurrentL3_A" >= 80
          or "ActivePowerL1_W" < -24000 or "ActivePowerL1_W" >= 24000
          or "ActivePowerL2_W" < -24000 or "ActivePowerL2_W" >= 24000
          or "ActivePowerL3_W" < -24000 or "ActivePowerL3_W" >= 24000
          or "ReactivePowerL1_VAR" < -24000 or "ReactivePowerL1_VAR" >= 24000
          or "ReactivePowerL2_VAR" < -24000 or "ReactivePowerL2_VAR" >= 24000
          or "ReactivePowerL3_VAR" < -24000 or "ReactivePowerL3_VAR" >= 24000;
      """);

      migrationBuilder.Sql("""
        delete from "AbbQuarterHourlyAggregates"
        where true;
      """);

      migrationBuilder.Sql("""
        delete from "AbbDailyAggregates"
        where true;
      """);

      migrationBuilder.Sql("""
        delete from "AbbMonthlyAggregates"
        where true;
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
    }


    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
  }
}
