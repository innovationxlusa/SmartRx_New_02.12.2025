using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnAddedInRewardBadge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward");

            migrationBuilder.AlterColumn<long>(
                name: "UserId",
                table: "Smartrx_PatientReward",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Configuration_RewardBadge",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_UserId",
                table: "Smartrx_PatientReward",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_UserId",
                table: "Smartrx_PatientReward",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_UserId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_Smartrx_PatientReward_UserId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Configuration_RewardBadge");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Smartrx_PatientReward",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
