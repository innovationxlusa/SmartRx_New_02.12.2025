using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class PatientVitalUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HeightFeet",
                table: "SmartRx_PatientVitals",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightInches",
                table: "SmartRx_PatientVitals",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HeightFeet",
                table: "SmartRx_PatientProfile",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "HeightInches",
                table: "SmartRx_PatientProfile",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeightFeet",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropColumn(
                name: "HeightInches",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropColumn(
                name: "HeightFeet",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "HeightInches",
                table: "SmartRx_PatientProfile");
        }
    }
}
