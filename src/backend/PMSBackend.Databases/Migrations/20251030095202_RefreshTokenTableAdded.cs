using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class RefreshTokenTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Security_RefreshToken",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Security_RefreshToken_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Security_RefreshToken_UserId",
                table: "Security_RefreshToken",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Security_RefreshToken");
        }
    }
}
