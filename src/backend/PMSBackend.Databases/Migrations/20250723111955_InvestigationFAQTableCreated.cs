using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InvestigationFAQTableCreated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuration_InvestigationFAQ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvestigationId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_InvestigationFAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_InvestigationFAQ_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_InvestigationFAQ_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_InvestigationFAQ_CreatedById",
                table: "Configuration_InvestigationFAQ",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_InvestigationFAQ_ModifiedById",
                table: "Configuration_InvestigationFAQ",
                column: "ModifiedById");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration_InvestigationFAQ");
        }
    }
}
