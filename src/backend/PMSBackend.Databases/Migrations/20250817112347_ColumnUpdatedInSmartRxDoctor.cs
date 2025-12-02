using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnUpdatedInSmartRxDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChamberWaitTime",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "ConsultingDuration",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.AlterColumn<decimal>(
                name: "DoctorRating",
                table: "SmartRx_PatientDoctor",
                type: "decimal(5,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<decimal>(
                name: "ChamberFee",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)");

            migrationBuilder.AddColumn<long>(
                name: "ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberWaitTimeHour",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberWaitTimeMinute",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "ConsultingDurationInMinutes",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor",
                column: "ChamberFeeMeasurementUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor",
                column: "ChamberFeeMeasurementUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientDoctor_Configuration_Unit_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientDoctor_ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "ChamberFeeMeasurementUnitId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "ChamberWaitTimeHour",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "ChamberWaitTimeMinute",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "ConsultingDurationInMinutes",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.AlterColumn<decimal>(
                name: "DoctorRating",
                table: "SmartRx_PatientDoctor",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comments",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChamberFee",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberWaitTime",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(10)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ConsultingDuration",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
