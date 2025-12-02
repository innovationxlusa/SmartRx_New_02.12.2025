using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ForeignKeyRemovedFromRewardTransaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Prescription_Upload_PrescriptionId",
                table: "SmartRx_Reward_Transactions");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_Reward_Transactions_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_Reward_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_Reward_Transactions_PatientId",
                table: "SmartRx_Reward_Transactions");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_Reward_Transactions_PrescriptionId",
                table: "SmartRx_Reward_Transactions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_PatientId",
                table: "SmartRx_Reward_Transactions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_PrescriptionId",
                table: "SmartRx_Reward_Transactions",
                column: "PrescriptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Prescription_Upload_PrescriptionId",
                table: "SmartRx_Reward_Transactions",
                column: "PrescriptionId",
                principalTable: "Prescription_Upload",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_Reward_Transactions_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_Reward_Transactions",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id");
        }
    }
}
