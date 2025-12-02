using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardColumnAddedInBenefits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RewardId",
                table: "Configuration_RewardBenefit",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBenefit_RewardId",
                table: "Configuration_RewardBenefit",
                column: "RewardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_RewardBenefit_Configuration_Reward_RewardId",
                table: "Configuration_RewardBenefit",
                column: "RewardId",
                principalTable: "Configuration_Reward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_RewardBenefit_Configuration_Reward_RewardId",
                table: "Configuration_RewardBenefit");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_RewardBenefit_RewardId",
                table: "Configuration_RewardBenefit");

            migrationBuilder.DropColumn(
                name: "RewardId",
                table: "Configuration_RewardBenefit");
        }
    }
}
