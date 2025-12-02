using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddedUserIdInPatientProfile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "SmartRx_PatientProfile",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_UserId",
                table: "SmartRx_PatientProfile",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientProfile_Security_PMSUser_UserId",
                table: "SmartRx_PatientProfile",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientProfile_Security_PMSUser_UserId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientProfile_UserId",
                table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "SmartRx_PatientProfile");
        }
    }
}
