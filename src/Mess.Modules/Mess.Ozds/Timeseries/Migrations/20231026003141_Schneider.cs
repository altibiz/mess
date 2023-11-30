using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
  /// <inheritdoc />
  public partial class Schneider : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropColumn(
        name: "ActivePowerL1",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerL2",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerL3",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL1",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL2",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL3",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(name: "CurrentL1", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "CurrentL2", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "CurrentL3", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "Energy", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "HighEnergy", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "LowEnergy", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "Power", table: "AbbMeasurements");

      migrationBuilder.DropColumn(
        name: "ReactivePowerL1",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerL2",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerL3",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(name: "VoltageL1", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "VoltageL2", table: "AbbMeasurements");

      migrationBuilder.DropColumn(name: "VoltageL3", table: "AbbMeasurements");

      migrationBuilder.AlterColumn<short>(
        name: "PowerFactorL3",
        table: "AbbMeasurements",
        type: "int2",
        nullable: true,
        oldClrType: typeof(float),
        oldType: "float4",
        oldNullable: true
      );

      migrationBuilder.AlterColumn<short>(
        name: "PowerFactorL2",
        table: "AbbMeasurements",
        type: "int2",
        nullable: true,
        oldClrType: typeof(float),
        oldType: "float4",
        oldNullable: true
      );

      migrationBuilder.AlterColumn<short>(
        name: "PowerFactorL1",
        table: "AbbMeasurements",
        type: "int2",
        nullable: true,
        oldClrType: typeof(float),
        oldType: "float4",
        oldNullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyExportTariff1_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyExportTariff2_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyExportTotal_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyImportTariff1_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyImportTariff2_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyImportTotal_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<long>(
        name: "ActiveEnergyNetTotal_kWh",
        table: "AbbMeasurements",
        type: "int8",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ActivePowerL1_W",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ActivePowerL2_W",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ActivePowerL3_W",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ActivePowerTotal_W",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ApparentPowerL1_VA",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ApparentPowerL2_VA",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ApparentPowerL3_VA",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ApparentPowerTotal_VA",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "CurrentL1_A",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "CurrentL2_A",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "CurrentL3_A",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<short>(
        name: "PowerFactorTotal",
        table: "AbbMeasurements",
        type: "int2",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ReactivePowerL1_VAR",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ReactivePowerL2_VAR",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ReactivePowerL3_VAR",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "ReactivePowerTotal_VAR",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "VoltageL1_V",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "VoltageL2_V",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.AddColumn<int>(
        name: "VoltageL3_V",
        table: "AbbMeasurements",
        type: "int4",
        nullable: true
      );

      migrationBuilder.CreateTable(
        name: "SchneiderMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(
              type: "timestamptz",
              nullable: false
            ),
            VoltageL1_V = table.Column<float>(type: "float4", nullable: true),
            VoltageL2_V = table.Column<float>(type: "float4", nullable: true),
            VoltageL3_V = table.Column<float>(type: "float4", nullable: true),
            VoltageAvg_V = table.Column<float>(type: "float4", nullable: true),
            CurrentL1_A = table.Column<float>(type: "float4", nullable: true),
            CurrentL2_A = table.Column<float>(type: "float4", nullable: true),
            CurrentL3_A = table.Column<float>(type: "float4", nullable: true),
            CurrentAvg_A = table.Column<float>(type: "float4", nullable: true),
            ActivePowerL1_kW = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ActivePowerL2_kW = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ActivePowerL3_kW = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ActivePowerTotal_kW = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ReactivePowerTotal_kVAR = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ApparentPowerTotal_kVA = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            PowerFactorTotal = table.Column<float>(
              type: "float4",
              nullable: true
            ),
            ActiveEnergyImportTotal_Wh = table.Column<long>(
              type: "int8",
              nullable: true
            ),
            ActiveEnergyExportTotal_Wh = table.Column<long>(
              type: "int8",
              nullable: true
            ),
            ActiveEnergyImportRateA_Wh = table.Column<long>(
              type: "int8",
              nullable: true
            ),
            ActiveEnergyImportRateB_Wh = table.Column<long>(
              type: "int8",
              nullable: true
            ),
            Milliseconds = table.Column<long>(type: "bigint", nullable: false)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_SchneiderMeasurements",
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
        name: "IX_AbbMeasurements_Tenant_Source_Milliseconds",
        table: "AbbMeasurements",
        columns: new[] { "Tenant", "Source", "Milliseconds" }
      );

      migrationBuilder.CreateIndex(
        name: "IX_SchneiderMeasurements_Tenant_Source_Milliseconds",
        table: "SchneiderMeasurements",
        columns: new[] { "Tenant", "Source", "Milliseconds" }
      );

      migrationBuilder.CreateIndex(
        name: "IX_SchneiderMeasurements_Tenant_Source_Timestamp",
        table: "SchneiderMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "SchneiderMeasurements");

      migrationBuilder.DropIndex(
        name: "IX_AbbMeasurements_Tenant_Source_Milliseconds",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyExportTariff1_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyExportTariff2_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyExportTotal_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyImportTariff1_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyImportTariff2_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyImportTotal_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActiveEnergyNetTotal_kWh",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerL1_W",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerL2_W",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerL3_W",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ActivePowerTotal_W",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL1_VA",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL2_VA",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerL3_VA",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ApparentPowerTotal_VA",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "CurrentL1_A",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "CurrentL2_A",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "CurrentL3_A",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "PowerFactorTotal",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerL1_VAR",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerL2_VAR",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerL3_VAR",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "ReactivePowerTotal_VAR",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "VoltageL1_V",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "VoltageL2_V",
        table: "AbbMeasurements"
      );

      migrationBuilder.DropColumn(
        name: "VoltageL3_V",
        table: "AbbMeasurements"
      );

      migrationBuilder.AlterColumn<float>(
        name: "PowerFactorL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true,
        oldClrType: typeof(short),
        oldType: "int2",
        oldNullable: true
      );

      migrationBuilder.AlterColumn<float>(
        name: "PowerFactorL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true,
        oldClrType: typeof(short),
        oldType: "int2",
        oldNullable: true
      );

      migrationBuilder.AlterColumn<float>(
        name: "PowerFactorL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true,
        oldClrType: typeof(short),
        oldType: "int2",
        oldNullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ActivePowerL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ActivePowerL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ActivePowerL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ApparentPowerL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ApparentPowerL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ApparentPowerL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "CurrentL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "CurrentL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "CurrentL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "Energy",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "HighEnergy",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "LowEnergy",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "Power",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ReactivePowerL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ReactivePowerL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "ReactivePowerL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "VoltageL1",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "VoltageL2",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );

      migrationBuilder.AddColumn<float>(
        name: "VoltageL3",
        table: "AbbMeasurements",
        type: "float4",
        nullable: true
      );
    }
  }
}
