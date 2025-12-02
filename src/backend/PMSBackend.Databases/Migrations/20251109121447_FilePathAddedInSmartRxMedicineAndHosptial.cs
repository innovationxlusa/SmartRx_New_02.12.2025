using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class FilePathAddedInSmartRxMedicineAndHosptial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Configuration_MedicineManufactureInfo",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Configuration_MedicineManufactureInfo",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Configuration_MedicineManufactureInfo",
                type: "nvarchar(1000)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileExtension",
                table: "Configuration_Hospital",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "Configuration_Hospital",
                type: "nvarchar(300)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FilePath",
                table: "Configuration_Hospital",
                type: "nvarchar(1000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Configuration_MedicineManufactureInfo");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Configuration_MedicineManufactureInfo");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Configuration_MedicineManufactureInfo");

            migrationBuilder.DropColumn(
                name: "FileExtension",
                table: "Configuration_Hospital");

            migrationBuilder.DropColumn(
                name: "FileName",
                table: "Configuration_Hospital");

            migrationBuilder.DropColumn(
                name: "FilePath",
                table: "Configuration_Hospital");
        }
    }
}
