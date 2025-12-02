using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PMSBackend.Databases.Migrations
{
    /// <inheritdoc />
    public partial class InitialCommit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Security_PMSUser",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserCode = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    MobileNo = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    GoogleId = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    FacebookId = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    TwitterId = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    FirstName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    AuthMethod = table.Column<int>(type: "int", nullable: false),
                    EmployeeId = table.Column<int>(type: "int", nullable: true),
                    EmployeeCode = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true),
                    SmartRxUserEntityId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security_PMSUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Security_PMSUser_Security_PMSUser_SmartRxUserEntityId",
                        column: x => x.SmartRxUserEntityId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_AdviceFAQ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    TagSearchKeyword = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    IconFileName = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    IconFilePath = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    IconFileExtension = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_AdviceFAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_AdviceFAQ_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_AdviceFAQ_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_ChiefComplaint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(10)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    FullForm = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(1500)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_ChiefComplaint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_ChiefComplaint_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_ChiefComplaint_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Country",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Code = table.Column<string>(type: "nchar(3)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Country", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Country_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Country_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Designation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Designation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Designation_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Designation_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_District",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(2)", nullable: false),
                    Name = table.Column<string>(type: "varchar(150)", nullable: false),
                    DivisionId = table.Column<int>(type: "int", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_District", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_District_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_District_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Doctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Code = table.Column<string>(type: "nchar(10)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Doctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Doctor_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Doctor_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_MedicineDosageForm",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ShortForm = table.Column<string>(type: "nvarchar(15)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_MedicineDosageForm", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineDosageForm_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineDosageForm_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_MedicineFAQ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MedicineId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_MedicineFAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineFAQ_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineFAQ_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_MedicineGeneric",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_MedicineGeneric", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineGeneric_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineGeneric_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_MedicineManufactureInfo",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    OriginRegion = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Importer = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    EstablishedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Products = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_MedicineManufactureInfo", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineManufactureInfo_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineManufactureInfo_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_PrescriptionSection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    HeadlineText = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_PrescriptionSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_PrescriptionSection_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_PrescriptionSection_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_SmartRxAcronym",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Keyword = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Acronym = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Abbreviation = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_SmartRxAcronym", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_SmartRxAcronym_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_SmartRxAcronym_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Tags",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TagShortName = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    TagDescription = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TagPrescriptionSection = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Tags", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Tags_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Tags_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Unit",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(4)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    MeasurementUnit = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Type = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Unit", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Unit_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Unit_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_VitalFAQ",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VitalId = table.Column<long>(type: "bigint", nullable: false),
                    Question = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Answer = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    TagSearchKeyword = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    IconFileName = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    IconFilePath = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    IconFileExtension = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_VitalFAQ", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_VitalFAQ_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_VitalFAQ_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Security_PMSRole",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    IsSelfService = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security_PMSRole", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Security_PMSRole_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Security_PMSRole_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_City",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Code = table.Column<string>(type: "nchar(5)", nullable: false),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_City", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_City_Configuration_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Configuration_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_City_Configuration_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Configuration_District",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_City_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_City_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Education",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(1500)", nullable: false),
                    Configuration_DoctorEntityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Education", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Education_Configuration_Doctor_Configuration_DoctorId",
                        column: x => x.Configuration_DoctorEntityId,
                        principalTable: "Configuration_Doctor",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Education_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Education_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_MedicineBrand",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ManufacturerId = table.Column<long>(type: "bigint", nullable: false),
                    BrandCode = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    BrandPublicId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_MedicineBrand", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineBrand_Configuration_MedicineManufactureInfo_ManufacturerId",
                        column: x => x.ManufacturerId,
                        principalTable: "Configuration_MedicineManufactureInfo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineBrand_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_MedicineBrand_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Investigation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(5)", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    TestDescription = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    TestFullName = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TestGenericName = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    TestShortName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    TestNameByDiagnosticCenter = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    PriceUnitId = table.Column<long>(type: "bigint", nullable: true),
                    NationalUnitPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    NationalPriceUnitId = table.Column<long>(type: "bigint", nullable: true),
                    Speciality = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    Specimen = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Investigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Investigation_Configuration_Unit_NationalPriceUnitId",
                        column: x => x.NationalPriceUnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Investigation_Configuration_Unit_PriceUnitId",
                        column: x => x.PriceUnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Investigation_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Investigation_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Vital",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(2)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ApplicableEntity = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    LowRange = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    LowStatus = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    MidRange = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MidStatus = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    MidNextRange = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    MidNextStatus = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    HighRange = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    HighStatus = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    ExtremeRange = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    ExtremeStatus = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    Configuration_UnitEntityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Vital", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Vital_Configuration_Unit_Configuration_UnitEntityId",
                        column: x => x.Configuration_UnitEntityId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Vital_Configuration_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Vital_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Vital_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Security_PMSUserWiseRole",
                columns: table => new
                {
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    RoleId = table.Column<long>(type: "bigint", nullable: false),
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security_PMSUserWiseRole", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_Security_PMSUserWiseRole_Security_PMSRole_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Security_PMSRole",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Security_PMSUserWiseRole_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Security_PMSUserWiseRole_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Security_PMSUserWiseRole_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Hospital",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(4000)", nullable: false),
                    DiagnosticCenterIcon = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    YearEstablished = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    GoogleRating = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    GoogleLocation = table.Column<string>(type: "nvarchar(3000)", nullable: true),
                    OpenTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CloseTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    OpenDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CloseDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Weekend = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    WebAddress = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Hospital", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Hospital_Configuration_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Configuration_City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Hospital_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Hospital_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_PoliceStation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(10)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    DistrictId = table.Column<long>(type: "bigint", nullable: false),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_PoliceStation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_PoliceStation_Configuration_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Configuration_City",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_PoliceStation_Configuration_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Configuration_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_PoliceStation_Configuration_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Configuration_District",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_PoliceStation_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_PoliceStation_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_DoctorEducation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    EducationId = table.Column<long>(type: "bigint", nullable: false),
                    EducationLocation = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Configuration_Medicine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    BrandId = table.Column<long>(type: "bigint", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    Slug = table.Column<string>(type: "nvarchar(400)", nullable: true),
                    DosageFormId = table.Column<long>(type: "bigint", nullable: false),
                    GenericId = table.Column<long>(type: "bigint", nullable: false),
                    Strength = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    MeasurementUnitId = table.Column<long>(type: "bigint", nullable: true),
                    UnitPrice = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    PriceInUnitId = table.Column<long>(type: "bigint", nullable: true),
                    PackageType = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    PackageSize = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    PackageQuantity = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    DAR = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Indication = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Pharmacology = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    DoseDescription = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    Administration = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Contradiction = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    SideEffects = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    PrecautionsAndWarnings = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    PregnencyAndLactation = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    ModeOfAction = table.Column<string>(type: "nvarchar(1)", nullable: true),
                    Interaction = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OverdoseEffects = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TherapeuticClass = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StorageCondition = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyDiscount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Medicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Configuration_MedicineBrand_BrandId",
                        column: x => x.BrandId,
                        principalTable: "Configuration_MedicineBrand",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Configuration_MedicineDosageForm_DosageFormId",
                        column: x => x.DosageFormId,
                        principalTable: "Configuration_MedicineDosageForm",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Configuration_MedicineGeneric_GenericId",
                        column: x => x.GenericId,
                        principalTable: "Configuration_MedicineGeneric",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Configuration_Unit_MeasurementUnitId",
                        column: x => x.MeasurementUnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Configuration_Unit_PriceInUnitId",
                        column: x => x.PriceInUnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_Medicine_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_DepartmentSection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_DepartmentSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_DepartmentSection_Configuration_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Configuration_DepartmentSection_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DepartmentSection_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_HospitalBranch",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    BranchLocation = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    AddressLine1 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    AddressLine2 = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    YearEstablished = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    GoogleRating = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    GoogleLocation = table.Column<string>(type: "nvarchar(3000)", nullable: true),
                    OpenTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CloseTime = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    OpenDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    CloseDay = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Weekend = table.Column<string>(type: "nvarchar(20)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(300)", nullable: true),
                    WebAddress = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    IsMainBranch = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientProfile",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientCode = table.Column<string>(type: "nchar(10)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    NickName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Age = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Gender = table.Column<int>(type: "int", nullable: false),
                    BloodGroup = table.Column<int>(type: "int", nullable: true),
                    Height = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(40)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    ProfilePhotoName = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    ProfilePhotoPath = table.Column<string>(type: "nvarchar(2000)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    PoliceStationId = table.Column<long>(type: "bigint", nullable: true),
                    CityId = table.Column<long>(type: "bigint", nullable: true),
                    DistrictId = table.Column<long>(type: "bigint", nullable: true),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CountryId = table.Column<long>(type: "bigint", nullable: false),
                    EmergencyContact = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaritalStatus = table.Column<int>(type: "int", nullable: true),
                    IsActive = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientProfile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Configuration_City_CityId",
                        column: x => x.CityId,
                        principalTable: "Configuration_City",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Configuration_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Configuration_Country",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Configuration_District_DistrictId",
                        column: x => x.DistrictId,
                        principalTable: "Configuration_District",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Configuration_PoliceStation_PoliceStationId",
                        column: x => x.PoliceStationId,
                        principalTable: "Configuration_PoliceStation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientProfile_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Department",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nvarchar(5)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(200)", nullable: false),
                    SectionId = table.Column<long>(type: "bigint", nullable: true),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_Department", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_Department_Configuration_DepartmentSection_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Configuration_DepartmentSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Department_Configuration_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Department_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_Department_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_DiagnosisCenterWiseTest",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TestCenterId = table.Column<long>(type: "bigint", nullable: false),
                    TestCenterBranchId = table.Column<long>(type: "bigint", nullable: true),
                    TestId = table.Column<long>(type: "bigint", nullable: false),
                    DiagnosticCenterGivenTestName = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    DiagnosticCenterGivenPrice = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    DiscountByAuthority = table.Column<decimal>(type: "decimal(10,2)", nullable: true),
                    PriceUnitId = table.Column<long>(type: "bigint", nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ReportDeliveryTime = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    SpecialNote = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_DiagnosisCenterWiseTest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_HospitalBranch_TestCenterBranchId",
                        column: x => x.TestCenterBranchId,
                        principalTable: "Configuration_HospitalBranch",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_Hospital_TestCenterId",
                        column: x => x.TestCenterId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_Investigation_TestId",
                        column: x => x.TestId,
                        principalTable: "Configuration_Investigation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Configuration_Unit_PriceUnitId",
                        column: x => x.PriceUnitId,
                        principalTable: "Configuration_Unit",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Configuration_DiagnosisCenterWiseTest_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Prescription_UserWiseFolder",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ParentFolderId = table.Column<long>(type: "bigint", nullable: true),
                    FolderHierarchy = table.Column<string>(type: "varchar(50)", nullable: false),
                    FolderName = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription_UserWiseFolder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescription_UserWiseFolder_Prescription_UserWiseFolder_ParentFolderId",
                        column: x => x.ParentFolderId,
                        principalTable: "Prescription_UserWiseFolder",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Prescription_UserWiseFolder_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_UserWiseFolder_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_UserWiseFolder_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Prescription_UserWiseFolder_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_Master",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    ChiefComplaintIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NextAppoinmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextAppoinmentTime = table.Column<string>(type: "varchar(10)", nullable: true),
                    DiscountPercentageOnMedicineByDoctor = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true),
                    DiscountPercentageOnInvestigationByDoctor = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    LockedById = table.Column<long>(type: "bigint", nullable: true),
                    LockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReported = table.Column<bool>(type: "bit", nullable: true),
                    ReportById = table.Column<long>(type: "bigint", nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportReason = table.Column<string>(type: "varchar(500)", nullable: true),
                    ReportDetails = table.Column<string>(type: "varchar(4000)", nullable: true),
                    IsRecommended = table.Column<bool>(type: "bit", nullable: true),
                    RecommendedById = table.Column<long>(type: "bigint", nullable: true),
                    RecommendedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedById = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    CompletedById = table.Column<long>(type: "bigint", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsRejected = table.Column<bool>(type: "bit", nullable: true),
                    RejectedById = table.Column<long>(type: "bigint", nullable: true),
                    RejectedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RejectionRemarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsExistingPatient = table.Column<bool>(type: "bit", nullable: true),
                    HasAnyRelative = table.Column<bool>(type: "bit", nullable: true),
                    Tag1 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag2 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag3 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag4 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag5 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_Master", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_CompletedById",
                        column: x => x.CompletedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_LockedById",
                        column: x => x.LockedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_RecommendedById",
                        column: x => x.RecommendedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_RejectedById",
                        column: x => x.RejectedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_ReportById",
                        column: x => x.ReportById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_Master_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Configuration_Chamber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(type: "nchar(10)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(1500)", nullable: false),
                    CityId = table.Column<long>(type: "bigint", nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    GoogleAddress = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    GoogleLocationLink = table.Column<string>(type: "nvarchar(2000)", nullable: false),
                    GoogleRating = table.Column<string>(type: "nvarchar(5)", nullable: true),
                    DoctorBookingMobileNos = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    Helpline_CallCenter = table.Column<string>(type: "nvarchar(15)", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", nullable: true),
                    OverseasCaller = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    VisitingHours = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    ClosedOnDay = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    WhatsAppNumber = table.Column<string>(type: "nvarchar(25)", nullable: true),
                    ChamberStartTime = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    ChamberEndTime = table.Column<string>(type: "nvarchar(6)", nullable: true),
                    OtherDoctorsId = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(500)", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
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

            migrationBuilder.CreateTable(
                name: "Prescription_Upload",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PrescriptionCode = table.Column<string>(type: "nchar(20)", nullable: false),
                    PatientId = table.Column<long>(type: "bigint", nullable: true),
                    SmartRxId = table.Column<long>(type: "bigint", nullable: true),
                    IsExistingPatient = table.Column<bool>(type: "bit", nullable: true),
                    HasExistingRelative = table.Column<bool>(type: "bit", nullable: true),
                    RelativePatientIds = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(300)", nullable: false),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    FileExtension = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    NumberOfFilesStoredForThisPrescription = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<long>(type: "bigint", nullable: false),
                    FolderId = table.Column<long>(type: "bigint", nullable: false),
                    IsSmartRxRequested = table.Column<bool>(type: "bit", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: true),
                    LockedById = table.Column<long>(type: "bigint", nullable: true),
                    LockedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReported = table.Column<bool>(type: "bit", nullable: true),
                    ReportById = table.Column<long>(type: "bigint", nullable: true),
                    ReportDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ReportReason = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    ReportDetails = table.Column<string>(type: "nvarchar(4000)", nullable: true),
                    IsRecommended = table.Column<bool>(type: "bit", nullable: true),
                    RecommendedById = table.Column<long>(type: "bigint", nullable: true),
                    RecommendedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedById = table.Column<long>(type: "bigint", nullable: true),
                    ApprovedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: true),
                    CompletedById = table.Column<long>(type: "bigint", nullable: true),
                    CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Tag1 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag2 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag3 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag4 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Tag5 = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    NextAppoinmentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    NextAppoinmentTime = table.Column<string>(type: "nvarchar(10)", nullable: true),
                    DiscountPercentageOnMedicineByDoctor = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    DiscountPercentageOnInvestigationByDoctor = table.Column<decimal>(type: "decimal(5,2)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Prescription_Upload", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Prescription_UserWiseFolder_FolderId",
                        column: x => x.FolderId,
                        principalTable: "Prescription_UserWiseFolder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_ApprovedById",
                        column: x => x.ApprovedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_CompletedById",
                        column: x => x.CompletedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_LockedById",
                        column: x => x.LockedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_RecommendedById",
                        column: x => x.RecommendedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_ReportById",
                        column: x => x.ReportById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_Security_PMSUser_UserId",
                        column: x => x.UserId,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_SmartRx_Master_SmartRxId",
                        column: x => x.SmartRxId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Prescription_Upload_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientRelatives",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PatientId = table.Column<long>(type: "bigint", nullable: false),
                    PatientRelativeId = table.Column<long>(type: "bigint", nullable: true),
                    SmartRx_MasterEntityId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientRelatives", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientRelatives_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientRelatives_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientRelatives_SmartRx_Master_SmartRx_MasterEntityId",
                        column: x => x.SmartRx_MasterEntityId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientRelatives_SmartRx_PatientProfile_PatientId",
                        column: x => x.PatientId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientRelatives_SmartRx_PatientProfile_PatientRelativeId",
                        column: x => x.PatientRelativeId,
                        principalTable: "SmartRx_PatientProfile",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Configuration_DoctorChamber",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    HospitalId = table.Column<long>(type: "bigint", nullable: false),
                    DepartmentSectionId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    ChamberId = table.Column<long>(type: "bigint", nullable: false),
                    DoctorDesignationInChamberId = table.Column<long>(type: "bigint", nullable: true),
                    VisitingHour = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ChamberRemarks = table.Column<string>(type: "nvarchar(1500)", nullable: false),
                    DoctorSpecialization = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration_DoctorChamber", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_Chamber_ChamberId",
                        column: x => x.ChamberId,
                        principalTable: "Configuration_Chamber",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_DepartmentSection_DepartmentSectionId",
                        column: x => x.DepartmentSectionId,
                        principalTable: "Configuration_DepartmentSection",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_Department_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Configuration_Department",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_Designation_DoctorDesignationInChamberId",
                        column: x => x.DoctorDesignationInChamberId,
                        principalTable: "Configuration_Designation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Configuration_Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Configuration_Hospital_HospitalId",
                        column: x => x.HospitalId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Configuration_DoctorChamber_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientAdvice",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    Advice = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdviceKeywordToRecommend = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientAdvice", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientAdvice_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientAdvice_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientAdvice_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientAdvice_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientChiefComplaint",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    ChiefComplaintId = table.Column<long>(type: "bigint", nullable: false),
                    UploadedPrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientChiefComplaint", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientChiefComplaint_Configuration_ChiefComplaint_ChiefComplaintId",
                        column: x => x.ChiefComplaintId,
                        principalTable: "Configuration_ChiefComplaint",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientChiefComplaint_Prescription_Upload_UploadedPrescriptionId",
                        column: x => x.UploadedPrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientChiefComplaint_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientChiefComplaint_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientChiefComplaint_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientDoctor",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    DoctorId = table.Column<long>(type: "bigint", nullable: false),
                    ChamberWaitTime = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    ChamberFee = table.Column<string>(type: "nvarchar(10)", nullable: false),
                    DoctorRating = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    ConsultingTime = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientDoctor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientDoctor_Configuration_Doctor_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Configuration_Doctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientDoctor_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientDoctor_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientDoctor_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientDoctor_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientHistory",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(1000)", nullable: false),
                    Details = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientHistory_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientHistory_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientHistory_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientHistory_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientInvestigation",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    TestId = table.Column<long>(type: "bigint", nullable: false),
                    DiagnosticCenterWiseTestId = table.Column<long>(type: "bigint", nullable: true),
                    TestCenterId = table.Column<long>(type: "bigint", nullable: true),
                    TestCenterBranchId = table.Column<long>(type: "bigint", nullable: true),
                    DiscountByAuthority = table.Column<decimal>(type: "decimal(10,2)", nullable: false),
                    TestPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Result = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientInvestigation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Configuration_DiagnosisCenterWiseTest_DiagnosticCenterWiseTestId",
                        column: x => x.DiagnosticCenterWiseTestId,
                        principalTable: "Configuration_DiagnosisCenterWiseTest",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Configuration_HospitalBranch_TestCenterBranchId",
                        column: x => x.TestCenterBranchId,
                        principalTable: "Configuration_HospitalBranch",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Configuration_Hospital_TestCenterId",
                        column: x => x.TestCenterId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Configuration_Investigation_TestId",
                        column: x => x.TestId,
                        principalTable: "Configuration_Investigation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientInvestigation_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientMedicine",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    MedicineId = table.Column<long>(type: "bigint", nullable: false),
                    FrequencyInADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose1InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose2InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose3InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose4InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose5InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose6InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose7InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose8InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose9InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose10InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose11InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    Dose12InADay = table.Column<string>(type: "nvarchar(50)", nullable: true),
                    IsMoreThanRegularDose = table.Column<bool>(type: "bit", nullable: true),
                    DescriptionForMoreThanRegularDose = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    IsBeforeMeal = table.Column<bool>(type: "bit", nullable: true),
                    DurationOfContinuation = table.Column<string>(type: "nvarchar(100)", nullable: false),
                    DurationOfContinuationCount = table.Column<int>(type: "int", nullable: false),
                    DurationOfContinuationStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DurationOfContinuationEndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Rules = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Restrictions = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(1000)", nullable: true),
                    Wishlist = table.Column<string>(type: "nvarchar(100)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientMedicine", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientMedicine_Configuration_Medicine_MedicineId",
                        column: x => x.MedicineId,
                        principalTable: "Configuration_Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientMedicine_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientMedicine_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientMedicine_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientMedicine_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientVitals",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    VitalId = table.Column<long>(type: "bigint", nullable: false),
                    VitalValue = table.Column<decimal>(type: "decimal(5,2)", nullable: false),
                    VitalStatus = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientVitals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientVitals_Configuration_Vital_VitalId",
                        column: x => x.VitalId,
                        principalTable: "Configuration_Vital",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientVitals_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientVitals_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientVitals_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientVitals_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_PatientWishlist",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    PrescriptionId = table.Column<long>(type: "bigint", nullable: false),
                    WishListType = table.Column<string>(type: "nvarchar(50)", nullable: false),
                    PatientMedicineId = table.Column<long>(type: "bigint", nullable: true),
                    PatientWishlistMedicineId = table.Column<long>(type: "bigint", nullable: true),
                    //PatientWishListMedicineId = table.Column<long>(type: "bigint", nullable: true),
                    PatientTestId = table.Column<long>(type: "bigint", nullable: true),
                    PatientRecommendedTestCenterId = table.Column<long>(type: "bigint", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_PatientWishlist", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Configuration_Hospital_PatientRecommendedTestCenterId",
                        column: x => x.PatientRecommendedTestCenterId,
                        principalTable: "Configuration_Hospital",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Configuration_Investigation_PatientTestId",
                        column: x => x.PatientTestId,
                        principalTable: "Configuration_Investigation",
                        principalColumn: "Id");
                    //table.ForeignKey(
                    //    name: "FK_SmartRx_PatientWishlist_Configuration_Medicine_PatientWishListMedicineId",
                    //    column: x => x.PatientWishListMedicineId,
                    //    principalTable: "Configuration_Medicine",
                    //    principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Configuration_Medicine_PatientWishlistMedicineId",
                        column: x => x.PatientWishlistMedicineId,
                        principalTable: "Configuration_Medicine",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Prescription_Upload_PrescriptionId",
                        column: x => x.PrescriptionId,
                        principalTable: "Prescription_Upload",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SmartRx_PatientWishlist_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SmartRx_ReferredConsultant",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SmartRxMasterId = table.Column<long>(type: "bigint", nullable: false),
                    ReferredConsultantId = table.Column<long>(type: "bigint", nullable: true),
                    ReferredBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedById = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ModifiedById = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SmartRx_ReferredConsultant", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SmartRx_ReferredConsultant_Security_PMSUser_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_ReferredConsultant_Security_PMSUser_ModifiedById",
                        column: x => x.ModifiedById,
                        principalTable: "Security_PMSUser",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_ReferredConsultant_SmartRx_Master_SmartRxMasterId",
                        column: x => x.SmartRxMasterId,
                        principalTable: "SmartRx_Master",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SmartRx_ReferredConsultant_SmartRx_PatientDoctor_ReferredConsultantId",
                        column: x => x.ReferredConsultantId,
                        principalTable: "SmartRx_PatientDoctor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_AdviceFAQ_CreatedById",
                table: "Configuration_AdviceFAQ",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_AdviceFAQ_ModifiedById",
                table: "Configuration_AdviceFAQ",
                column: "ModifiedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_ChiefComplaint_CreatedById",
                table: "Configuration_ChiefComplaint",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_ChiefComplaint_ModifiedById",
                table: "Configuration_ChiefComplaint",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_City_CountryId",
                table: "Configuration_City",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_City_CreatedById",
                table: "Configuration_City",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_City_DistrictId",
                table: "Configuration_City",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_City_ModifiedById",
                table: "Configuration_City",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Country_CreatedById",
                table: "Configuration_Country",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Country_ModifiedById",
                table: "Configuration_Country",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Department_CreatedById",
                table: "Configuration_Department",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Department_HospitalId",
                table: "Configuration_Department",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Department_ModifiedById",
                table: "Configuration_Department",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Department_SectionId",
                table: "Configuration_Department",
                column: "SectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DepartmentSection_CreatedById",
                table: "Configuration_DepartmentSection",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DepartmentSection_HospitalId",
                table: "Configuration_DepartmentSection",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DepartmentSection_ModifiedById",
                table: "Configuration_DepartmentSection",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Designation_CreatedById",
                table: "Configuration_Designation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Designation_ModifiedById",
                table: "Configuration_Designation",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_CreatedById",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_ModifiedById",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_PriceUnitId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "PriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_TestCenterBranchId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "TestCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_TestCenterId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "TestCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DiagnosisCenterWiseTest_TestId",
                table: "Configuration_DiagnosisCenterWiseTest",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_District_CreatedById",
                table: "Configuration_District",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_District_ModifiedById",
                table: "Configuration_District",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Doctor_CreatedById",
                table: "Configuration_Doctor",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Doctor_ModifiedById",
                table: "Configuration_Doctor",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_ChamberId",
                table: "Configuration_DoctorChamber",
                column: "ChamberId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_CreatedById",
                table: "Configuration_DoctorChamber",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_DepartmentId",
                table: "Configuration_DoctorChamber",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_DepartmentSectionId",
                table: "Configuration_DoctorChamber",
                column: "DepartmentSectionId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_DoctorDesignationInChamberId",
                table: "Configuration_DoctorChamber",
                column: "DoctorDesignationInChamberId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_DoctorId",
                table: "Configuration_DoctorChamber",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_HospitalId",
                table: "Configuration_DoctorChamber",
                column: "HospitalId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_DoctorChamber_ModifiedById",
                table: "Configuration_DoctorChamber",
                column: "ModifiedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Education_Configuration_DoctorEntityId",
                table: "Configuration_Education",
                column: "Configuration_DoctorEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Education_CreatedById",
                table: "Configuration_Education",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Education_ModifiedById",
                table: "Configuration_Education",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Hospital_CityId",
                table: "Configuration_Hospital",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Hospital_CreatedById",
                table: "Configuration_Hospital",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Hospital_ModifiedById",
                table: "Configuration_Hospital",
                column: "ModifiedById");

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

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Investigation_CreatedById",
                table: "Configuration_Investigation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Investigation_ModifiedById",
                table: "Configuration_Investigation",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Investigation_NationalPriceUnitId",
                table: "Configuration_Investigation",
                column: "NationalPriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Investigation_PriceUnitId",
                table: "Configuration_Investigation",
                column: "PriceUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_BrandId",
                table: "Configuration_Medicine",
                column: "BrandId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_CreatedById",
                table: "Configuration_Medicine",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_DosageFormId",
                table: "Configuration_Medicine",
                column: "DosageFormId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_GenericId",
                table: "Configuration_Medicine",
                column: "GenericId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_MeasurementUnitId",
                table: "Configuration_Medicine",
                column: "MeasurementUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_ModifiedById",
                table: "Configuration_Medicine",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Medicine_PriceInUnitId",
                table: "Configuration_Medicine",
                column: "PriceInUnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineBrand_CreatedById",
                table: "Configuration_MedicineBrand",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineBrand_ManufacturerId",
                table: "Configuration_MedicineBrand",
                column: "ManufacturerId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineBrand_ModifiedById",
                table: "Configuration_MedicineBrand",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineDosageForm_CreatedById",
                table: "Configuration_MedicineDosageForm",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineDosageForm_ModifiedById",
                table: "Configuration_MedicineDosageForm",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineFAQ_CreatedById",
                table: "Configuration_MedicineFAQ",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineFAQ_ModifiedById",
                table: "Configuration_MedicineFAQ",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineGeneric_CreatedById",
                table: "Configuration_MedicineGeneric",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineGeneric_ModifiedById",
                table: "Configuration_MedicineGeneric",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineManufactureInfo_CreatedById",
                table: "Configuration_MedicineManufactureInfo",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_MedicineManufactureInfo_ModifiedById",
                table: "Configuration_MedicineManufactureInfo",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PoliceStation_CityId",
                table: "Configuration_PoliceStation",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PoliceStation_CountryId",
                table: "Configuration_PoliceStation",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PoliceStation_CreatedById",
                table: "Configuration_PoliceStation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PoliceStation_DistrictId",
                table: "Configuration_PoliceStation",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PoliceStation_ModifiedById",
                table: "Configuration_PoliceStation",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PrescriptionSection_CreatedById",
                table: "Configuration_PrescriptionSection",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_PrescriptionSection_ModifiedById",
                table: "Configuration_PrescriptionSection",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_SmartRxAcronym_CreatedById",
                table: "Configuration_SmartRxAcronym",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_SmartRxAcronym_ModifiedById",
                table: "Configuration_SmartRxAcronym",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Tags_CreatedById",
                table: "Configuration_Tags",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Tags_ModifiedById",
                table: "Configuration_Tags",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Unit_CreatedById",
                table: "Configuration_Unit",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Unit_ModifiedById",
                table: "Configuration_Unit",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Vital_Configuration_UnitEntityId",
                table: "Configuration_Vital",
                column: "Configuration_UnitEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Vital_CreatedById",
                table: "Configuration_Vital",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Vital_ModifiedById",
                table: "Configuration_Vital",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_Vital_UnitId",
                table: "Configuration_Vital",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_VitalFAQ_CreatedById",
                table: "Configuration_VitalFAQ",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Configuration_VitalFAQ_ModifiedById",
                table: "Configuration_VitalFAQ",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_ApprovedById",
                table: "Prescription_Upload",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_CompletedById",
                table: "Prescription_Upload",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_CreatedById",
                table: "Prescription_Upload",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_FolderId",
                table: "Prescription_Upload",
                column: "FolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_LockedById",
                table: "Prescription_Upload",
                column: "LockedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_ModifiedById",
                table: "Prescription_Upload",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_PatientId",
                table: "Prescription_Upload",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_RecommendedById",
                table: "Prescription_Upload",
                column: "RecommendedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_ReportById",
                table: "Prescription_Upload",
                column: "ReportById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_SmartRxId",
                table: "Prescription_Upload",
                column: "SmartRxId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_Upload_UserId",
                table: "Prescription_Upload",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_CreatedById",
                table: "Prescription_UserWiseFolder",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_ModifiedById",
                table: "Prescription_UserWiseFolder",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_ParentFolderId",
                table: "Prescription_UserWiseFolder",
                column: "ParentFolderId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_PatientId",
                table: "Prescription_UserWiseFolder",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_Prescription_UserWiseFolder_UserId",
                table: "Prescription_UserWiseFolder",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSRole_CreatedById",
                table: "Security_PMSRole",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSRole_ModifiedById",
                table: "Security_PMSRole",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSUser_SmartRxUserEntityId",
                table: "Security_PMSUser",
                column: "SmartRxUserEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSUserWiseRole_CreatedById",
                table: "Security_PMSUserWiseRole",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSUserWiseRole_ModifiedById",
                table: "Security_PMSUserWiseRole",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_Security_PMSUserWiseRole_RoleId",
                table: "Security_PMSUserWiseRole",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_ApprovedById",
                table: "SmartRx_Master",
                column: "ApprovedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_CompletedById",
                table: "SmartRx_Master",
                column: "CompletedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_CreatedById",
                table: "SmartRx_Master",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_LockedById",
                table: "SmartRx_Master",
                column: "LockedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_ModifiedById",
                table: "SmartRx_Master",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_PatientId",
                table: "SmartRx_Master",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_RecommendedById",
                table: "SmartRx_Master",
                column: "RecommendedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_RejectedById",
                table: "SmartRx_Master",
                column: "RejectedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_ReportById",
                table: "SmartRx_Master",
                column: "ReportById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_Master_UserId",
                table: "SmartRx_Master",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientAdvice_CreatedById",
                table: "SmartRx_PatientAdvice",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientAdvice_ModifiedById",
                table: "SmartRx_PatientAdvice",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientAdvice_PrescriptionId",
                table: "SmartRx_PatientAdvice",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientAdvice_SmartRxMasterId",
                table: "SmartRx_PatientAdvice",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_ChiefComplaintId",
                table: "SmartRx_PatientChiefComplaint",
                column: "ChiefComplaintId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_CreatedById",
                table: "SmartRx_PatientChiefComplaint",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_ModifiedById",
                table: "SmartRx_PatientChiefComplaint",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_SmartRxMasterId",
                table: "SmartRx_PatientChiefComplaint",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientChiefComplaint_UploadedPrescriptionId",
                table: "SmartRx_PatientChiefComplaint",
                column: "UploadedPrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_CreatedById",
                table: "SmartRx_PatientDoctor",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_DoctorId",
                table: "SmartRx_PatientDoctor",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_ModifiedById",
                table: "SmartRx_PatientDoctor",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_PrescriptionId",
                table: "SmartRx_PatientDoctor",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientDoctor_SmartRxMasterId",
                table: "SmartRx_PatientDoctor",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientHistory_CreatedById",
                table: "SmartRx_PatientHistory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientHistory_ModifiedById",
                table: "SmartRx_PatientHistory",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientHistory_PrescriptionId",
                table: "SmartRx_PatientHistory",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientHistory_SmartRxMasterId",
                table: "SmartRx_PatientHistory",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_CreatedById",
                table: "SmartRx_PatientInvestigation",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_DiagnosticCenterWiseTestId",
                table: "SmartRx_PatientInvestigation",
                column: "DiagnosticCenterWiseTestId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_ModifiedById",
                table: "SmartRx_PatientInvestigation",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_PrescriptionId",
                table: "SmartRx_PatientInvestigation",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_SmartRxMasterId",
                table: "SmartRx_PatientInvestigation",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterBranchId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_TestCenterId",
                table: "SmartRx_PatientInvestigation",
                column: "TestCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientInvestigation_TestId",
                table: "SmartRx_PatientInvestigation",
                column: "TestId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientMedicine_CreatedById",
                table: "SmartRx_PatientMedicine",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientMedicine_MedicineId",
                table: "SmartRx_PatientMedicine",
                column: "MedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientMedicine_ModifiedById",
                table: "SmartRx_PatientMedicine",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientMedicine_PrescriptionId",
                table: "SmartRx_PatientMedicine",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientMedicine_SmartRxMasterId",
                table: "SmartRx_PatientMedicine",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_CityId",
                table: "SmartRx_PatientProfile",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_CountryId",
                table: "SmartRx_PatientProfile",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_CreatedById",
                table: "SmartRx_PatientProfile",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_DistrictId",
                table: "SmartRx_PatientProfile",
                column: "DistrictId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_ModifiedById",
                table: "SmartRx_PatientProfile",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientProfile_PoliceStationId",
                table: "SmartRx_PatientProfile",
                column: "PoliceStationId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientRelatives_CreatedById",
                table: "SmartRx_PatientRelatives",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientRelatives_ModifiedById",
                table: "SmartRx_PatientRelatives",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientRelatives_PatientId",
                table: "SmartRx_PatientRelatives",
                column: "PatientId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientRelatives_PatientRelativeId",
                table: "SmartRx_PatientRelatives",
                column: "PatientRelativeId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientRelatives_SmartRx_MasterEntityId",
                table: "SmartRx_PatientRelatives",
                column: "SmartRx_MasterEntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_CreatedById",
                table: "SmartRx_PatientVitals",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_ModifiedById",
                table: "SmartRx_PatientVitals",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_PrescriptionId",
                table: "SmartRx_PatientVitals",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_SmartRxMasterId",
                table: "SmartRx_PatientVitals",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientVitals_VitalId",
                table: "SmartRx_PatientVitals",
                column: "VitalId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_CreatedById",
                table: "SmartRx_PatientWishlist",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_ModifiedById",
                table: "SmartRx_PatientWishlist",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_PatientRecommendedTestCenterId",
                table: "SmartRx_PatientWishlist",
                column: "PatientRecommendedTestCenterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_PatientTestId",
                table: "SmartRx_PatientWishlist",
                column: "PatientTestId");

            //migrationBuilder.CreateIndex(
            //    name: "IX_SmartRx_PatientWishlist_PatientWishlistMedicineId",
            //    table: "SmartRx_PatientWishlist",
            //    column: "PatientWishlistMedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_PatientWishListMedicineId",
                table: "SmartRx_PatientWishlist",
                column: "PatientWishListMedicineId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_PrescriptionId",
                table: "SmartRx_PatientWishlist",
                column: "PrescriptionId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_PatientWishlist_SmartRxMasterId",
                table: "SmartRx_PatientWishlist",
                column: "SmartRxMasterId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_ReferredConsultant_CreatedById",
                table: "SmartRx_ReferredConsultant",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_ReferredConsultant_ModifiedById",
                table: "SmartRx_ReferredConsultant",
                column: "ModifiedById");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_ReferredConsultant_ReferredConsultantId",
                table: "SmartRx_ReferredConsultant",
                column: "ReferredConsultantId");

            migrationBuilder.CreateIndex(
                name: "IX_SmartRx_ReferredConsultant_SmartRxMasterId",
                table: "SmartRx_ReferredConsultant",
                column: "SmartRxMasterId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Configuration_AdviceFAQ");

            migrationBuilder.DropTable(
                name: "Configuration_DoctorChamber");

            migrationBuilder.DropTable(
                name: "Configuration_DoctorEducation");

            migrationBuilder.DropTable(
                name: "Configuration_MedicineFAQ");

            migrationBuilder.DropTable(
                name: "Configuration_PrescriptionSection");

            migrationBuilder.DropTable(
                name: "Configuration_SmartRxAcronym");

            migrationBuilder.DropTable(
                name: "Configuration_Tags");

            migrationBuilder.DropTable(
                name: "Configuration_VitalFAQ");

            migrationBuilder.DropTable(
                name: "Security_PMSUserWiseRole");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientAdvice");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientChiefComplaint");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientHistory");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientInvestigation");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientMedicine");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientRelatives");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientVitals");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientWishlist");

            migrationBuilder.DropTable(
                name: "SmartRx_ReferredConsultant");

            migrationBuilder.DropTable(
                name: "Configuration_Chamber");

            migrationBuilder.DropTable(
                name: "Configuration_Designation");

            migrationBuilder.DropTable(
                name: "Configuration_Education");

            migrationBuilder.DropTable(
                name: "Security_PMSRole");

            migrationBuilder.DropTable(
                name: "Configuration_ChiefComplaint");

            migrationBuilder.DropTable(
                name: "Configuration_DiagnosisCenterWiseTest");

            migrationBuilder.DropTable(
                name: "Configuration_Vital");

            migrationBuilder.DropTable(
                name: "Configuration_Medicine");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientDoctor");

            migrationBuilder.DropTable(
                name: "Configuration_Department");

            migrationBuilder.DropTable(
                name: "Configuration_HospitalBranch");

            migrationBuilder.DropTable(
                name: "Configuration_Investigation");

            migrationBuilder.DropTable(
                name: "Configuration_MedicineBrand");

            migrationBuilder.DropTable(
                name: "Configuration_MedicineDosageForm");

            migrationBuilder.DropTable(
                name: "Configuration_MedicineGeneric");

            migrationBuilder.DropTable(
                name: "Configuration_Doctor");

            migrationBuilder.DropTable(
                name: "Prescription_Upload");

            migrationBuilder.DropTable(
                name: "Configuration_DepartmentSection");

            migrationBuilder.DropTable(
                name: "Configuration_Unit");

            migrationBuilder.DropTable(
                name: "Configuration_MedicineManufactureInfo");

            migrationBuilder.DropTable(
                name: "Prescription_UserWiseFolder");

            migrationBuilder.DropTable(
                name: "SmartRx_Master");

            migrationBuilder.DropTable(
                name: "Configuration_Hospital");

            migrationBuilder.DropTable(
                name: "SmartRx_PatientProfile");

            migrationBuilder.DropTable(
                name: "Configuration_PoliceStation");

            migrationBuilder.DropTable(
                name: "Configuration_City");

            migrationBuilder.DropTable(
                name: "Configuration_Country");

            migrationBuilder.DropTable(
                name: "Configuration_District");

            migrationBuilder.DropTable(
                name: "Security_PMSUser");
        }
    }
}
