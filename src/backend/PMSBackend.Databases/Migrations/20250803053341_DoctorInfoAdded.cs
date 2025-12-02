using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class DoctorInfoAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Configuration_Education_Configuration_Doctor_Configuration_DoctorEntityId",
            //    table: "Configuration_Education");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Configuration_Country_CountryId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Configuration_District_DistrictId",
            //    table: "SmartRx_PatientProfile");

            migrationBuilder.DropTable(
                name: "Configuration_DoctorEducation");

            //migrationBuilder.DropIndex(
            //    name: "IX_SmartRx_PatientProfile_CountryId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropIndex(
            //    name: "IX_Configuration_Education_Configuration_DoctorEntityId",
            //    table: "Configuration_Education");

            //migrationBuilder.DropColumn(
            //    name: "CountryId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "Configuration_DoctorEntityId",
            //    table: "Configuration_Education");

            //migrationBuilder.RenameColumn(
            //    name: "DistrictId",
            //    table: "SmartRx_PatientProfile",
            //    newName: "RelatedToPatientId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_SmartRx_PatientProfile_DistrictId",
            //    table: "SmartRx_PatientProfile",
            //    newName: "IX_SmartRx_PatientProfile_RelatedToPatientId");

            migrationBuilder.RenameColumn(
                name: "ConsultingTime",
                table: "SmartRx_PatientDoctor",
                newName: "ConsultingDuration");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Configuration_Education",
                newName: "DegreeName");

            migrationBuilder.RenameColumn(
                name: "ChamberRemarks",
                table: "Configuration_DoctorChamber",
                newName: "Remarks");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Configuration_Doctor",
                newName: "Title");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SmartRx_PatientProfile",
                type: "nvarchar(2000)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)");

            //migrationBuilder.AddColumn<long>(
            //    name: "ExistingPatientId",
            //    table: "SmartRx_PatientProfile",
            //    type: "bigint",
            //    nullable: true);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsExistingPatient",
            //    table: "SmartRx_PatientProfile",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<bool>(
            //    name: "IsRelative",
            //    table: "SmartRx_PatientProfile",
            //    type: "bit",
            //    nullable: false,
            //    defaultValue: false);

            //migrationBuilder.AddColumn<string>(
            //    name: "Profession",
            //    table: "SmartRx_PatientProfile",
            //    type: "nvarchar(max)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "RelationToPatient",
            //    table: "SmartRx_PatientProfile",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ActiveChamberId",
                table: "SmartRx_PatientDoctor",
                type: "bigint",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Configuration_Education",
                type: "nvarchar(1500)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)");

            migrationBuilder.AddColumn<string>(
                name: "InstitutionName",
                table: "Configuration_Education",
                type: "nvarchar(200)",
                nullable: true);

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

            migrationBuilder.AddColumn<bool>(
                name: "IsMainChamber",
                table: "Configuration_DoctorChamber",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "BMDCRegNo",
                table: "Configuration_Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChamberIds",
                table: "Configuration_Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Configuration_Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EducationDegreeIds",
                table: "Configuration_Doctor",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Experiences",
                table: "Configuration_Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                table: "Configuration_Doctor",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                table: "Configuration_Doctor",
                type: "nvarchar(200)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProfessionalSummary",
                table: "Configuration_Doctor",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Rating",
                table: "Configuration_Doctor",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SpecializedArea",
                table: "Configuration_Doctor",
                type: "nvarchar(200)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "YearOfExperiences",
                table: "Configuration_Doctor",
                type: "int",
                nullable: false,
                defaultValue: 0);

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientProfile_ExistingPatientId",
            //    table: "SmartRx_PatientProfile",
            //    column: "ExistingPatientId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmartRx_PatientProfile_SmartRx_PatientProfile_ExistingPatientId",
            //    table: "SmartRx_PatientProfile",
            //    column: "ExistingPatientId",
            //    principalTable: "SmartRx_PatientProfile",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmartRx_PatientProfile_SmartRx_PatientProfile_RelatedToPatientId",
            //    table: "SmartRx_PatientProfile",
            //    column: "RelatedToPatientId",
            //    principalTable: "SmartRx_PatientProfile",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmartRx_PatientProfile_SmartRx_PatientProfile_ExistingPatientId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropForeignKey(
            //    name: "FK_SmartRx_PatientProfile_SmartRx_PatientProfile_RelatedToPatientId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropIndex(
            //    name: "IX_SmartRx_PatientProfile_ExistingPatientId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "ExistingPatientId",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "IsExistingPatient",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "IsRelative",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "Profession",
            //    table: "SmartRx_PatientProfile");

            //migrationBuilder.DropColumn(
            //    name: "RelationToPatient",
            //    table: "SmartRx_PatientProfile");

            migrationBuilder.DropColumn(
                name: "ActiveChamberId",
                table: "SmartRx_PatientDoctor");

            migrationBuilder.DropColumn(
                name: "InstitutionName",
                table: "Configuration_Education");

            migrationBuilder.DropColumn(
                name: "MajorSubject",
                table: "Configuration_Education");

            migrationBuilder.DropColumn(
                name: "PassingYear",
                table: "Configuration_Education");

            migrationBuilder.DropColumn(
                name: "IsMainChamber",
                table: "Configuration_DoctorChamber");

            migrationBuilder.DropColumn(
                name: "BMDCRegNo",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "ChamberIds",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "EducationDegreeIds",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "Experiences",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "FirstName",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "LastName",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "ProfessionalSummary",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "Rating",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "SpecializedArea",
                table: "Configuration_Doctor");

            migrationBuilder.DropColumn(
                name: "YearOfExperiences",
                table: "Configuration_Doctor");

            //migrationBuilder.RenameColumn(
            //    name: "RelatedToPatientId",
            //    table: "SmartRx_PatientProfile",
            //    newName: "DistrictId");

            //migrationBuilder.RenameIndex(
            //    name: "IX_SmartRx_PatientProfile_RelatedToPatientId",
            //    table: "SmartRx_PatientProfile",
            //    newName: "IX_SmartRx_PatientProfile_DistrictId");

            migrationBuilder.RenameColumn(
                name: "ConsultingDuration",
                table: "SmartRx_PatientDoctor",
                newName: "ConsultingTime");

            migrationBuilder.RenameColumn(
                name: "DegreeName",
                table: "Configuration_Education",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Remarks",
                table: "Configuration_DoctorChamber",
                newName: "ChamberRemarks");

            migrationBuilder.RenameColumn(
                name: "Title",
                table: "Configuration_Doctor",
                newName: "Name");

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "SmartRx_PatientProfile",
                type: "nvarchar(2000)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(2000)",
                oldNullable: true);

            //migrationBuilder.AddColumn<long>(
            //    name: "CountryId",
            //    table: "SmartRx_PatientProfile",
            //    type: "bigint",
            //    nullable: false,
            //    defaultValue: 0L);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Configuration_Education",
                type: "nvarchar(1500)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(1500)",
                oldNullable: true);

            //migrationBuilder.AddColumn<long>(
            //    name: "Configuration_DoctorEntityId",
            //    table: "Configuration_Education",
            //    type: "bigint",
            //    nullable: true);

            migrationBuilder.CreateTable(
                name: "Configuration_DoctorEducation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    EducationId = table.Column<long>(type: "bigint", nullable: false),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    EducationLocation = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_DoctorEducation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorEducation_Configuration_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Configuration_Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorEducation_Configuration_Education_EducationId",
                        column: x => x.EducationId,
                        principalTable: "Configuration_Education",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorEducation_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorEducation_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientProfile_CountryId",
            //    table: "SmartRx_PatientProfile",
            //    column: "CountryId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Configuration_Education_Configuration_DoctorEntityId",
            //    table: "Configuration_Education",
            //    column: "Configuration_DoctorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorEducation_CreatedById",
                table: "Configuration_DoctorEducation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorEducation_DoctorId",
                table: "Configuration_DoctorEducation",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorEducation_EducationId",
                table: "Configuration_DoctorEducation",
                column: "EducationId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorEducation_ModifiedById",
                table: "Configuration_DoctorEducation",
                column: "ModifiedById");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_Configuration_Education_Configuration_Doctor_Configuration_DoctorEntityId",
            //    table: "Configuration_Education",
            //    column: "Configuration_DoctorEntityId",
            //    principalTable: "Configuration_Doctor",
            //    principalColumn: "Id");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Configuration_Country_CountryId",
            //    table: "SmartRx_PatientProfile",
            //    column: "CountryId",
            //    principalTable: "Configuration_Country",
            //    principalColumn: "Id",
            //    onDelete: ReferentialAction.Cascade);

            //migrationBuilder.AddForeignKey(
            //    name: "FK_SmartRx_PatientProfile_Configuration_District_DistrictId",
            //    table: "SmartRx_PatientProfile",
            //    column: "DistrictId",
            //    principalTable: "Configuration_District",
            //    principalColumn: "Id");
        }
    }
}
