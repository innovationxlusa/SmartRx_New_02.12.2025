using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RemoveUserIdInPatientReward1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_UserId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropIndex(
                name: "IX_Smartrx_PatientReward_UserId",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Smartrx_PatientReward");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "UserId",
                table: "Smartrx_PatientReward",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_Smartrx_PatientReward_UserId",
                table: "Smartrx_PatientReward",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Smartrx_PatientReward_Security_PMSUser_UserId",
                table: "Smartrx_PatientReward",
                column: "UserId",
                principalTable: "Security_PMSUser",
                principalColumn: "Id");
        }
    }
}
