using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnRemovedPatientInvestigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_HospitalBranch_TestCenterBranchId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterBranchId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropColumn(
                name: "TestCenterBranchId",
                table: "SmartRx_PatientInvestigation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "TestCenterBranchId",
                table: "SmartRx_PatientInvestigation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterBranchId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterBranchId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_HospitalBranch_TestCenterBranchId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterBranchId",
                principalTable: "Configuration_HospitalBranch",
                principalColumn: "Id");
        }
    }
}
