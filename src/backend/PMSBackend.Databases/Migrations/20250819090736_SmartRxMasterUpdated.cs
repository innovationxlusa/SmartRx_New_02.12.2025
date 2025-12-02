using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class SmartRxMasterUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Weight",
                table: "SmartRx_PatientProfile",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrescriptionId",
                table: "SmartRx_Master",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                column: "HeightMeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                column: "WeightMeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_PrescriptionId",
                table: "SmartRx_Master",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_Master_Prescription_Upload_PrescriptionId",
                table: "SmartRx_Master",
                column: "PrescriptionId",
                principalTable: "Prescription_Upload",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientProfile_Configuration_Unit_HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                column: "HeightMeasurementUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientProfile_Configuration_Unit_WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile",
                column: "WeightMeasurementUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_Master_Prescription_Upload_PrescriptionId",
                table: "SmartRx_Master");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientProfile_Configuration_Unit_HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientProfile_Configuration_Unit_WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientProfile_HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientProfile_WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_Master_PrescriptionId",
                table: "SmartRx_Master");

            migrationBuilder.DropColumn(
                name: "HeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "Weight",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "WeightMeasurementUnitId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "SmartRx_Master");
        }
    }
}
