using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Enms.Timeseries.Migrations
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
        name: "EgaugeMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "text", nullable: false),
            Source = table.Column<string>(type: "text", nullable: false),
            Timestamp = table.Column<DateTimeOffset>(
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
        name: "IX_EgaugeMeasurements_Tenant_Source_Timestamp",
        table: "EgaugeMeasurements",
        columns: new[] { "Tenant", "Source", "Timestamp" }
      );
    }

    /// <inheritdoc />
    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "EgaugeMeasurements");
    }
  }
}
