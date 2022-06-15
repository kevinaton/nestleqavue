using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class HRD_Update_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Responsibility",
                table: "HRD",
                newName: "FMSource");

            migrationBuilder.RenameColumn(
                name: "RawBatchLot",
                table: "HRD",
                newName: "FMVendorBatch");

            migrationBuilder.RenameColumn(
                name: "BatchLot",
                table: "HRD",
                newName: "SMIVendorBatch");

            migrationBuilder.AddColumn<string>(
                name: "YearOfIncident",
                table: "HRD",
                type: "nvarchar(4)",
                maxLength: 4,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "YearOfIncident",
                table: "HRD");

            migrationBuilder.RenameColumn(
                name: "FMSource",
                table: "HRD",
                newName: "Responsibility");

            migrationBuilder.RenameColumn(
                name: "FMVendorBatch",
                table: "HRD",
                newName: "RawBatchLot");

            migrationBuilder.RenameColumn(
                name: "SMIVendorBatch",
                table: "HRD",
                newName: "BatchLot");
        }
    }
}
