using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    public partial class PacketEncoder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues");

            migrationBuilder.DropColumn(
                name: "FinalValue",
                table: "DeterminedValues");

            migrationBuilder.DropColumn(
                name: "RawValue",
                table: "DeterminedValues");

            migrationBuilder.RenameColumn(
                name: "SensorId",
                table: "DeterminedValues",
                newName: "PacketDecoderId");

            migrationBuilder.RenameIndex(
                name: "IX_DeterminedValues_SensorId",
                table: "DeterminedValues",
                newName: "IX_DeterminedValues_PacketDecoderId");

            migrationBuilder.AddColumn<double>(
                name: "Value",
                table: "DeterminedValues",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "PacketDecoders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    RegisterDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ByteNumbers = table.Column<List<int>>(type: "integer[]", nullable: false),
                    KindProperty = table.Column<int>(type: "integer", nullable: false),
                    NameProperty = table.Column<string>(type: "text", nullable: false),
                    StartByte = table.Column<int>(type: "integer", nullable: false),
                    SensorId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PacketDecoders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PacketDecoders_Sensors_SensorId",
                        column: x => x.SensorId,
                        principalTable: "Sensors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PacketDecoders_SensorId",
                table: "PacketDecoders",
                column: "SensorId");

            migrationBuilder.AddForeignKey(
                name: "FK_DeterminedValues_PacketDecoders_PacketDecoderId",
                table: "DeterminedValues",
                column: "PacketDecoderId",
                principalTable: "PacketDecoders",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DeterminedValues_PacketDecoders_PacketDecoderId",
                table: "DeterminedValues");

            migrationBuilder.DropTable(
                name: "PacketDecoders");

            migrationBuilder.DropColumn(
                name: "Value",
                table: "DeterminedValues");

            migrationBuilder.RenameColumn(
                name: "PacketDecoderId",
                table: "DeterminedValues",
                newName: "SensorId");

            migrationBuilder.RenameIndex(
                name: "IX_DeterminedValues_PacketDecoderId",
                table: "DeterminedValues",
                newName: "IX_DeterminedValues_SensorId");

            migrationBuilder.AddColumn<double>(
                name: "FinalValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "RawValue",
                table: "DeterminedValues",
                type: "double precision",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_DeterminedValues_Sensors_SensorId",
                table: "DeterminedValues",
                column: "SensorId",
                principalTable: "Sensors",
                principalColumn: "Id");
        }
    }
}
