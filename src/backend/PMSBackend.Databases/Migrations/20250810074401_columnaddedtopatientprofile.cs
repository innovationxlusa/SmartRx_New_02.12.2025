using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnaddedtopatientprofile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "SmartRx_PatientVitals",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_PatientId",
                table: "SmartRx_PatientVitals",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientVitals_SmartRx_PatientProfile_PatientId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientVitals_PatientId",
                table: "SmartRx_PatientVitals");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "SmartRx_PatientVitals");
        }
    }
}
