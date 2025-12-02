using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class SmartRxDoctorUpdated1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_Vital_Configuration_Unit_Configuration_UnitEntityId",
                table: "Configuration_Vital");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_Vital_Configuration_UnitEntityId",
                table: "Configuration_Vital");

            migrationBuilder.DropColumn(
                name: "Configuration_UnitEntityId",
                table: "Configuration_Vital");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Configuration_UnitEntityId",
                table: "Configuration_Vital",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Vital_Configuration_UnitEntityId",
                table: "Configuration_Vital",
                column: "Configuration_UnitEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_Vital_Configuration_Unit_Configuration_UnitEntityId",
                table: "Configuration_Vital",
                column: "Configuration_UnitEntityId",
                principalTable: "Configuration_Unit",
                principalColumn: "Id");
        }
    }
}
