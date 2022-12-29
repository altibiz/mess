using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mess.Migrations.Timescale
{
  public partial class Initial : Migration
  {
    protected override void Up(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.AlterColumn<DateTime>(
        name: "Timestamp",
        table: "Measurements",
        type: "timestamp with time zone",
        nullable: false,
        oldClrType: typeof(DateTime),
        oldType: "timestamp"
      );

      migrationBuilder.AddColumn<string>(
        name: "SourceId",
        table: "Measurements",
        type: "string",
        nullable: false,
        defaultValue: ""
      );

      migrationBuilder.AddColumn<float>(
        name: "Voltage",
        table: "Measurements",
        type: "float",
        nullable: false,
        defaultValue: 0f
      );

      migrationBuilder.AddPrimaryKey(
        name: "PK_Measurements",
        table: "Measurements",
        columns: new[] { "Timestamp", "SourceId" }
      );
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
      migrationBuilder.DropPrimaryKey(
        name: "PK_Measurements",
        table: "Measurements"
      );

      migrationBuilder.DropColumn(name: "SourceId", table: "Measurements");

      migrationBuilder.DropColumn(name: "Voltage", table: "Measurements");

      migrationBuilder.AlterColumn<DateTime>(
        name: "Timestamp",
        table: "Measurements",
        type: "timestamp",
        nullable: false,
        oldClrType: typeof(DateTime),
        oldType: "timestamp with time zone"
      );
    }
  }
}
