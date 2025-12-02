using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class ColumnAddedInRewardBadge1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Heirarchy",
                table: "Configuration_RewardBadge",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Heirarchy",
                table: "Configuration_RewardBadge");
        }
    }
}
