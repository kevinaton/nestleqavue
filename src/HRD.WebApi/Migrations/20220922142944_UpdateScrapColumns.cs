using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class UpdateScrapColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReasonAction",
                table: "HRD");

            migrationBuilder.RenameColumn(
                name: "CaseCount",
                table: "HRD",
                newName: "ScrapCaseCount");

            migrationBuilder.AlterColumn<string>(
                name: "RawMaterialDescription",
                table: "HRD",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ScrapReasonAction",
                table: "HRD",
                type: "nvarchar(250)",
                maxLength: 250,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScrapReasonAction",
                table: "HRD");

            migrationBuilder.RenameColumn(
                name: "ScrapCaseCount",
                table: "HRD",
                newName: "CaseCount");

            migrationBuilder.AlterColumn<string>(
                name: "RawMaterialDescription",
                table: "HRD",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(250)",
                oldMaxLength: 250,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReasonAction",
                table: "HRD",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
