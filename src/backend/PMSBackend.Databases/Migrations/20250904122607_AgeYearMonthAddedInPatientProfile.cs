using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AgeYearMonthAddedInPatientProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AgeMonth",
                table: "SmartRx_PatientProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AgeYear",
                table: "SmartRx_PatientProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose9InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose8InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose7InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose6InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose5InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose4InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose3InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose2InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose1InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose12InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose11InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Dose10InADay",
                table: "SmartRx_PatientMedicine",
                type: "decimal(5,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AgeMonth",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "AgeYear",
                table: "SmartRx_PatientProfile");

            migrationBuilder.AlterColumn<string>(
                name: "Dose9InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose8InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose7InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose6InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose5InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose4InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose3InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose2InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose1InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose12InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose11InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.AlterColumn<string>(
                name: "Dose10InADay",
                table: "SmartRx_PatientMedicine",
                type: "nvarchar(50)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");
        }
    }
}
