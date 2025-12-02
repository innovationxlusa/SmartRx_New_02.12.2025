using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class BadgeIdRequiredRemovedInRewarTansactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_Reward_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "BadgeId",
                table: "SmartRx_Reward_Transactions",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "BadgeId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_Reward_Transactions",
                column: "BadgeId",
                principalTable: "Configuration_RewardBadge",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_Reward_Transactions");

            migrationBuilder.AlterColumn<long>(
                name: "BadgeId",
                table: "SmartRx_Reward_Transactions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "BadgeId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_Reward_Transactions_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_Reward_Transactions",
                column: "BadgeId",
                principalTable: "Configuration_RewardBadge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
