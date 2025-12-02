using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnUpdatedPatientInvestigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Hospital_TestCenterId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropColumn(
                name: "TestCenterId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.AddColumn<string>(
                name: "TestCenterIds",
                table: "SmartRx_PatientInvestigation",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TestCenterIds",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.AddColumn<long>(
                name: "TestCenterId",
                table: "SmartRx_PatientInvestigation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Hospital_TestCenterId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterId",
                principalTable: "Configuration_Hospital",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
