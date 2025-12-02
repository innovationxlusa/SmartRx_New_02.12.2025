using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAddedInPatientReward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConsumedMoney",
                table: "Smartrx_PatientReward");

            migrationBuilder.RenameColumn(
                name: "EncashMoney",
                table: "Smartrx_PatientReward",
                newName: "EncashedMoney");

            migrationBuilder.RenameColumn(
                name: "EarnedMoney",
                table: "Smartrx_PatientReward",
                newName: "ConvertedCashableToMoney");

            migrationBuilder.AddColumn<decimal>(
                name: "ConvertedCashableToNonCashablePoints",
                table: "Smartrx_PatientReward",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "ConvertedNonCashableToCashablePoints",
                table: "Smartrx_PatientReward",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConvertedCashableToNonCashablePoints",
                table: "Smartrx_PatientReward");

            migrationBuilder.DropColumn(
                name: "ConvertedNonCashableToCashablePoints",
                table: "Smartrx_PatientReward");

            migrationBuilder.RenameColumn(
                name: "EncashedMoney",
                table: "Smartrx_PatientReward",
                newName: "EncashMoney");

            migrationBuilder.RenameColumn(
                name: "ConvertedCashableToMoney",
                table: "Smartrx_PatientReward",
                newName: "EarnedMoney");

            migrationBuilder.AddColumn<decimal>(
                name: "ConsumedMoney",
                table: "Smartrx_PatientReward",
                type: "decimal(18,2)",
                nullable: true);
        }
    }
}
