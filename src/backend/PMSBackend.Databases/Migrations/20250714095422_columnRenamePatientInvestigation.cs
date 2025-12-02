using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnRenamePatientInvestigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDoctorRecommendedTheTestCenter",
                table: "SmartRx_PatientInvestigation",
                newName: "IsDoctorRecommendedTestCenter");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDoctorRecommendedTestCenter",
                table: "SmartRx_PatientInvestigation",
                newName: "IsDoctorRecommendedTheTestCenter");
        }
    }
}
