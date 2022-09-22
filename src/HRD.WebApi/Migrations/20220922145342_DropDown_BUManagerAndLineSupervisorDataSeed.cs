using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class DropDown_BUManagerAndLineSupervisorDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"
                            INSERT [dbo].DropDownType ([Name]) VALUES (N'BU Manager')
                            INSERT [dbo].DropDownType ([Name]) VALUES (N'Line Supervisor')
                            GO

                            DECLARE @buManager int;
                            DECLARE @lineSupervisor int;
                            SET @buManager = (SELECT Id FROM DropDownType WHERE [Name] = 'BU Manager');
                            SET @lineSupervisor = (SELECT Id FROM DropDownType WHERE [Name] = 'Line Supervisor');
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@buManager, N'Luke Mitchell', 1, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@buManager, N'Jimmie Coulter', 2, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@buManager, N'Open Spot', 3, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Kizzie Posey', 1, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Jennifer Garner', 2, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Moses Mekki', 3, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Rick Harris', 4, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Laquanda Lane', 5, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Lawanda Strand', 6, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Eric Johnson', 7, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Dery Rodrigues', 8, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Marvin Johnson', 9, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Michael Rainey', 10, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Barney Bishop', 11, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Josh Jackson', 12, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Spencer White', 13, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Teddy Palivos', 14, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Phillip Martin', 15, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Maurice Haynes', 16, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Phillip Davis', 17, 1)
                            INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (@lineSupervisor, N'Aubrey Capers', 18, 1)
                            GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            var sql = @"DELETE from DropDownItem where DropDownTypeId IN (SELECT Id FROM DropDownType WHERE [Name] IN ('BU Manager', 'Line Supervisor'))
                        DELETE FROM DropDownType WHERE [Name] IN ('BU Manager', 'Line Supervisor')";

            migrationBuilder.Sql(sql);
        }
    }
}
