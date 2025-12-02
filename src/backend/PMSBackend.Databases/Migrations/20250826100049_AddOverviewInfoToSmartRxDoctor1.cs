using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddOverviewInfoToSmartRxDoctor1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConsultingDurationInMinutes",
                table: "SmartRx_PatientDoctor",
                type: "int",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ChamberWaitTimeMinute",
                table: "SmartRx_PatientDoctor",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TravelTimeMinute",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TravelTimeMinute",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.AlterColumn<decimal>(
                name: "ConsultingDurationInMinutes",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ChamberWaitTimeMinute",
                table: "SmartRx_PatientDoctor",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }
    }
}
