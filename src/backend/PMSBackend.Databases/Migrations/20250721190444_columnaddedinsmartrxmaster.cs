using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class columnaddedinsmartrxmaster : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HasInvestigationFavourite",
                table: "SmartRx_Master",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "HasMedicineFavourite",
                table: "SmartRx_Master",
                type: "bit",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HasInvestigationFavourite",
                table: "SmartRx_Master");

            migrationBuilder.DropColumn(
                name: "HasMedicineFavourite",
                table: "SmartRx_Master");
        }
    }
}
