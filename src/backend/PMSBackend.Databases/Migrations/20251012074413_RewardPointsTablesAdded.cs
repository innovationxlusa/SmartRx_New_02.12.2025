using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RewardPointsTablesAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<long>(
            //    name: "UserId",
            //    table: "SmartRx_PatientProfile",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            //migrationBuilder.AddColumn<DateTime>(
            //    name: "PrescriptionDate",
            //    table: "Prescription_Upload",
            //    type: "datetime2",
            //    nullable: false,
            //    defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "Configuration_Reward",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Heading = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    IsNegativePointAllowed = table.Column<bool>(type: "bit", nullable: false),
                    NonCashablePoints = table.Column<int>(type: "int", nullable: false),
                    IsCashable = table.Column<bool>(type: "bit", nullable: false),
                    CashablePoints = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Reward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Reward_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Reward_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_RewardBadge",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_RewardBadge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_RewardBadge_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_RewardBadge_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            //migrationBuilder.CreateTable(
            //    name: "SmartRx_PatientOtherExpense",
            //    columns: table => new
            //    {
            //        Id = table.Column<long>(type: "bigint", nullable: false)
            //            .Annotation("SqlServer:Identity", "1, 1"),
            //        SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
            //        PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
            //        ExpenseName = table.Column<string>(type: "nvarchar(200)", nullable: false),
            //        Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
            //        Amount = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
            //        CurrencyUnitId = table.Column<long>(type: "bigint", nullable: true),
            //        ExpenseDate = table.Column<DateTime>(type: "datetime2", nullable: false),
            //        ExpenseNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
            //        CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        CreatedById = table.Column<long>(type: "bigint", nullable: true),
            //        ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
            //        ModifiedById = table.Column<long>(type: "bigint", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_SmartRx_PatientOtherExpense", x => x.Id);
            //        table.ForeignKey(
            //            name: "FK_SmartRx_PatientOtherExpense_Configuration_Unit_CurrencyUnitId",
            //            column: x => x.CurrencyUnitId,
            //            principalTable: "Configuration_Unit",
            //            principalColumn: "Id");
            //        table.ForeignKey(
            //            name: "FK_SmartRx_PatientOtherExpense_Prescription_Upload_PrescriptionId",
            //            column: x => x.PrescriptionId,
            //            principalTable: "Prescription_Upload",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_SmartRx_PatientOtherExpense_Security_PMSUser_CreatedById",
            //            column: x => x.CreatedById,
            //            principalTable: "Security_PMSUser",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_SmartRx_PatientOtherExpense_Security_PMSUser_ModifiedById",
            //            column: x => x.ModifiedById,
            //            principalTable: "Security_PMSUser",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //        table.ForeignKey(
            //            name: "FK_SmartRx_PatientOtherExpense_SmartRx_Master_SmartRxMasterId",
            //            column: x => x.SmartRxMasterId,
            //            principalTable: "SmartRx_Master",
            //            principalColumn: "Id",
            //            onDelete: ReferentialAction.Restrict);
            //    });

            migrationBuilder.CreateTable(
                name: "Smartrx_PatientReward",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: true),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: true),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    BadgeId = table.Column<long>(type: "bigint", nullable: false),
                    EarnedNonCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumedNonCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalNonCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EarnedCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ConsumedCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalCashablePoints = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EarnedMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    ConsumedMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    EncashMoney = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Smartrx_PatientReward", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_Configuration_RewardBadge_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Configuration_RewardBadge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Smartrx_PatientReward_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientProfile_UserId",
            //    table: "SmartRx_PatientProfile",
            //    column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Reward_CreatedById",
                table: "Configuration_Reward",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Reward_ModifiedById",
                table: "Configuration_Reward",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBadge_CreatedById",
                table: "Configuration_RewardBadge",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_RewardBadge_ModifiedById",
                table: "Configuration_RewardBadge",
                column: "ModifiedById");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientOtherExpense_CreatedById",
            //    table: "SmartRx_PatientOtherExpense",
            //    column: "CreatedById");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientOtherExpense_CurrencyUnitId",
            //    table: "SmartRx_PatientOtherExpense",
            //    column: "CurrencyUnitId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientOtherExpense_ModifiedById",
            //    table: "SmartRx_PatientOtherExpense",
            //    column: "ModifiedById");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientOtherExpense_PrescriptionId",
            //    table: "SmartRx_PatientOtherExpense",
            //    column: "PrescriptionId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientOtherExpense_SmartRxMasterId",
            //    table: "SmartRx_PatientOtherExpense",
            //    column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_BadgeId",
                table: "Smartrx_PatientReward",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_CreatedById",
                table: "Smartrx_PatientReward",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_ModifiedById",
                table: "Smartrx_PatientReward",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_PatientId",
                table: "Smartrx_PatientReward",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_PrescriptionId",
                table: "Smartrx_PatientReward",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_SmartRxMasterId",
                table: "Smartrx_PatientReward",
                column: "SmartRxMasterId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Security_PMSUser_UserId",
            //    table: "SmartRx_PatientProfile",
            //    column: "UserId",
            //    principalTable: "Security_PMSUser",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Security_PMSUser_UserId",
            //    table: "SmartRx_PatientProfile");

            migrationBuilder.DropTable(
                name: "Configuration_Reward");

            //migrationBuilder.DropTable(
            //    name: "SmartRx_PatientOtherExpense");

            migrationBuilder.DropTable(
                name: "Smartrx_PatientReward");

            migrationBuilder.DropTable(
                name: "Configuration_RewardBadge");

            //migrationBuilder.DropIndex(
            //    name: "IX_SmartRx_PatientProfile_UserId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "UserId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "PrescriptionDate",
            //    table: "Prescription_Upload");
        }
    }
}
