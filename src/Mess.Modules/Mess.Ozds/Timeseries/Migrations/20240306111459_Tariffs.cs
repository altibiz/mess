using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
    /// <inheritdoc />
    public partial class Tariffs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2_Wh",
                table: "SchneiderMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbQuarterHourlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbMonthlyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2_Wh",
                table: "AbbMeasurements",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbDailyAggregates",
                type: "float8",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2_Wh",
                table: "SchneiderMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "SchneiderDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "SchneiderDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "SchneiderDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "SchneiderDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbQuarterHourlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbMonthlyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2_Wh",
                table: "AbbMeasurements");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Max_Wh",
                table: "AbbDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT1Min_Wh",
                table: "AbbDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Max_Wh",
                table: "AbbDailyAggregates");

            migrationBuilder.DropColumn(
                name: "ActiveEnergyImportTotalT2Min_Wh",
                table: "AbbDailyAggregates");
        }
    }
}
