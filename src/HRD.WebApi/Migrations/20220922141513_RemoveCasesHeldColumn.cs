using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RemoveCasesHeldColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CasesHeld",
                table: "HRD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CasesHeld",
                table: "HRD",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);
        }
    }
}
