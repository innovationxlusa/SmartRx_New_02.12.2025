using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardTablesNewlyAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsNegativePointAllowed",
                table: "Configuration_Reward",
                newName: "IsDeduction");

            migrationBuilder.AddColumn<decimal>(
                name: "Rate",
                table: "SmartRx_RewardPointConversions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SmartRx_RewardPointConversions",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "BadgeType",
                table: "Configuration_RewardBadge",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RequiredActivities",
                table: "Configuration_RewardBadge",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RequiredPoints",
                table: "Configuration_RewardBadge",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration_Reward_Rules",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActivityCode = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    ActivityName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    RewardType = table.Column<int>(type: "int", nullable: false),
                    RewardDetails = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeductible = table.Column<bool>(type: "bit", nullable: false),
                    Points = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Reward_Rules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Reward_Rules_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Reward_Rules_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_Reward_UserBalances",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    NonCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashedMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_Reward_UserBalances", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_UserBalances_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_UserBalances_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_UserBalances_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_Reward_Transactions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BadgeId = table.Column<long>(type: "bigint", nullable: false),
                    RewardRuleId = table.Column<long>(type: "bigint", nullable: false),
                    RewardType = table.Column<int>(type: "int", nullable: false),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: true),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    IsDeductPoints = table.Column<bool>(type: "bit", nullable: false),
                    AmountChanged = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NonCashableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashableBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashedMoneyBalance = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_Reward_Transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Configuration_RewardBadge_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Configuration_RewardBadge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Configuration_Reward_Rules_RewardRuleId",
                        column: x => x.RewardRuleId,
                        principalTable: "Configuration_Reward_Rules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Reward_Transactions_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_RewardPointConversions_UserId",
                table: "SmartRx_RewardPointConversions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Reward_Rules_CreatedById",
                table: "Configuration_Reward_Rules",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Reward_Rules_ModifiedById",
                table: "Configuration_Reward_Rules",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_BadgeId",
                table: "SmartRx_Reward_Transactions",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_CreatedById",
                table: "SmartRx_Reward_Transactions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_ModifiedById",
                table: "SmartRx_Reward_Transactions",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_PatientId",
                table: "SmartRx_Reward_Transactions",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_PrescriptionId",
                table: "SmartRx_Reward_Transactions",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_RewardRuleId",
                table: "SmartRx_Reward_Transactions",
                column: "RewardRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_SmartRxMasterId",
                table: "SmartRx_Reward_Transactions",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_Transactions_UserId",
                table: "SmartRx_Reward_Transactions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_UserBalances_CreatedById",
                table: "SmartRx_Reward_UserBalances",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_UserBalances_ModifiedById",
                table: "SmartRx_Reward_UserBalances",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Reward_UserBalances_UserId",
                table: "SmartRx_Reward_UserBalances",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_RewardPointConversions_Security_PMSUser_UserId",
                table: "SmartRx_RewardPointConversions",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_RewardPointConversions_Security_PMSUser_UserId",
                table: "SmartRx_RewardPointConversions");

            migrationBuilder.DropTable(
                name: "SmartRx_Reward_Transactions");

            migrationBuilder.DropTable(
                name: "SmartRx_Reward_UserBalances");

            migrationBuilder.DropTable(
                name: "Configuration_Reward_Rules");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_RewardPointConversions_UserId",
                table: "SmartRx_RewardPointConversions");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "SmartRx_RewardPointConversions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SmartRx_RewardPointConversions");

            migrationBuilder.DropColumn(
                name: "BadgeType",
                table: "Configuration_RewardBadge");

            migrationBuilder.DropColumn(
                name: "RequiredActivities",
                table: "Configuration_RewardBadge");

            migrationBuilder.DropColumn(
                name: "RequiredPoints",
                table: "Configuration_RewardBadge");

            migrationBuilder.RenameColumn(
                name: "IsDeduction",
                table: "Configuration_Reward",
                newName: "IsNegativePointAllowed");
        }
    }
}
