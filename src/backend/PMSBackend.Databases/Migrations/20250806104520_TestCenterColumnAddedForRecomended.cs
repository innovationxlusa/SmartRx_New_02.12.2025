using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class TestCenterColumnAddedForRecomended : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDoctorRecommendedTestCenter",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.RenameColumn(
                name: "TestCenterIds",
                table: "SmartRx_PatientInvestigation",
                newName: "UserSelectedTestCenterIds");

            migrationBuilder.AddColumn<string>(
                name: "DoctorRecommendedTestCenterIds",
                table: "SmartRx_PatientInvestigation",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DoctorRecommendedTestCenterIds",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.RenameColumn(
                name: "UserSelectedTestCenterIds",
                table: "SmartRx_PatientInvestigation",
                newName: "TestCenterIds");

            migrationBuilder.AddColumn<bool>(
                name: "IsDoctorRecommendedTestCenter",
                table: "SmartRx_PatientInvestigation",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
