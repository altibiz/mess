using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.MeasurementDevice.Timeseries.Migrations
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "EgaugeMeasurements",
        columns: table =>
          new
          {
            Tenant = table.Column<string>(type: "varchar", nullable: false),
            Source = table.Column<string>(type: "varchar", nullable: false),
            Timestamp = table.Column<DateTime>(
              type: "timestamp",
              nullable: false
            ),
            Power = table.Column<float>(type: "float", nullable: false),
            Voltage = table.Column<float>(type: "float", nullable: false)
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
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "EgaugeMeasurements");
    }
  }
}
