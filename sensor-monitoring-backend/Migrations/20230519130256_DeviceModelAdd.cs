using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    public partial class DeviceModelAdd : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues");

            migrationBuilder.DropColumn(
                name: "IMEIModem",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "deviceModel",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "simCardCharge",
                table: "Sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "DeviceId",
                table: "Sensors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Kind",
                table: "Sensors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NickName",
                table: "Sensors",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortKind",
                table: "Sensors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortName",
                table: "Sensors",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UnitMeasurement",
                table: "Sensors",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "SensorId",
                table: "DeterminedValues",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<double>(
                name: "RawValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<double>(
                name: "FinalValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "double precision");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeDetermined",
                table: "DeterminedValues",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone");

            migrationBuilder.CreateTable(
                name: "Devices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    deviceModel = table.Column<string>(type: "text", nullable: true),
                    phoneNumber = table.Column<string>(type: "text", nullable: true),
                    IMEIModem = table.Column<string>(type: "text", nullable: true),
                    simCardCharge = table.Column<string>(type: "text", nullable: true),
                    NickName = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Devices", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TemplateSensors",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    NickName = table.Column<string>(type: "text", nullable: true),
                    Kind = table.Column<int>(type: "integer", nullable: true),
                    UnitMeasurement = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TemplateSensors", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors",
                column: "DeviceId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors",
                column: "DeviceId",
                principalTable: "Devices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues");

            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_Devices_DeviceId",
                table: "Sensors");

            migrationBuilder.DropTable(
                name: "Devices");

            migrationBuilder.DropTable(
                name: "TemplateSensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_DeviceId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "DeviceId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "PortKind",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "PortName",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "UnitMeasurement",
                table: "Sensors");

            migrationBuilder.AddColumn<string>(
                name: "IMEIModem",
                table: "Sensors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "deviceModel",
                table: "Sensors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "Sensors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "simCardCharge",
                table: "Sensors",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<Guid>(
                name: "SensorId",
                table: "DeterminedValues",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "RawValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "FinalValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "double precision",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateTimeDetermined",
                table: "DeterminedValues",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
