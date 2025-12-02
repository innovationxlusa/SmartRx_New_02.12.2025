using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class PriceUnitColumnAddedPatientInvestigation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "TestPrice",
                table: "SmartRx_PatientInvestigation",
                type: "decimal(18,2)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<long>(
                name: "PriceUnitId",
                table: "SmartRx_PatientInvestigation",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_PriceUnitId",
                table: "SmartRx_PatientInvestigation",
                column: "PriceUnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation",
                column: "PriceUnitId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientInvestigation_Configuration_Unit_PriceUnitId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientInvestigation_PriceUnitId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.DropColumn(
                name: "PriceUnitId",
                table: "SmartRx_PatientInvestigation");

            migrationBuilder.AlterColumn<decimal>(
                name: "TestPrice",
                table: "SmartRx_PatientInvestigation",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)",
                oldNullable: true);
        }
    }
}
