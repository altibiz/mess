using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries
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
            Timestamp = table.Column<DateTimeOffset>(
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
            PowerFactorL3 = table.Column<float>(type: "float4", nullable: true),
            Milliseconds = table.Column<long>(type: "bigint", nullable: false),
            Energy = table.Column<float>(type: "float4", nullable: true),
            LowEnergy = table.Column<float>(type: "float4", nullable: true),
            HighEnergy = table.Column<float>(type: "float4", nullable: true),
            Power = table.Column<float>(type: "float4", nullable: true)
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

      migrationBuilder.CreateIndex(
        name: "IX_AbbMeasurements_Tenant_Source_Timestamp",
        table: "AbbMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "AbbMeasurements");
    }
  }
}
