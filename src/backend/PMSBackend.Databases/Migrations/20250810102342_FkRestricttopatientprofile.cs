using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class FkRestricttopatientprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    UPDATE [SmartRx_PatientVitals]
                    SET PatientId = 1
                    WHERE PatientId IS NULL
                ");
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientVitals",
                column: "SmartRxMasterId",
                principalTable: "SmartRx_Master",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientVitals",
                column: "SmartRxMasterId",
                principalTable: "SmartRx_Master",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
