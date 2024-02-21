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
        create materialized view "AbbEnergyBoundsQuarterHourly"
        with (timescaledb.continuous)
        as
          select
            "Tenant",
            "Source",
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh"
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh"
            min("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMin_Wh",
            max("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMax_Wh"
            min("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMin_Wh",
            max("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMax_Wh"
          from
            "AbbMeasurements"
          group by
            time_bucket('15 minutes', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        create index on "AbbEnergyBoundsQuarterHourly" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "SchneiderEnergyBoundsQuarterHourly"
        with (timescaledb.continuous)
        as
          select
            "Tenant",
            "Source",
            time_bucket('15 minutes', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh"
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh"
            min("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMin_Wh",
            max("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMax_Wh"
            min("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMin_Wh",
            max("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMax_Wh"
          from
            "SchneiderMeasurements"
          group by
            time_bucket('15 minutes', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        create index on "SchneiderEnergyBoundsQuarterHourly" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "AbbEnergyBoundsMonthly"
        with (timescaledb.continuous)
        as
          select
            "Tenant",
            "Source",
            time_bucket('1 month', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh"
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh"
            min("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMin_Wh",
            max("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMax_Wh"
            min("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMin_Wh",
            max("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMax_Wh"
          from
            "AbbMeasurements"
          group by
            time_bucket('1 month', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        create index on "AbbEnergyBoundsMonthly" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);

      migrationBuilder.Sql("""
        create materialized view "SchneiderEnergyBoundsMonthly"
        with (timescaledb.continuous)
        as
          select
            "Tenant",
            "Source",
            time_bucket('1 month', "Timestamp") as "Timestamp",
            min("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMin_Wh",
            max("ActiveEnergyImportTotal_Wh") as "ActiveEnergyImportTotalMax_Wh"
            min("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMin_Wh",
            max("ActiveEnergyExportTotal_Wh") as "ActiveEnergyExportTotalMax_Wh"
            min("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMin_Wh",
            max("ReactiveEnergyImportTotal_Wh") as "ReactiveEnergyImportTotalMax_Wh"
            min("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMin_Wh",
            max("ReactiveEnergyExportTotal_Wh") as "ReactiveEnergyExportTotalMax_Wh"
          from
            "SchneiderMeasurements"
          group by
            time_bucket('1 month', "Timestamp"),
            "Source",
            "Tenant";
      """);

      migrationBuilder.Sql("""
        create index on "SchneiderEnergyBoundsMonthly" (
          "Timestamp",
          "Source",
          "Tenant"
        );
      """);
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.Sql("""
        drop materialized view if exists "AbbEnergyBoundsQuarterHourly";
      """);

      migrationBuilder.Sql("""
        drop materialized view if exists "SchneiderEnergyBoundsQuarterHourly";
      """);

      migrationBuilder.Sql("""
        drop materialized view if exists "AbbEnergyBoundsMonthly";
      """);

      migrationBuilder.Sql("""
        drop materialized view if exists "SchneiderEnergyBoundsMonthly";
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
