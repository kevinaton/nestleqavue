using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RolesAndPermissionsHRDScrapCheckboxes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = @"---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Approval request by QA------------------------------------------
                        ---------------------------------------------------------------------------------------------------------

                        DECLARE @qaManager int;
                        SET @qaManager = (SELECT Id FROM Roles WHERE [Name] = 'QAManager');
						IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @qaManager AND [Name] = 'Pages.HRD.ApprovalRequestByQa')
						BEGIN
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.ApprovalRequestByQa', 1)
						END
                        GO


                        DECLARE @qaProductRelease int;
                        SET @qaProductRelease = (SELECT Id FROM Roles WHERE [Name] = 'QAProductRelease');
						IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @qaProductRelease AND [Name] = 'Pages.HRD.ApprovalRequestByQa')
						BEGIN
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD.ApprovalRequestByQa', 1)
						END
                        GO
                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Plant Manager---------------------------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'PlantManager')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'PlantManager', N'Plant Manager', 1)
						END                                          
                        GO

                        DECLARE @plantManager int;
                        SET @plantManager = (SELECT Id FROM Roles WHERE [Name] = 'PlantManager');
						IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @plantManager)
						BEGIN
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@plantManager, N'Pages.HRD', 1)
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@plantManager, N'Pages.HRD.PlantManagerApproval', 1)
						END
                        GO

                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Plant Controller------------------------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'PlantController')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'PlantController', N'Plant Controller', 1)
						END                                          
                        GO

                        DECLARE @plantController int;
                        SET @plantController = (SELECT Id FROM Roles WHERE [Name] = 'PlantController');
						IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @plantController)
						BEGIN
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@plantController, N'Pages.HRD', 1)
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@plantController, N'Pages.HRD.PlantControllerApproval', 1)
						END
                        GO


                        ---------------------------------------------------------------------------------------------------------
                        -----------------------------------------Material Handler---------------------------------------------------
                        ---------------------------------------------------------------------------------------------------------
						IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'MaterialHandler')
						BEGIN
							INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'MaterialHandler', N'Material Handler', 1)
						END                                          
                        GO

                        DECLARE @materialHandler int;
                        SET @materialHandler = (SELECT Id FROM Roles WHERE [Name] = 'MaterialHandler');
						IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @materialHandler)
						BEGIN
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@materialHandler, N'Pages.HRD', 1)
							INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@materialHandler, N'Pages.HRD.Destroyed', 1)
						END
                        GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
