using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    public partial class fixTemplateSensor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kind",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "NickName",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "UnitMeasurement",
                table: "Sensors");

            migrationBuilder.AddColumn<Guid>(
                name: "TemplateSensorId",
                table: "Sensors",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Sensors_TemplateSensorId",
                table: "Sensors",
                column: "TemplateSensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sensors_TemplateSensors_TemplateSensorId",
                table: "Sensors",
                column: "TemplateSensorId",
                principalTable: "TemplateSensors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sensors_TemplateSensors_TemplateSensorId",
                table: "Sensors");

            migrationBuilder.DropIndex(
                name: "IX_Sensors_TemplateSensorId",
                table: "Sensors");

            migrationBuilder.DropColumn(
                name: "TemplateSensorId",
                table: "Sensors");

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

            migrationBuilder.AddColumn<string>(
                name: "UnitMeasurement",
                table: "Sensors",
                type: "text",
                nullable: true);
        }
    }
}
