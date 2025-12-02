using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RemovedColumnAddedInPatientReward2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_RewardBadge_Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.AddColumn<long>(
                name: "BadgeId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RewardId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_BadgeId",
                table: "SmartRx_PatientReward",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_RewardId",
                table: "SmartRx_PatientReward",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_PatientReward",
                column: "BadgeId",
                principalTable: "Configuration_RewardBadge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_Reward_RewardId",
                table: "SmartRx_PatientReward",
                column: "RewardId",
                principalTable: "Configuration_Reward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_Reward_RewardId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_BadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_RewardId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "BadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "RewardId",
                table: "SmartRx_PatientReward");

            migrationBuilder.AddColumn<long>(
                name: "Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward",
                column: "Configuration_RewardBadgeId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_RewardBadge_Configuration_RewardBadgeId",
                table: "SmartRx_PatientReward",
                column: "Configuration_RewardBadgeId",
                principalTable: "Configuration_RewardBadge",
                principalColumn: "Id");
        }
    }
}
