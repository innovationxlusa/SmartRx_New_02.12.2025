using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardColumnAddedInPatientReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RewardId",
                table: "Smartrx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_RewardId",
                table: "Smartrx_PatientReward",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_Reward_RewardId",
                table: "Smartrx_PatientReward",
                column: "RewardId",
                principalTable: "Configuration_Reward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_Reward_RewardId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_Smartrx_PatientReward_RewardId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropColumn(
                name: "RewardId",
                table: "Smartrx_PatientReward");
        }
    }
}
