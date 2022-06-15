using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RemoveHazardousColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HazardousSize",
                table: "HRD");

            migrationBuilder.DropColumn(
                name: "NonHazardousSize",
                table: "HRD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "HazardousSize",
                table: "HRD",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NonHazardousSize",
                table: "HRD",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
