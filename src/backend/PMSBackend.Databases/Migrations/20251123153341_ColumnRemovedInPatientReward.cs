using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnRemovedInPatientReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_UserId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_PatientId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_PrescriptionId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_SmartRxMasterId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_UserId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "PrescriptionId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "SmartRxMasterId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SmartRx_PatientReward");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "PrescriptionId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SmartRxMasterId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_PatientId",
                table: "SmartRx_PatientReward",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_PrescriptionId",
                table: "SmartRx_PatientReward",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_SmartRxMasterId",
                table: "SmartRx_PatientReward",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_UserId",
                table: "SmartRx_PatientReward",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "SmartRx_PatientReward",
                column: "PrescriptionId",
                principalTable: "Prescription_Upload",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_UserId",
                table: "SmartRx_PatientReward",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientReward",
                column: "SmartRxMasterId",
                principalTable: "SmartRx_Master",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientReward",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id");
        }
    }
}
