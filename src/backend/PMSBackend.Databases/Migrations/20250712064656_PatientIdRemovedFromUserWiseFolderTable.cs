using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class PatientIdRemovedFromUserWiseFolderTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Prescription_UserWiseFolder_SmartRx_PatientProfile_PatientId",
                table: "Prescription_UserWiseFolder");

            migrationBuilder.DropIndex(
                name: "IX_Prescription_UserWiseFolder_PatientId",
                table: "Prescription_UserWiseFolder");

            migrationBuilder.DropColumn(
                name: "PatientId",
                table: "Prescription_UserWiseFolder");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "PatientId",
                table: "Prescription_UserWiseFolder",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_PatientId",
                table: "Prescription_UserWiseFolder",
                column: "PatientId");

            migrationBuilder.AddForeignKey(
                name: "FK_Prescription_UserWiseFolder_SmartRx_PatientProfile_PatientId",
                table: "Prescription_UserWiseFolder",
                column: "PatientId",
                principalTable: "SmartRx_PatientProfile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
