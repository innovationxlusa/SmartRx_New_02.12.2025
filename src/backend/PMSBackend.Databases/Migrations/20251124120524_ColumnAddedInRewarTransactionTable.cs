using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAddedInRewarTransactionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SmartRx_UserRewardBadge",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    BadgeId = table.Column<long>(type: "bigint", nullable: false),
                    EarnedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_UserRewardBadge", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_UserRewardBadge_Configuration_RewardBadge_BadgeId",
                        column: x => x.BadgeId,
                        principalTable: "Configuration_RewardBadge",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_UserRewardBadge_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_UserRewardBadge_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_UserRewardBadge_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_UserRewardBadge_BadgeId",
                table: "SmartRx_UserRewardBadge",
                column: "BadgeId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_UserRewardBadge_CreatedById",
                table: "SmartRx_UserRewardBadge",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_UserRewardBadge_ModifiedById",
                table: "SmartRx_UserRewardBadge",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_UserRewardBadge_UserId",
                table: "SmartRx_UserRewardBadge",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SmartRx_UserRewardBadge");
        }
    }
}
