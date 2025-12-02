using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DoctorImagePathAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoName",
                table: "Configuration_Doctor",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoPath",
                table: "Configuration_Doctor",
                type: "nvarchar(2000)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoName",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "ProfilePhotoPath",
                table: "Configuration_Doctor");
        }
    }
}
