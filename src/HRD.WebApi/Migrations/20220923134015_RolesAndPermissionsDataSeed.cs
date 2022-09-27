using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HRD.WebApi.Migrations
{
    public partial class RolesAndPermissionsDataSeed : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Administrator---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'Administrator')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'Administrator', N'Administrator', 1)
							END
                            GO

                            DECLARE @administrator int;
                            SET @administrator = (SELECT Id FROM Roles WHERE [Name] = 'Administrator');
                            IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @administrator)
							BEGIN
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.QARecords', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.QARecords.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.QARecords.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Products', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Products.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Products.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Labor', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Labor.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Labor.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Testing', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Testing.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Testing.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Roles', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Roles.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Roles.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Users', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Users.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.Users.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.LookupLists', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.LookupLists.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.LookupLists.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.CasesAndCostHeldByCategory', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.CasesAndCostHeldByCategory.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.CasesAndCostHeldByCategory.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.MicrobeCases', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.MicrobeCases.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.MicrobeCases.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.FMCases', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.FMCases.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.FMCases.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.PestLog', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.PestLog.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.PestLog.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD.Read', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD.Edit', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD.Delete', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD.ApproveRework', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.HRD.EmailNotification', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.BestBeforeCalculator', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.BestBeforeCalculator.BasedOnCountry', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.BestBeforeCalculator.BasedOnGPN', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.GSTD', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.GSTD.Member', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.GSTD.Notification', 1)
                                INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@administrator, N'Pages.BusinessUnitManager', 1)
                            END
                            GO

                            ---------------------------------------------------------------------------------------------------------
                            -------------------------------------------QA Manager----------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'QAManager')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'QAManager', N'QA Manager', 1)
							END                            
                            GO

                            DECLARE @qaManager int;
                            SET @qaManager = (SELECT Id FROM Roles WHERE [Name] = 'QAManager');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @qaManager)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Products', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Products.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Products.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Labor', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Labor.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Labor.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Testing', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Testing.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.Testing.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.CasesAndCostHeldByCategory', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.CasesAndCostHeldByCategory.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.CasesAndCostHeldByCategory.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.MicrobeCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.MicrobeCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.MicrobeCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.FMCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.FMCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.FMCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.PestLog', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.PestLog.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.PestLog.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.Delete', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.ApproveRework', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.HRD.EmailNotification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.BestBeforeCalculator', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.BestBeforeCalculator.BasedOnCountry', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.BestBeforeCalculator.BasedOnGPN', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.GSTD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.GSTD.Member', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.GSTD.Notification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaManager, N'Pages.BusinessUnitManager', 1)
							END
                            GO
                            ---------------------------------------------------------------------------------------------------------
                            ------------------------------------------Specialists----------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'Specialists')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'Specialists', N'Specialists', 1)
							END
                            GO    

                            DECLARE @specialists int;
                            SET @specialists = (SELECT Id FROM Roles WHERE [Name] = 'Specialists');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @specialists)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Products', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Products.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Products.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Labor', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Labor.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Labor.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Testing', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Testing.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.Testing.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.CasesAndCostHeldByCategory', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.CasesAndCostHeldByCategory.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.CasesAndCostHeldByCategory.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.MicrobeCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.MicrobeCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.MicrobeCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.FMCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.FMCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.FMCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.PestLog', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.PestLog.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.PestLog.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.HRD.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.HRD.Delete', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.HRD.EmailNotification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.BestBeforeCalculator', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.BestBeforeCalculator.BasedOnCountry', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.GSTD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.GSTD.Member', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.GSTD.Notification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@specialists, N'Pages.BusinessUnitManager', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------QA Product Release---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'QAProductRelease')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'QAProductRelease', N'QA Product Release', 1)
							END
                            GO

                            DECLARE @qaProductRelease int;
                            SET @qaProductRelease = (SELECT Id FROM Roles WHERE [Name] = 'QAProductRelease');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @qaProductRelease)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Products', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Products.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Products.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Labor', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Labor.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Labor.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Testing', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Testing.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.Testing.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.LookupLists', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.LookupLists.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.LookupLists.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.CasesAndCostHeldByCategory', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.CasesAndCostHeldByCategory.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.CasesAndCostHeldByCategory.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.MicrobeCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.MicrobeCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.MicrobeCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.FMCases', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.FMCases.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.FMCases.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.PestLog', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.PestLog.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.PestLog.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD.Delete', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.HRD.EmailNotification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.BestBeforeCalculator', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.BestBeforeCalculator.BasedOnCountry', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.BestBeforeCalculator.BasedOnGPN', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.GSTD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.GSTD.Member', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.GSTD.Notification', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@qaProductRelease, N'Pages.BusinessUnitManager', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Rework Members---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'ReworkMembers')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'ReworkMembers', N'Rework Members', 1)
							END                            
                            GO

                            DECLARE @reworkMembers int;
                            SET @reworkMembers = (SELECT Id FROM Roles WHERE [Name] = 'ReworkMembers');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @reworkMembers)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@reworkMembers, N'Pages.HRD.Edit', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------GSTD Notification-----------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'GSTDNotification')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'GSTDNotification', N'GSTD Notification', 1)
							END          
                            GO

                            DECLARE @gstdNotification int;
                            SET @gstdNotification = (SELECT Id FROM Roles WHERE [Name] = 'GSTDNotification');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @gstdNotification)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@gstdNotification, N'Pages.GSTD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@gstdNotification, N'Pages.GSTD.Notification', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Delete HRD---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'DeleteHRD')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'DeleteHRD', N'Delete HRD', 1)
							END
                            GO

                            DECLARE @deleteHRD int;
                            SET @deleteHRD = (SELECT Id FROM Roles WHERE [Name] = 'DeleteHRD');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @deleteHRD)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.HRD.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@deleteHRD, N'Pages.HRD.Delete', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Edit HRDs / Approve Reworks-------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'ApproveReworks')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'ApproveReworks', N'Edit HRDs / Approve Reworks', 1)
							END                            
                            GO

                            DECLARE @approveReworks int;
                            SET @approveReworks = (SELECT Id FROM Roles WHERE [Name] = 'ApproveReworks');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @approveReworks)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.QARecords', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.QARecords.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.QARecords.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.HRD.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.HRD.Edit', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@approveReworks, N'Pages.HRD.ApproveRework', 1)
							END
                            GO



							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Product List Maintenance----------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'ProductListMaintenance')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'ProductListMaintenance', N'Product List Maintenance', 1)
							END                                                        
                            GO

                            DECLARE @productMaintenance int;
                            SET @productMaintenance = (SELECT Id FROM Roles WHERE [Name] = 'ProductListMaintenance');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @productMaintenance)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@productMaintenance, N'Pages.Products', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@productMaintenance, N'Pages.Products.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@productMaintenance, N'Pages.Products.Edit', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------GSTD Member---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'GSTD')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'GSTD', N'GSTD Member', 1)
							END                                                                                    
                            GO

                            DECLARE @gstd int;
                            SET @gstd = (SELECT Id FROM Roles WHERE [Name] = 'GSTD');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @gstd)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@gstd, N'Pages.GSTD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@gstd, N'Pages.GSTD.Member', 1)
							END
                            GO

							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Business Unit Managers---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'BUManager')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'BUManager', N'Business Unit Managers', 1)
							END                                                                                                                
                            GO

                            DECLARE @buManager int;
                            SET @buManager = (SELECT Id FROM Roles WHERE [Name] = 'BUManager');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @buManager)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@buManager, N'Pages.BusinessUnitManager', 1)
							END
                            GO

							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Labor Cost List---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'LaborCostList')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'LaborCostList', N'Labor Cost List', 1)
							END                                      
                            GO

                            DECLARE @laborCostList int;
                            SET @laborCostList = (SELECT Id FROM Roles WHERE [Name] = 'LaborCostList');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @laborCostList)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@laborCostList, N'Pages.Labor', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@laborCostList, N'Pages.Labor.Read', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@laborCostList, N'Pages.Labor.Edit', 1)
							END
                            GO


							---------------------------------------------------------------------------------------------------------
                            -----------------------------------------Notification---------------------------------------------------
                            ---------------------------------------------------------------------------------------------------------
							IF NOT EXISTS(SELECT Id FROM Roles WHERE [Name] = 'Notification')
							BEGIN
								INSERT [dbo].Roles([Name], [DisplayName], [IsStatic]) VALUES (N'Notification', N'Notification', 1)
							END                                          
                            GO

                            DECLARE @notification int;
                            SET @notification = (SELECT Id FROM Roles WHERE [Name] = 'Notification');
							IF NOT EXISTS(SELECT Id FROM [Permissions] WHERE RoleId = @notification)
							BEGIN
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@notification, N'Pages.HRD', 1)
								INSERT [dbo].[Permissions] ([RoleId], [Name], [IsGranted]) VALUES (@notification, N'Pages.HRD.EmailNotification', 1)
							END
                            GO";

            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
