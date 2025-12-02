using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class AddOverviewInfoToSmartRxDoctor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "OtherExpense",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TransportExpense",
                table: "SmartRx_PatientDoctor",
                type: "decimal(18,2)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherExpense",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "TransportExpense",
                table: "SmartRx_PatientDoctor");
        }
    }
}
