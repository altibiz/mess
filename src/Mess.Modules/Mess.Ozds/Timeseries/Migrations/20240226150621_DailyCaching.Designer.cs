﻿// <auto-generated />
using System;
using Mess.Ozds.Timeseries;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mess.Ozds.Timeseries.Migrations
{
    [DbContext(typeof(OzdsTimeseriesDbContext))]
    [Migration("20240226150621_DailyCaching")]
    partial class DailyCaching
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.11")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "timescaledb");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mess.Ozds.Timeseries.AbbDailyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("AbbDailyEnergyRanges", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.AbbMeasurementEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotal_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActiveEnergyImportTotal_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerExportL1_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerExportL2_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerExportL3_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerImportL1_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerImportL2_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerImportL3_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL1_W")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL2_W")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL3_W")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL1_A")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL2_A")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL3_A")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactiveEnergyExportTotal_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactiveEnergyImportTotal_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerExportL1_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerExportL2_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerExportL3_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerImportL1_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerImportL2_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerImportL3_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerL1_VAR")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerL2_VAR")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerL3_VAR")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL1_V")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL2_V")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL3_V")
                        .HasColumnType("double precision");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("AbbMeasurements", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.AbbMonthlyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("AbbMonthlyEnergyRanges", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.AbbQuarterHourlyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("AbbQuarterHourlyEnergyRanges", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.SchneiderDailyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("SchneiderDailyEnergyRanges", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.SchneiderMeasurementEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotal_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActiveEnergyImportL1_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActiveEnergyImportL2_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActiveEnergyImportL3_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActiveEnergyImportTotal_Wh")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL1_W")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL2_W")
                        .HasColumnType("double precision");

                    b.Property<double>("ActivePowerL3_W")
                        .HasColumnType("double precision");

                    b.Property<double>("ApparentPowerTotal_VA")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL1_A")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL2_A")
                        .HasColumnType("double precision");

                    b.Property<double>("CurrentL3_A")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactiveEnergyExportTotal_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactiveEnergyImportTotal_VARh")
                        .HasColumnType("double precision");

                    b.Property<double>("ReactivePowerTotal_VAR")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL1_V")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL2_V")
                        .HasColumnType("double precision");

                    b.Property<double>("VoltageL3_V")
                        .HasColumnType("double precision");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("SchneiderMeasurements", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.SchneiderMonthlyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("SchneiderMonthlyEnergyRanges", (string)null);
                });

            modelBuilder.Entity("Mess.Ozds.Timeseries.SchneiderQuarterHourlyEnergyRangeEntity", b =>
                {
                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<double>("ActiveEnergyExportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyExportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMax_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ActiveEnergyImportTotalMin_Wh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyExportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMax_VARh")
                        .HasColumnType("float8");

                    b.Property<double>("ReactiveEnergyImportTotalMin_VARh")
                        .HasColumnType("float8");

                    b.HasKey("Timestamp", "Source", "Tenant");

                    b.HasIndex("Timestamp", "Source", "Tenant");

                    b.ToTable("SchneiderQuarterHourlyEnergyRanges", (string)null);
                });
#pragma warning restore 612, 618
        }
    }
}