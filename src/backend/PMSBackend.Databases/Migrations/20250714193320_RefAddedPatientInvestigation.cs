using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RefAddedPatientInvestigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation",
                column: "PriceUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation",
                column: "PriceUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }
    }
}
