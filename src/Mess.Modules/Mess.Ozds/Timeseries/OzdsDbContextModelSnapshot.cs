﻿// <auto-generated />
using System;
using Mess.Ozds.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mess.Ozds.Timeseries
{
    [DbContext(typeof(OzdsDbContext))]
    partial class OzdsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "timescaledb");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mess.Ozds.Entities.AbbMeasurementEntity", b =>
                {
                    b.Property<string>("Tenant")
                        .HasColumnType("text");

                    b.Property<string>("Source")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamptz");

                    b.Property<float?>("ActivePowerL1")
                        .HasColumnType("float4");

                    b.Property<float?>("ActivePowerL2")
                        .HasColumnType("float4");

                    b.Property<float?>("ActivePowerL3")
                        .HasColumnType("float4");

                    b.Property<float?>("ApparentPowerL1")
                        .HasColumnType("float4");

                    b.Property<float?>("ApparentPowerL2")
                        .HasColumnType("float4");

                    b.Property<float?>("ApparentPowerL3")
                        .HasColumnType("float4");

                    b.Property<float?>("CurrentL1")
                        .HasColumnType("float4");

                    b.Property<float?>("CurrentL2")
                        .HasColumnType("float4");

                    b.Property<float?>("CurrentL3")
                        .HasColumnType("float4");

                    b.Property<float?>("Energy")
                        .HasColumnType("float4");

                    b.Property<float?>("HighEnergy")
                        .HasColumnType("float4");

                    b.Property<float?>("LowEnergy")
                        .HasColumnType("float4");

                    b.Property<long>("Milliseconds")
                        .HasColumnType("bigint");

                    b.Property<float?>("Power")
                        .HasColumnType("float4");

                    b.Property<float?>("PowerFactorL1")
                        .HasColumnType("float4");

                    b.Property<float?>("PowerFactorL2")
                        .HasColumnType("float4");

                    b.Property<float?>("PowerFactorL3")
                        .HasColumnType("float4");

                    b.Property<float?>("ReactivePowerL1")
                        .HasColumnType("float4");

                    b.Property<float?>("ReactivePowerL2")
                        .HasColumnType("float4");

                    b.Property<float?>("ReactivePowerL3")
                        .HasColumnType("float4");

                    b.Property<float?>("VoltageL1")
                        .HasColumnType("float4");

                    b.Property<float?>("VoltageL2")
                        .HasColumnType("float4");

                    b.Property<float?>("VoltageL3")
                        .HasColumnType("float4");

                    b.HasKey("Tenant", "Source", "Timestamp");

                    b.HasIndex("Tenant", "Source", "Timestamp");

                    b.ToTable("AbbMeasurements");
                });
#pragma warning restore 612, 618
        }
    }
}
