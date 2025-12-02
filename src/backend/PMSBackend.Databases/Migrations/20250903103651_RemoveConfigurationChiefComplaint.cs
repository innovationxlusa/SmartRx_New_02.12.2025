using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RemoveConfigurationChiefComplaint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SmartRx_PatientChiefComplaint_Configuration_ChiefComplaint_ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint");

            migrationBuilder.DropIndex(
                name: "IX_SmartRx_PatientChiefComplaint_ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint");

            migrationBuilder.DropColumn(
                name: "ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "SmartRx_PatientChiefComplaint",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "SmartRx_PatientChiefComplaint");

            migrationBuilder.AddColumn<long>(
                name: "ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint",
                column: "ChiefComplaintId");

            migrationBuilder.AddForeignKey(
                name: "FK_SmartRx_PatientChiefComplaint_Configuration_ChiefComplaint_ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint",
                column: "ChiefComplaintId",
                principalTable: "Configuration_ChiefComplaint",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
