using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class nullableTableSmartRxAcronym : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)");

            migrationBuilder.AlterColumn<string>(
                name: "Acronym",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)");

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(500)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Details",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(2000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Acronym",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(100)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Abbreviation",
                table: "Configuration_SmartRxAcronym",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);
        }
    }
}
