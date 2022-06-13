using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class AddInitial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DropDownType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRD",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Plant = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: true),
                    Originator = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsHRD = table.Column<bool>(type: "bit", nullable: true),
                    DayCode = table.Column<string>(type: "nvarchar(7)", maxLength: 7, nullable: true),
                    Line = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Shift = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LCL = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Schedule = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HourCode = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Cases = table.Column<int>(type: "int", nullable: true),
                    UnpalletizedProductHeld = table.Column<bool>(type: "bit", nullable: true),
                    RejectOrNinePack = table.Column<bool>(type: "bit", nullable: true),
                    ReworkOutsideNormalProduction = table.Column<bool>(type: "bit", nullable: true),
                    ContactOtherPlant = table.Column<bool>(type: "bit", nullable: true),
                    GLOBENum = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    ShortDescription = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    Problem = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReworkInstructions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReworkApproved = table.Column<bool>(type: "bit", nullable: true),
                    ReworkApprovedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ReworkStarted = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReworkComplete = table.Column<DateTime>(type: "datetime", nullable: true),
                    ReworkCompletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CauseMan = table.Column<bool>(type: "bit", nullable: true),
                    CauseMethod = table.Column<bool>(type: "bit", nullable: true),
                    CauseMaterial = table.Column<bool>(type: "bit", nullable: true),
                    CauseMachine = table.Column<bool>(type: "bit", nullable: true),
                    TLforFU = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Classification = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FTQCases = table.Column<int>(type: "int", nullable: true),
                    NonFTQCases = table.Column<int>(type: "int", nullable: true),
                    DCDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    DCUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FCDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    FCUser = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    OtherHRDAffected = table.Column<bool>(type: "bit", nullable: true),
                    OtherHRDNum = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateHeld = table.Column<DateTime>(type: "datetime", nullable: true),
                    HoldCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldSubCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CostofProductonHold = table.Column<decimal>(type: "money", nullable: true),
                    DateofDisposition = table.Column<DateTime>(type: "datetime", nullable: true),
                    Clear = table.Column<int>(type: "int", nullable: true),
                    Scrap = table.Column<int>(type: "int", nullable: true),
                    ThriftStore = table.Column<int>(type: "int", nullable: true),
                    Samples = table.Column<int>(type: "int", nullable: true),
                    Cancelled = table.Column<bool>(type: "bit", nullable: true),
                    Complete = table.Column<bool>(type: "bit", nullable: true),
                    HRDCompletedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AllCasesAccountedFor = table.Column<bool>(type: "bit", nullable: true),
                    DateReleased = table.Column<DateTime>(type: "datetime", nullable: true),
                    TestingCost = table.Column<decimal>(type: "money", nullable: true),
                    LaborHours = table.Column<decimal>(type: "numeric(10,4)", nullable: true),
                    GSTDRequired = table.Column<bool>(type: "bit", nullable: true),
                    HighRisk = table.Column<bool>(type: "bit", nullable: true),
                    PhysicalIsolationNeeded = table.Column<bool>(type: "bit", nullable: true),
                    Physical5DaysGSTD = table.Column<bool>(type: "bit", nullable: true),
                    LastChange = table.Column<DateTime>(type: "datetime", nullable: true),
                    LastChangeUser = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ReworkApprovedDate = table.Column<DateTime>(type: "datetime", nullable: true),
                    CodingType = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CodingDetails = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    YearHeld = table.Column<string>(type: "nvarchar(4)", maxLength: 4, nullable: true),
                    QAComments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryNotification = table.Column<bool>(type: "bit", nullable: true),
                    IsPest = table.Column<bool>(type: "bit", nullable: true),
                    IsSMI = table.Column<bool>(type: "bit", nullable: true),
                    IsNR = table.Column<bool>(type: "bit", nullable: true),
                    IsFM = table.Column<bool>(type: "bit", nullable: true),
                    IsMicro = table.Column<bool>(type: "bit", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    TimeOfIncident = table.Column<DateTime>(type: "datetime2", nullable: true),
                    BUManager = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    FertDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LineSupervisor = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Area = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AreaIfOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdditionalDescription = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    DetailedDescription = table.Column<string>(type: "nvarchar(75)", maxLength: 75, nullable: true),
                    ContinuousRun = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DateCompleted = table.Column<DateTime>(type: "datetime", nullable: true),
                    NumberOfDaysHeld = table.Column<int>(type: "int", nullable: true),
                    Donate = table.Column<int>(type: "int", nullable: true),
                    Total = table.Column<int>(type: "int", nullable: true),
                    MonthHeld = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    WeekHeld = table.Column<int>(type: "int", nullable: true),
                    NumberOfDayToReworkApproval = table.Column<int>(type: "int", nullable: true),
                    CaseCount = table.Column<int>(type: "int", nullable: true),
                    ReasonAction = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsApprovalRequestByQa = table.Column<bool>(type: "bit", nullable: true),
                    IsPlantManagerAprpoval = table.Column<bool>(type: "bit", nullable: true),
                    IsPlantControllerApproval = table.Column<bool>(type: "bit", nullable: true),
                    IsDestroyed = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedByQAWho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByQAWhen = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApprovedByPlantManagerWho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedPlantManagerQAWhen = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApprovedByPlantControllerWho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByPlantControllerWhen = table.Column<DateTime>(type: "datetime", nullable: true),
                    ApprovedByDistroyedWho = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ApprovedByDistroyedWhen = table.Column<DateTime>(type: "datetime", nullable: true),
                    Comments = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CasesHeld = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    POs = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PestType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PCOContactedImmediately = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ProductAdultered = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WhereFound = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IfYesAffectedProduct = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    MaterialNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RawMaterialDescription = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    BatchLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendorNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendorName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VendorSiteNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsInspections = table.Column<bool>(type: "bit", nullable: true),
                    IsXray = table.Column<bool>(type: "bit", nullable: true),
                    IsMetalDetector = table.Column<bool>(type: "bit", nullable: true),
                    FMType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Equipment = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    EquipmentIfOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ROHMaterial = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PiecesTotal = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    RawBatchLot = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HazardousSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Responsibility = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NonHazardousSize = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateReceived = table.Column<DateTime>(type: "datetime", nullable: true),
                    InspectorsName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    NRCategory = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Tagged = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TagNumber = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Response = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    HoldConcern = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DayOfWeek = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    When = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    WhenOther = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    DateOfResample = table.Column<DateTime>(type: "datetime", nullable: true),
                    MeatComponent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    VeggieComponent = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    SauceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StarchType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    AdditionalComments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LaborCost",
                columns: table => new
                {
                    Year = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    LaborCost = table.Column<decimal>(type: "smallmoney", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LaborCost", x => x.Year);
                });

            migrationBuilder.CreateTable(
                name: "Product",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nchar(4)", fixedLength: true, maxLength: 4, nullable: false, defaultValueSql: "((0))"),
                    GPN = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false, defaultValueSql: "((0))"),
                    Description = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    CostPerCase = table.Column<decimal>(type: "money", nullable: true),
                    Country = table.Column<string>(type: "nchar(2)", fixedLength: true, maxLength: 2, nullable: true),
                    NoBBDate = table.Column<bool>(type: "bit", nullable: true),
                    Holiday = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Product", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Security",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<string>(type: "nvarchar(5)", maxLength: 5, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Administrator = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    QAManager = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    QATGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    QAProdRelGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ProdTLGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ReworkMembersGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    PhysicalGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    Physical5Grp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    GSTDNotificationGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    DeleteHRD = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    EditHRDApproveRework = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    ProdMaintenanceList = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    GSTDGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    BUMGrp = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    LaborCostList = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))"),
                    Notification = table.Column<bool>(type: "bit", nullable: true, defaultValueSql: "((0))")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Security", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Settings",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Value = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Type = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Settings", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TestCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Year = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    TestCost = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TestCost", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DropDownItem",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DropDownTypeId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SortOrder = table.Column<short>(type: "smallint", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DropDownItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK__DropDownI__DropD__286302EC",
                        column: x => x.DropDownTypeId,
                        principalTable: "DropDownType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRDDC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumberOfCases = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDDC", x => x.Id);
                    table.ForeignKey(
                        name: "FK__HRDDC__HRDId__30F848ED",
                        column: x => x.HRDId,
                        principalTable: "HRD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRDFC",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    Location = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    NumberOfCases = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDFC", x => x.Id);
                    table.ForeignKey(
                        name: "FK__HRDFC__HRDId__2E1BDC42",
                        column: x => x.HRDId,
                        principalTable: "HRD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRDNotes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    Category = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserID = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Date = table.Column<DateTime>(type: "datetime", nullable: true),
                    FileName = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Path = table.Column<string>(type: "nvarchar(400)", maxLength: 400, nullable: true),
                    Size = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDNotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK__HRDNotes__HRDId__398D8EEE",
                        column: x => x.HRDId,
                        principalTable: "HRD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRDPO",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDPO", x => x.Id);
                    table.ForeignKey(
                        name: "FK__HRDPO__HRDId__2B3F6F97",
                        column: x => x.HRDId,
                        principalTable: "HRD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "HRDTestCost",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HRDId = table.Column<int>(type: "int", nullable: false),
                    TestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Qty = table.Column<int>(type: "int", nullable: true),
                    Cost = table.Column<decimal>(type: "money", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDTestCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK__HRDTestCo__HRDId__4222D4EF",
                        column: x => x.HRDId,
                        principalTable: "HRD",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_DropDownItem_DropDownTypeId",
                table: "DropDownItem",
                column: "DropDownTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDDC_HRDId",
                table: "HRDDC",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDFC_HRDId",
                table: "HRDFC",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDNotes_HRDId",
                table: "HRDNotes",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDPO_HRDId",
                table: "HRDPO",
                column: "HRDId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDTestCost_HRDId",
                table: "HRDTestCost",
                column: "HRDId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DropDownItem");

            migrationBuilder.DropTable(
                name: "HRDDC");

            migrationBuilder.DropTable(
                name: "HRDFC");

            migrationBuilder.DropTable(
                name: "HRDNotes");

            migrationBuilder.DropTable(
                name: "HRDPO");

            migrationBuilder.DropTable(
                name: "HRDTestCost");

            migrationBuilder.DropTable(
                name: "LaborCost");

            migrationBuilder.DropTable(
                name: "Product");

            migrationBuilder.DropTable(
                name: "Security");

            migrationBuilder.DropTable(
                name: "Settings");

            migrationBuilder.DropTable(
                name: "TestCost");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "DropDownType");

            migrationBuilder.DropTable(
                name: "HRD");
        }
    }
}
