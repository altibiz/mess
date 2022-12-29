using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Migrations.Timescale
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.CreateTable(
        name: "Measurements",
        columns: table =>
          new
          {
            Timestamp = table.Column<DateTime>(
              type: "timestamp with time zone",
              nullable: false
            ),
            SourceId = table.Column<string>(type: "string", nullable: false),
            Power = table.Column<float>(type: "float", nullable: false),
            Voltage = table.Column<float>(type: "float", nullable: false),
            Tenant = table.Column<string>(type: "string", nullable: false)
          },
        constraints: table =>
        {
          table.PrimaryKey(
            "PK_Measurements",
            x => new { x.Timestamp, x.SourceId }
          );
        }
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropTable(name: "Measurements");
    }
  }
}
