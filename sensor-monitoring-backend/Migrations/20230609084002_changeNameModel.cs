using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace sensor_monitoring_backend.Migrations
{
    public partial class changeNameModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "simCardCharge",
                table: "Devices",
                newName: "SimCardCharge");

            migrationBuilder.RenameColumn(
                name: "phoneNumber",
                table: "Devices",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "deviceModel",
                table: "Devices",
                newName: "DeviceModel");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SimCardCharge",
                table: "Devices",
                newName: "simCardCharge");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Devices",
                newName: "phoneNumber");

            migrationBuilder.RenameColumn(
                name: "DeviceModel",
                table: "Devices",
                newName: "deviceModel");
        }
    }
}
