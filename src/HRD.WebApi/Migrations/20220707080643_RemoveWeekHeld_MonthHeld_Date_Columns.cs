using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RemoveWeekHeld_MonthHeld_Date_Columns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "HRD");

            migrationBuilder.DropColumn(
                name: "MonthHeld",
                table: "HRD");

            migrationBuilder.DropColumn(
                name: "WeekHeld",
                table: "HRD");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "HRD",
                type: "datetime",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MonthHeld",
                table: "HRD",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WeekHeld",
                table: "HRD",
                type: "int",
                nullable: true);
        }
    }
}
