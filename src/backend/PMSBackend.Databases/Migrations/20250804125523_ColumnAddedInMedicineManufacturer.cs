using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAddedInMedicineManufacturer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyUrl",
                table: "Configuration_Medicine");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUrl",
                table: "Configuration_MedicineManufactureInfo",
                type: "nvarchar(200)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyUrl",
                table: "Configuration_MedicineManufactureInfo");

            migrationBuilder.AddColumn<string>(
                name: "CompanyUrl",
                table: "Configuration_Medicine",
                type: "nvarchar(200)",
                nullable: true);
        }
    }
}
