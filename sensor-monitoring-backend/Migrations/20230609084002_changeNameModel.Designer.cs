﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using sensor_monitoring_backend.Data;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    [DbContext(typeof(DatabaseContext))]
    [Migration("20230609084002_changeNameModel")]
    partial class changeNameModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entities.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("DeviceModel")
                        .HasColumnType("text");

                    b.Property<string>("IMEIModem")
                        .HasColumnType("text");

                    b.Property<double?>("LatPosition")
                        .HasColumnType("double precision");

                    b.Property<double?>("LngPosition")
                        .HasColumnType("double precision");

                    b.Property<string>("NickName")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SimCardCharge")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entities.TemplateSensor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int?>("Kind")
                        .HasColumnType("integer");

                    b.Property<string>("NickName")
                        .HasColumnType("text");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UnitMeasurement")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("TemplateSensors");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entity.DeterminedValue", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime?>("DateTimeDetermined")
                        .HasColumnType("timestamp with time zone");

                    b.Property<double?>("FinalValue")
                        .HasColumnType("double precision");

                    b.Property<double?>("RawValue")
                        .HasColumnType("double precision");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("SensorId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("SensorId");

                    b.ToTable("DeterminedValues");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entity.Sensor", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("DeviceId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.Property<int?>("PortKind")
                        .HasColumnType("integer");

                    b.Property<int?>("PortName")
                        .HasColumnType("integer");

                    b.Property<DateTime>("RegisterDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid?>("TemplateSensorId")
                        .IsRequired()
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DeviceId");

                    b.HasIndex("TemplateSensorId");

                    b.ToTable("Sensors");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entity.DeterminedValue", b =>
                {
                    b.HasOne("sensor_monitoring_backend.Domain.Entity.Sensor", "Sensor")
                        .WithMany("DeterminedValues")
                        .HasForeignKey("SensorId");

                    b.Navigation("Sensor");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entity.Sensor", b =>
                {
                    b.HasOne("sensor_monitoring_backend.Domain.Entities.Device", "Device")
                        .WithMany("Sensors")
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("sensor_monitoring_backend.Domain.Entities.TemplateSensor", "TemplateSensor")
                        .WithMany("Sensors")
                        .HasForeignKey("TemplateSensorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Device");

                    b.Navigation("TemplateSensor");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entities.Device", b =>
                {
                    b.Navigation("Sensors");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entities.TemplateSensor", b =>
                {
                    b.Navigation("Sensors");
                });

            modelBuilder.Entity("sensor_monitoring_backend.Domain.Entity.Sensor", b =>
                {
                    b.Navigation("DeterminedValues");
                });
#pragma warning restore 612, 618
        }
    }
}
