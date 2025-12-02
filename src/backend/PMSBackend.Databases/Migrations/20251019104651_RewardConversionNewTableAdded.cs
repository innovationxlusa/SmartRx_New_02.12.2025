using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardConversionNewTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_RewardBadge_BadgeId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_Reward_RewardId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_CreatedById",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_ModifiedById",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_Master_SmartRxMasterId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Smartrx_PatientReward",
                table: "Smartrx_PatientReward");

            migrationBuilder.RenameTable(
                name: "Smartrx_PatientReward",
                newName: "SmartRx_PatientReward");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_SmartRxMasterId",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_SmartRxMasterId");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_RewardId",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_RewardId");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_PrescriptionId",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_PatientId",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_ModifiedById",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_CreatedById",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_Smartrx_PatientReward_BadgeId",
                table: "SmartRx_PatientReward",
                newName: "IX_SmartRx_PatientReward_BadgeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SmartRx_PatientReward",
                table: "SmartRx_PatientReward",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SmartRx_RewardPointConversions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FromType = table.Column<int>(type: "int", nullable: false),
                    ToType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_RewardPointConversions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_RewardPointConversions_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_RewardPointConversions_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_RewardPointConversions_CreatedById",
                table: "SmartRx_RewardPointConversions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_RewardPointConversions_ModifiedById",
                table: "SmartRx_RewardPointConversions",
                column: "ModifiedById");

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

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "SmartRx_PatientReward",
                column: "PrescriptionId",
                principalTable: "Prescription_Upload",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_CreatedById",
                table: "SmartRx_PatientReward",
                column: "CreatedById",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_ModifiedById",
                table: "SmartRx_PatientReward",
                column: "ModifiedById",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_RewardBadge_BadgeId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Configuration_Reward_RewardId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_CreatedById",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_Security_PMSUser_ModifiedById",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_Master_SmartRxMasterId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientReward");

            migrationBuilder.DropTable(
                name: "SmartRx_RewardPointConversions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SmartRx_PatientReward",
                table: "SmartRx_PatientReward");

            migrationBuilder.RenameTable(
                name: "SmartRx_PatientReward",
                newName: "Smartrx_PatientReward");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_SmartRxMasterId",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_SmartRxMasterId");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_RewardId",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_RewardId");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_PrescriptionId",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_PrescriptionId");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_PatientId",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_PatientId");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_ModifiedById",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_ModifiedById");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_CreatedById",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_CreatedById");

            migrationBuilder.RenameIndex(
                name: "IX_SmartRx_PatientReward_BadgeId",
                table: "Smartrx_PatientReward",
                newName: "IX_Smartrx_PatientReward_BadgeId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Smartrx_PatientReward",
                table: "Smartrx_PatientReward",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_RewardBadge_BadgeId",
                table: "Smartrx_PatientReward",
                column: "BadgeId",
                principalTable: "Configuration_RewardBadge",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Configuration_Reward_RewardId",
                table: "Smartrx_PatientReward",
                column: "RewardId",
                principalTable: "Configuration_Reward",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Prescription_Upload_PrescriptionId",
                table: "Smartrx_PatientReward",
                column: "PrescriptionId",
                principalTable: "Prescription_Upload",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_CreatedById",
                table: "Smartrx_PatientReward",
                column: "CreatedById",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_ModifiedById",
                table: "Smartrx_PatientReward",
                column: "ModifiedById",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_Master_SmartRxMasterId",
                table: "Smartrx_PatientReward",
                column: "SmartRxMasterId",
                principalTable: "SmartRx_Master",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                table: "Smartrx_PatientReward",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id");
        }
    }
}
