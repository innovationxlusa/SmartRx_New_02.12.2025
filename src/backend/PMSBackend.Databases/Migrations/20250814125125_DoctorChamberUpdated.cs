using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DoctorChamberUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_DoctorChamber_Configuration_Chamber_ChamberId",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropTable(
                name: "Configuration_Chamber");

            migrationBuilder.RenameColumn(
                name: "ChamberId",
                table: "Configuration_DoctorChamber",
                newName: "CityId");

            migrationBuilder.RenameIndex(
                name: "IX_Configuration_DoctorChamber_ChamberId",
                table: "Configuration_DoctorChamber",
                newName: "IX_Configuration_DoctorChamber_CityId");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)");

            migrationBuilder.AddColumn<string>(
                name: "ChamberAddress",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(1500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<long>(
                name: "ChamberCityId",
                table: "Configuration_DoctorChamber",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "ChamberClosedOnDay",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(100)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberDescription",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberEmail",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberEndTime",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberGoogleAddress",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberGoogleLocationLink",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(2000)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberGoogleRating",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(5)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberName",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(500)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberOtherDoctorsId",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberOverseasCaller",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(50)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberPostalCode",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(20)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberStartTime",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberType",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ChamberVisitingHours",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(10)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberWhatsAppNumber",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(25)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "DoctorBookingMobileNos",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Helpline_CallCenter",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(15)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_DoctorChamber_Configuration_City_CityId",
                table: "Configuration_DoctorChamber",
                column: "CityId",
                principalTable: "Configuration_City",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Configuration_DoctorChamber_Configuration_City_CityId",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberAddress",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberCityId",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberClosedOnDay",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberDescription",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberEmail",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberEndTime",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberGoogleAddress",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberGoogleLocationLink",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberGoogleRating",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberName",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberOtherDoctorsId",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberOverseasCaller",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberPostalCode",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberStartTime",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberType",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberVisitingHours",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "ChamberWhatsAppNumber",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "DoctorBookingMobileNos",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "Helpline_CallCenter",
                table: "Configuration_DoctorChamber");

            migrationBuilder.RenameColumn(
                name: "CityId",
                table: "Configuration_DoctorChamber",
                newName: "ChamberId");

            migrationBuilder.RenameIndex(
                name: "IX_Configuration_DoctorChamber_CityId",
                table: "Configuration_DoctorChamber",
                newName: "IX_Configuration_DoctorChamber_ChamberId");

            migrationBuilder.AlterColumn<string>(
                name: "Remarks",
                table: "Configuration_DoctorChamber",
                type: "nvarchar(1500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(500)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration_Chamber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(1500)", nullable: false),
                    ChamberEndTime = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    ChamberStartTime = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    ClosedOnDay = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Code = table.Column<string>(type: "nchar(10)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    DoctorBookingMobileNos = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    GoogleAddress = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    GoogleLocationLink = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    GoogleRating = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    Helpline_CallCenter = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    OtherDoctorsId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    OverseasCaller = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    VisitingHours = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(25)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Chamber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Chamber_Configuration_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Configuration_City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_Chamber_Configuration_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Configuration_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_Chamber_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Chamber_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Chamber_CityId",
                table: "Configuration_Chamber",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Chamber_CreatedById",
                table: "Configuration_Chamber",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Chamber_DepartmentId",
                table: "Configuration_Chamber",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Chamber_ModifiedById",
                table: "Configuration_Chamber",
                column: "ModifiedById");

            migrationBuilder.AddForeignKey(
                name: "FK_Configuration_DoctorChamber_Configuration_Chamber_ChamberId",
                table: "Configuration_DoctorChamber",
                column: "ChamberId",
                principalTable: "Configuration_Chamber",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
