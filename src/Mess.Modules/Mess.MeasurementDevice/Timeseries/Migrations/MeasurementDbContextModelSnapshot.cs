﻿// <auto-generated />
using System;
using Mess.MeasurementDevice.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Mess.MeasurementDevice.Timeseries.Migrations
{
    [DbContext(typeof(MeasurementDbContext))]
    partial class MeasurementDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Mess.Timeseries.Entities.EgaugeMeasurementEntity", b =>
                {
                    b.Property<string>("Tenant")
                        .HasColumnType("varchar");

                    b.Property<string>("Source")
                        .HasColumnType("varchar");

                    b.Property<DateTime>("Timestamp")
                        .HasColumnType("timestamp");

                    b.Property<float>("Power")
                        .HasColumnType("float");

                    b.Property<float>("Voltage")
                        .HasColumnType("float");

                    b.HasKey("Tenant", "Source", "Timestamp");

                    b.ToTable("EgaugeMeasurements");
                });
#pragma warning restore 612, 618
        }
    }
}