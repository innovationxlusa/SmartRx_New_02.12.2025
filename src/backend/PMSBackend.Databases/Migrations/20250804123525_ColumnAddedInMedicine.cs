using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAddedInMedicine : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyDiscount",
                table: "Configuration_Medicine");

            migrationBuilder.AlterColumn<string>(
                name: "UserFor",
                table: "Configuration_Medicine",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TherapeuticClass",
                table: "Configuration_Medicine",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageCondition",
                table: "Configuration_Medicine",
                type: "nvarchar(300)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OverdoseEffects",
                table: "Configuration_Medicine",
                type: "nvarchar(200)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Interaction",
                table: "Configuration_Medicine",
                type: "nvarchar(100)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CompanyDiscountPercentage",
                table: "Configuration_Medicine",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyUrl",
                table: "Configuration_Medicine",
                type: "nvarchar(200)",
                nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsActive",
            //    table: "Configuration_DoctorChamber",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyDiscountPercentage",
                table: "Configuration_Medicine");

            migrationBuilder.DropColumn(
                name: "CompanyUrl",
                table: "Configuration_Medicine");

            //migrationBuilder.DropColumn(
            //    name: "IsActive",
            //    table: "Configuration_DoctorChamber");

            migrationBuilder.AlterColumn<string>(
                name: "UserFor",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "TherapeuticClass",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "StorageCondition",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(300)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "OverdoseEffects",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Interaction",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CompanyDiscount",
                table: "Configuration_Medicine",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
