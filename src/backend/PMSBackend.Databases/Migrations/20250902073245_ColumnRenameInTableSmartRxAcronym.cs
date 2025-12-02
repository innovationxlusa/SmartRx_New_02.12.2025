using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnRenameInTableSmartRxAcronym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Keyword",
                table: "Configuration_SmartRxAcronym");

            migrationBuilder.AddColumn<string>(
                name: "Elaboration",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Elaboration",
                table: "Configuration_SmartRxAcronym");

            migrationBuilder.AddColumn<string>(
                name: "Keyword",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "");
        }
    }
}
