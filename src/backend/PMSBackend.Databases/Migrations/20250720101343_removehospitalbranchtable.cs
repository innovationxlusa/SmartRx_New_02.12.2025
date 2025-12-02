using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class removehospitalbranchtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_HospitalBranch_TestCenterBranchId",
                table: "Configuration_DiagnosisCenterWiseTest");

            migrationBuilder.DropTable(
                name: "Configuration_HospitalBranch");

            migrationBuilder.DropIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_TestCenterBranchId",
                table: "Configuration_DiagnosisCenterWiseTest");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Configuration_HospitalBranch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    BranchLocation = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CloseDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CloseTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    GoogleLocation = table.Column<string>(type: "nvarchar(3000)", nullable: true),
                    GoogleRating = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsMainBranch = table.Column<bool>(type: "bit", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    OpenDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    OpenTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    WebAddress = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Weekend = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    YearEstablished = table.Column<string>(type: "nvarchar(50)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_HospitalBranch", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_HospitalBranch_Configuration_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Configuration_City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_HospitalBranch_Configuration_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_HospitalBranch_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_HospitalBranch_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_TestCenterBranchId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "TestCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_HospitalBranch_CityId",
                table: "Configuration_HospitalBranch",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_HospitalBranch_CreatedById",
                table: "Configuration_HospitalBranch",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_HospitalBranch_HospitalId",
                table: "Configuration_HospitalBranch",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_HospitalBranch_ModifiedById",
                table: "Configuration_HospitalBranch",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_HospitalBranch_TestCenterBranchId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "TestCenterBranchId",
                principalTable: "Configuration_HospitalBranch",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
