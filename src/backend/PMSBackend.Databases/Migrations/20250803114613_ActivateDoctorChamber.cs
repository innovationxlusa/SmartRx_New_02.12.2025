using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ActivateDoctorChamber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MajorSubject",
                table: "Configuration_Education");

            migrationBuilder.DropColumn(
                name: "PassingYear",
                table: "Configuration_Education");

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "Configuration_DoctorChamber",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "Configuration_DoctorChamber");

            migrationBuilder.AddColumn<string>(
                name: "MajorSubject",
                table: "Configuration_Education",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PassingYear",
                table: "Configuration_Education",
                type: "int",
                nullable: true);
        }
    }
}
