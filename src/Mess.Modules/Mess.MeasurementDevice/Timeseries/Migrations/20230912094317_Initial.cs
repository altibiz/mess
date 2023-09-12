using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.MeasurementDevice.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class Initial : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder
        .AlterDatabase()
        .Annotation("Npgsql:PostgresExtension:timescaledb", ",,");

      migrationBuilder.CreateTable(
        name: "AbbMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTime>(
              type: "timestamptz",
              nullable: false
            ),
            CurrentL1 = table.Column<float>(type: "float4", nullable: true),
            CurrentL2 = table.Column<float>(type: "float4", nullable: true),
            CurrentL3 = table.Column<float>(type: "float4", nullable: true),
            VoltageL1 = table.Column<float>(type: "float4", nullable: true),
            VoltageL2 = table.Column<float>(type: "float4", nullable: true),
            VoltageL3 = table.Column<float>(type: "float4", nullable: true),
            ActivePowerL1 = table.Column<float>(type: "float4", nullable: true),
            ActivePowerL2 = table.Column<float>(type: "float4", nullable: true),
            ActivePowerL3 = table.Column<float>(type: "float4", nullable: true),
            ReactivePowerL1 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ReactivePowerL2 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ReactivePowerL3 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ApparentPowerL1 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ApparentPowerL2 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ApparentPowerL3 = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            PowerFactorL1 = table.Column<float>(type: "float4", nullable: true),
            PowerFactorL2 = table.Column<float>(type: "float4", nullable: true),
            PowerFactorL3 = table.Column<float>(type: "float4", nullable: true)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_AbbMeasurements",
            x =>
              new
              {
                x.Tenant,
                x.Source,
                x.Timestamp
              }
          );
        }
      );

      migrationBuilder.CreateTable(
        name: "EgaugeMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTime>(
              type: "timestamptz",
              nullable: false
            ),
            Power = table.Column<float>(type: "float4", nullable: false),
            Voltage = table.Column<float>(type: "float4", nullable: false)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_EgaugeMeasurements",
            x =>
              new
              {
                x.Tenant,
                x.Source,
                x.Timestamp
              }
          );
        }
      );

      migrationBuilder.CreateIndex(
        name: "IX_AbbMeasurements_Tenant_Source_Timestamp",
        table: "AbbMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );

      migrationBuilder.CreateIndex(
        name: "IX_EgaugeMeasurements_Tenant_Source_Timestamp",
        table: "EgaugeMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "AbbMeasurements");

      migrationBuilder.DropTable(name: "EgaugeMeasurements");
    }
  }
}
