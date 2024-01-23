using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
    /// <inheritdoc />
    public partial class Epilogue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportRateA_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportRateB_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL1_kW",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL2_kW",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL3_kW",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerTotal_kW",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerTotal_kVA",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "CurrentAvg_A",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "PowerFactorTotal",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerTotal_kVAR",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "VoltageAvg_V",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyExportTariff1_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyExportTariff2_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyExportTotal_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTariff1_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTariff2_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotal_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyNetTotal_kWh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerTotal_W",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerL1_VA",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerL2_VA",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerL3_VA",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerTotal_VA",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "PowerFactorL1",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "PowerFactorL2",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "PowerFactorL3",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "PowerFactorTotal",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerTotal_VAR",
                table: "AbbMeasurements");

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL3_V",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL2_V",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL1_V",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL3_A",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL2_A",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL1_A",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(float),
                oldType: "float4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ActiveEnergyImportTotal_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(long),
                oldType: "int8",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ActiveEnergyExportTotal_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(long),
                oldType: "int8",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportL1_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportL2_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportL3_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerL1_W",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerL2_W",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerL3_W",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ApparentPowerTotal_VA",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactiveEnergyExportTotal_VARh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactiveEnergyImportTotal_VARh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerTotal_VAR",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL3_V",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL2_V",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "VoltageL1_V",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReactivePowerL3_VAR",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReactivePowerL2_VAR",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ReactivePowerL1_VAR",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL3_A",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL2_A",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "CurrentL1_A",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ActivePowerL3_W",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ActivePowerL2_W",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "ActivePowerL1_W",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(int),
                oldType: "int4",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyExportTotal_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotal_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerExportL1_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerExportL2_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerExportL3_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerImportL1_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerImportL2_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActivePowerImportL3_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactiveEnergyExportTotal_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactiveEnergyImportTotal_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerExportL1_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerExportL2_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerExportL3_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerImportL1_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerImportL2_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ReactivePowerImportL3_VARh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportL1_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportL2_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportL3_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL1_W",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL2_W",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerL3_W",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ApparentPowerTotal_VA",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactiveEnergyExportTotal_VARh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactiveEnergyImportTotal_VARh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerTotal_VAR",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyExportTotal_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotal_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerExportL1_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerExportL2_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerExportL3_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerImportL1_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerImportL2_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActivePowerImportL3_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactiveEnergyExportTotal_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactiveEnergyImportTotal_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerExportL1_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerExportL2_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerExportL3_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerImportL1_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerImportL2_VARh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ReactivePowerImportL3_VARh",
                table: "AbbMeasurements");

            migrationBuilder.AlterColumn<float>(
                name: "VoltageL3_V",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "VoltageL2_V",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "VoltageL1_V",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "CurrentL3_A",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "CurrentL2_A",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<float>(
                name: "CurrentL1_A",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<long>(
                name: "ActiveEnergyImportTotal_Wh",
                table: "SchneiderMeasurements",
                type: "int8",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<long>(
                name: "ActiveEnergyExportTotal_Wh",
                table: "SchneiderMeasurements",
                type: "int8",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyImportRateA_Wh",
                table: "SchneiderMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyImportRateB_Wh",
                table: "SchneiderMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ActivePowerL1_kW",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ActivePowerL2_kW",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ActivePowerL3_kW",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ActivePowerTotal_kW",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ApparentPowerTotal_kVA",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "CurrentAvg_A",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "PowerFactorTotal",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "ReactivePowerTotal_kVAR",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "VoltageAvg_V",
                table: "SchneiderMeasurements",
                type: "float4",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "VoltageL3_V",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "VoltageL2_V",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "VoltageL1_V",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ReactivePowerL3_VAR",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ReactivePowerL2_VAR",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ReactivePowerL1_VAR",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentL3_A",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentL2_A",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "CurrentL1_A",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ActivePowerL3_W",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ActivePowerL2_W",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<int>(
                name: "ActivePowerL1_W",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyExportTariff1_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyExportTariff2_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyExportTotal_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyImportTariff1_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyImportTariff2_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyImportTotal_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveEnergyNetTotal_kWh",
                table: "AbbMeasurements",
                type: "int8",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ActivePowerTotal_W",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApparentPowerL1_VA",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApparentPowerL2_VA",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApparentPowerL3_VA",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ApparentPowerTotal_VA",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PowerFactorL1",
                table: "AbbMeasurements",
                type: "int2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PowerFactorL2",
                table: "AbbMeasurements",
                type: "int2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PowerFactorL3",
                table: "AbbMeasurements",
                type: "int2",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "PowerFactorTotal",
                table: "AbbMeasurements",
                type: "int2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ReactivePowerTotal_VAR",
                table: "AbbMeasurements",
                type: "int4",
                nullable: true);
        }
    }
}
