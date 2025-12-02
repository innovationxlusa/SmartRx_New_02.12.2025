using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class SmartRxDoctorUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor",
                column: "ChamberFeeMeasurementUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor",
                column: "ChamberFeeMeasurementUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }
    }
}
