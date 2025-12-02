using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardColumnAddedAndUserActivityTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration_RewardBenefit");

            migrationBuilder.RenameColumn(
                name: "Heading",
                table: "Configuration_Reward",
                newName: "Title");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeductPoints",
                table: "SmartRx_PatientReward",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "UserActivityId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SmartRx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<double>(
                name: "NonCashablePoints",
                table: "Configuration_Reward",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "CashablePoints",
                table: "Configuration_Reward",
                type: "float",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<double>(
                name: "CashedMoney",
                table: "Configuration_Reward",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCashedMoney",
                table: "Configuration_Reward",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsVisibleToUser",
                table: "Configuration_Reward",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "RewardCode",
                table: "Configuration_Reward",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Configuration_UserActivity",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityCode = table.Column<string>(type: "nchar(10)", nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ActivityPoint = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_UserActivity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_UserActivity_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_UserActivity_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_UserActivityId",
                table: "SmartRx_PatientReward",
                column: "UserActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientReward_UserId",
                table: "SmartRx_PatientReward",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_UserActivity_CreatedById",
                table: "Configuration_UserActivity",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_UserActivity_ModifiedById",
                table: "Configuration_UserActivity",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_UserActivity_UserActivityId",
                table: "SmartRx_PatientReward",
                column: "UserActivityId",
                principalTable: "Configuration_UserActivity",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_UserId",
                table: "SmartRx_PatientReward",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_UserActivity_UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_UserId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropTable(
                name: "Configuration_UserActivity");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientReward_UserId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "IsDeductPoints",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "UserActivityId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropColumn(
                name: "CashedMoney",
                table: "Configuration_Reward");

            migrationBuilder.DropColumn(
                name: "IsCashedMoney",
                table: "Configuration_Reward");

            migrationBuilder.DropColumn(
                name: "IsVisibleToUser",
                table: "Configuration_Reward");

            migrationBuilder.DropColumn(
                name: "RewardCode",
                table: "Configuration_Reward");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Configuration_Reward",
                newName: "Heading");

            migrationBuilder.AlterColumn<int>(
                name: "NonCashablePoints",
                table: "Configuration_Reward",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CashablePoints",
                table: "Configuration_Reward",
                type: "int",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration_RewardBenefit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    RewardId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_RewardBenefit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_RewardBenefit_Configuration_Reward_RewardId",
                        column: x => x.RewardId,
                        principalTable: "Configuration_Reward",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_RewardBenefit_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_RewardBenefit_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBenefit_CreatedById",
                table: "Configuration_RewardBenefit",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBenefit_ModifiedById",
                table: "Configuration_RewardBenefit",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBenefit_RewardId",
                table: "Configuration_RewardBenefit",
                column: "RewardId");
        }
    }
}
