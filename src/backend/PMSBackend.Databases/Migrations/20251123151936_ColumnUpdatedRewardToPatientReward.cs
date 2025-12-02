using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnUpdatedRewardToPatientReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_UserActivity_UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.AddColumn<long>(
                name: "PresentRewardBadgeId",
                table: "Configuration_Settings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Configuration_Reward",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<string>(
                name: "RewardCode",
                table: "Configuration_Reward",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Configuration_Reward",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserActivityId",
                table: "Configuration_Reward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Reward_UserActivityId",
                table: "Configuration_Reward",
                column: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_Reward_Configuration_UserActivity_UserActivityId",
                table: "Configuration_Reward",
                column: "UserActivityId",
                principalTable: "Configuration_UserActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_Reward_Configuration_UserActivity_UserActivityId",
                table: "Configuration_Reward");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_Reward_UserActivityId",
                table: "Configuration_Reward");

            migrationBuilder.DropColumn(
                name: "PresentRewardBadgeId",
                table: "Configuration_Settings");

            migrationBuilder.DropColumn(
                name: "UserActivityId",
                table: "Configuration_Reward");

            migrationBuilder.AddColumn<long>(
                name: "UserActivityId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                table: "Configuration_Reward",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "RewardCode",
                table: "Configuration_Reward",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Configuration_Reward",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1000)",
                oldMaxLength: 1000,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_UserActivityId",
                table: "SmartRx_PatientReward",
                column: "UserActivityId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_UserActivity_UserActivityId",
                table: "SmartRx_PatientReward",
                column: "UserActivityId",
                principalTable: "Configuration_UserActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
