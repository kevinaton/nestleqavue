using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RolesForHrdScrapEmailDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"---------------------------------------------------------------------------------------------------------
                        -----------------------------------------QA Scrap Notification-------------------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'QAScrapNotification')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'QAScrapNotification', N'QA Scrap Notification', 1)
						END                                          
                        GO

                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Plant Manager Scrap Notification--------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'PlantManagerScrapNotification')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'PlantManagerScrapNotification', N'Plant Manager Scrap Notification', 1)
						END                                          
                        GO

                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Controller Scrap Notification-----------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'ControllerScrapNotification')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'ControllerScrapNotification', N'Controller Scrap Notification', 1)
						END                                          
                        GO

                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Scrap Destroyed Notification------------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'ScrapDestroyedNotification')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'ScrapDestroyedNotification', N'Scrap Destroyed Notification', 1)
						END                                          
                        GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
