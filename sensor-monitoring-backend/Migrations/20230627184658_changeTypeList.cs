using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    public partial class changeTypeList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<double>>(
                name: "ByteNumbers",
                table: "PacketDecoders",
                type: "double precision[]",
                nullable: false,
                oldClrType: typeof(List<int>),
                oldType: "integer[]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<List<int>>(
                name: "ByteNumbers",
                table: "PacketDecoders",
                type: "integer[]",
                nullable: false,
                oldClrType: typeof(List<double>),
                oldType: "double precision[]");
        }
    }
}
