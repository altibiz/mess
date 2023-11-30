using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Eor.Iot.Migrations
{
  /// <inheritdoc />
  public partial class Initial : Migration
  {
    /// <inheritdoc />
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder
        .AlterDatabase()
        .Annotation("Npgsql:Enum:eor_diode_bridge_state", "error,ok")
        .Annotation("Npgsql:Enum:eor_door_state", "closed,open")
        .Annotation("Npgsql:Enum:eor_main_circuit_breaker_state", "off,on")
        .Annotation(
          "Npgsql:Enum:eor_measurement_device_reset_state",
          "shouldnt_reset,should_reset"
        )
        .Annotation(
          "Npgsql:Enum:eor_measurement_device_run_state",
          "stopped,started,error"
        )
        .Annotation("Npgsql:Enum:eor_transformer_contractor_state", "on,off")
        .Annotation("Npgsql:PostgresExtension:timescaledb", ",,");

      migrationBuilder.CreateTable(
        name: "EorMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(
              type: "timestamptz",
              nullable: false
            ),
            Voltage = table.Column<float>(type: "float4", nullable: false),
            Current = table.Column<float>(type: "float4", nullable: false),
            Temperature = table.Column<float>(type: "float4", nullable: false),
            CoolingFans = table.Column<bool>(type: "boolean", nullable: false),
            HatsinkFans = table.Column<bool>(type: "boolean", nullable: false)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_EorMeasurements",
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
        name: "EorStatuses",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(
              type: "timestamptz",
              nullable: false
            ),
            Stamp = table.Column<int>(type: "integer", nullable: false),
            Mode = table.Column<int>(type: "integer", nullable: false),
            ProcessFault = table.Column<int>(type: "integer", nullable: false),
            ProcessFaults = table.Column<string[]>(
              type: "text[]",
              nullable: false
            ),
            CommunicationFault = table.Column<int>(
              type: "integer",
              nullable: false
            ),
            RunState = table.Column<int>(type: "integer", nullable: false),
            ResetState = table.Column<int>(type: "integer", nullable: false),
            DoorState = table.Column<int>(type: "integer", nullable: false),
            MainCircuitBreakerState = table.Column<int>(
              type: "integer",
              nullable: false
            ),
            TransformerContractorState = table.Column<int>(
              type: "integer",
              nullable: false
            ),
            FirstDiodeBridgeState = table.Column<int>(
              type: "integer",
              nullable: false
            ),
            SecondDiodeBridgeState = table.Column<int>(
              type: "integer",
              nullable: false
            ),
            Current = table.Column<float>(type: "float4", nullable: false),
            Voltage = table.Column<float>(type: "float4", nullable: false),
            Temperature = table.Column<float>(type: "real", nullable: false),
            HeatsinkFans = table.Column<bool>(type: "boolean", nullable: false),
            CoolingFans = table.Column<bool>(type: "boolean", nullable: false)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_EorStatuses",
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
        name: "IX_EorMeasurements_Tenant_Source_Timestamp",
        table: "EorMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );

      migrationBuilder.CreateIndex(
        name: "IX_EorStatuses_Tenant_Source_Timestamp",
        table: "EorStatuses",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "EorMeasurements");

      migrationBuilder.DropTable(name: "EorStatuses");
    }
  }
}
