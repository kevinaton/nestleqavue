using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class CountryLookupTypeDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                            INSERT [dbo].DropDownType ([Name]) VALUES (N'Country')
                            GO

                            DECLARE @country int;
                            SET @country = (SELECT Id FROM DropDownType WHERE [Name] = 'Country');
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@country, N'US', 1, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@country, N'CA', 2, 1)
                            GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DELETE from DropDownItem where DropDownTypeId IN (SELECT Id FROM DropDownType WHERE [Name] 'Country')
                        DELETE FROM DropDownType WHERE [Name] = 'Country'";

            migrationBuilder.Sql(sql);
        }
    }
}
