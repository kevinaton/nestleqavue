﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class AddROHMaterial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HRDROHMaterial",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Hrdid = table.Column<int>(type: "int", nullable: false),
                    DropDownItemId = table.Column<int>(type: "int", nullable: false),
                    Material = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRDROHMaterial", x => x.Id);
                    table.ForeignKey(
                        name: "FK_HRDROHMaterial_DropDownItem_DropDownItemId",
                        column: x => x.DropDownItemId,
                        principalTable: "DropDownItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HRDROHMaterial_HRD_Hrdid",
                        column: x => x.Hrdid,
                        principalTable: "HRD",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HRDROHMaterial_DropDownItemId",
                table: "HRDROHMaterial",
                column: "DropDownItemId");

            migrationBuilder.CreateIndex(
                name: "IX_HRDROHMaterial_Hrdid",
                table: "HRDROHMaterial",
                column: "Hrdid");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HRDROHMaterial");
        }
    }
}
