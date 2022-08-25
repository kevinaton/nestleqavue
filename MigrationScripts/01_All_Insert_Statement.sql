USE HRD

SET IDENTITY_INSERT dbo.HRD ON 
INSERT INTO dbo.HRD 
	(
		[Id],
		[Plant],
		[Originator],
		[IsHRD],
		[DayCode],
		[Line],
		[Shift],
		[LCL],
		[Schedule],
		[HourCode],
		[Cases],
		[UnpalletizedProductHeld],
		[RejectOrNinePack],
		[ReworkOutsideNormalProduction],
		[ContactOtherPlant],
		[GLOBENum],
		[ShortDescription],
		[Problem],
		[ReworkInstructions],
		[ReworkApproved],
		[ReworkApprovedBy],
		[ReworkStarted],
		[ReworkComplete],
		[ReworkCompletedBy],
		[CauseMan],
		[CauseMethod],
		[CauseMaterial],
		[CauseMachine],
		[TLforFU],
		[Classification],
		[FTQCases],
		[NonFTQCases],
		[DCDate],
		[DCUser],
		[FCDate],
		[FCUser],
		[OtherHRDAffected],
		[OtherHRDNum],
		[DateHeld],
		[HoldCategory],
		[HoldSubCategory],
		[CostofProductonHold],
		[DateofDisposition],
		[Clear],
		[Scrap],
		[ThriftStore],
		[Samples],
		[Cancelled],
		[Complete],
		[HRDCompletedBy],
		[AllCasesAccountedFor],
		[DateReleased],
		[TestingCost],
		[LaborHours],
		[GSTDRequired],
		[HighRisk],
		[PhysicalIsolationNeeded],
		[Physical5DaysGSTD],
		[LastChange],
		[LastChangeUser],
		[ReworkApprovedDate],
		[CodingType],
		[CodingDetails],
		[YearHeld],
		[QAComments],
		[SecondaryNotification]
	)
SELECT 
		[AutoNum],
		[Plant],
		[Originator],
		1,
		[DayCode],
		[Line],
		[Shift],
		[LCL],
		[Schedule],
		[HourCode],
		[Cases],
		[UnpalletizedProductHeld],
		[RejectOrNinePack],
		[ReworkOutsideNormalProduction],
		[ContactOtherPlant],
		[GLOBENum],
		[ShortDescription],
		[Problem],
		[ReworkInstructions],
		[ReworkApproved],
		[ReworkApprovedBy],
		[ReworkStarted],
		[ReworkComplete],
		[ReworkCompletedBy],
		[CauseMan],
		[CauseMethod],
		[CauseMaterial],
		[CauseMachine],
		[TLforFU],
		[Classification],
		[FTQCases],
		[NonFTQCases],
		[DCDate],
		[DCUser],
		[FCDate],
		[FCUser],
		[OtherHRDAffected],
		[OtherHRDNum],
		[DateHeld],
		[HoldCategory],
		[HoldSubCategory],
		[CostofProductonHold],
		[DateofDisposition],
		[Clear],
		[Scrap],
		[ThriftStore],
		[Samples],
		[Cancelled],
		[Complete],
		[HRDCompletedBy],
		[AllCasesAccountedFor],
		[DateReleased],
		[TestingCost],
		[LaborHours],
		[GSTDRequired],
		[HighRisk],
		[PhysicalIsolationNeeded],
		[Physical5DaysGSTD],
		[LastChange],
		[LastChangeUser],
		[ReworkApprovedDate],
		[CodingType],
		[CodingDetails],
		[YearHeld],
		[QAComments],
		[SecondaryNotification]
FROM OldHRD.dbo.HRD oldTable
		WHERE oldTable.AutoNum NOT IN (SELECT ID FROM dbo.HRD)

SET IDENTITY_INSERT dbo.HRD OFF
GO

/*********************************************/
/***** HRDPO  ********/
/*********************************************/
INSERT INTO dbo.HRDPO(HRDId, PONumber)
SELECT AutoNum, PONumber
FROM 
   (SELECT AutoNum, PO1, PO2
   FROM OldHRD.dbo.HRD) ot
UNPIVOT
   (
	PONumber For PONumbers IN (PO1, PO2)
   )AS unpvt;
GO

/*********************************************/
/***** HRDFC  ********/
/*********************************************/
INSERT INTO dbo.HRDFC(HRDId, [Location], NumberOfCases)
SELECT AutoNum, SUBSTRING([Location], 3, DATALENGTH([Location])- 2) as [Location], NumberOfCases
FROM 
   (SELECT AutoNum, FC5070, FC5702, FC5847, FC5687, FC5222,
			CAST(SUBSTRING(FC5222Dest, PATINDEX('%[0-9]%', FC5222Dest), PATINDEX('%[0-9][^0-9]%', FC5222Dest + 't') - PATINDEX('%[0-9]%', 
                    FC5222Dest) + 1) as int) AS FC5222Dest
			,FCInTransit, FCXC, FCLX,
			CAST(SUBSTRING(FCLXDest, PATINDEX('%[0-9]%', FCLXDest), PATINDEX('%[0-9][^0-9]%', FCLXDest + 't') - PATINDEX('%[0-9]%', 
                    FCLXDest) + 1) as int) AS FCLXDest
			,FCXD,
			CAST(SUBSTRING(FCXDDest, PATINDEX('%[0-9]%', FCXDDest), PATINDEX('%[0-9][^0-9]%', FCXDDest + 't') - PATINDEX('%[0-9]%', 
                    FCXDDest) + 1) as int) AS FCXDDest
   FROM OldHRD.dbo.HRD) ot
UNPIVOT
   (
	NumberOfCases For [Location]IN (FC5070, FC5702, FC5847, FC5687, FC5222, FC5222Dest, FCInTransit, FCXC, FCLX, FCLXDest, FCXD, FCXDDest)
   )AS unpvt where NumberOfCases > 0;

GO

/*********************************************/
/***** HRDDC  ********/
/*********************************************/
INSERT INTO dbo.HRDDC(HRDId, [Location], NumberOfCases)
SELECT AutoNum, SUBSTRING([Location], 3, DATALENGTH([Location])- 2) as [Location], NumberOfCases
FROM 
   (SELECT AutoNum, DC5070, DC5702, DC5847, DC5687, DC5222,
			CAST(SUBSTRING(DC5222Dest, PATINDEX('%[0-9]%', DC5222Dest), PATINDEX('%[0-9][^0-9]%', DC5222Dest + 't') - PATINDEX('%[0-9]%', 
                    DC5222Dest) + 1) as int) AS DC5222Dest
			,DCInTransit, DCXC, DCLX,
			CAST(SUBSTRING(DCLXDest, PATINDEX('%[0-9]%', DCLXDest), PATINDEX('%[0-9][^0-9]%', DCLXDest + 't') - PATINDEX('%[0-9]%', 
                    DCLXDest) + 1) as int) AS DCLXDest
			,DCXD,
			CAST(SUBSTRING(DCXDDest, PATINDEX('%[0-9]%', DCXDDest), PATINDEX('%[0-9][^0-9]%', DCXDDest + 't') - PATINDEX('%[0-9]%', 
                    DCXDDest) + 1) as int) AS DCXDDest
   FROM OldHRD.dbo.HRD) ot
UNPIVOT
   (
	NumberOfCases For [Location]IN (DC5070, DC5702, DC5847, DC5687, DC5222, DC5222Dest, DCInTransit, DCXC, DCLX, DCLXDest, DCXD, DCXDDest)
   )AS unpvt where NumberOfCases > 0;

GO

/*********************************************/
/***** LaborCost  ********/
/*********************************************/
INSERT INTO [dbo].[LaborCost] (Year, LaborCost)
	SELECT Year, LaborCost
		FROM OldHRD.dbo.LaborCost

GO

/*********************************************/
/***** PRODUCT  ********/
/*********************************************/
INSERT INTO [dbo].[Product] ([Year], GPN, [Description], CostPerCase, Country, NoBBDate, Holiday)
	SELECT [Year], GPN, [Description], CostPerCase, Country, NoBBDate, Holiday
		FROM OldHRD.dbo.Product
GO

/*********************************************/
/***** HRDNotes  ********/
/*********************************************/
SET IDENTITY_INSERT dbo.HRDNotes ON 
INSERT INTO [dbo].[HRDNotes] ([Id], HRDId, [Category], [Description], UserID, [Date], [FileName], [Path], Size)
	SELECT AutoNum, HRDNum, Category, [Description], UserID, [Date], [FileName], [Path], Size
		FROM OldHRD.dbo.tblNotesList notes
		WHERE notes.HRDNum in (select AutoNum from OldHRD.dbo.HRD)

SET IDENTITY_INSERT dbo.HRDNotes OFF		
GO

/*********************************************/
/***** Settings  ********/
/*********************************************/
SET IDENTITY_INSERT dbo.Settings ON 
INSERT INTO [dbo].[Settings] ([ID], [Name], [Description], [Value], [Type])
	SELECT [ID], [Name], [Description], [Value], [Type]
		FROM OldHRD.dbo.tConstants

SET IDENTITY_INSERT dbo.Settings OFF 
GO

/*********************************************/
/***** Security  ********/
/*********************************************/
SET IDENTITY_INSERT dbo.[Security] ON 
INSERT INTO [dbo].[Security]
           ([ID], [Type], [Name], [Administrator], [QAManager], [QATGrp], [QAProdRelGrp], [ProdTLGrp], [ReworkMembersGrp],
			[PhysicalGrp], [Physical5Grp], [GSTDNotificationGrp], [DeleteHRD], [EditHRDApproveRework], [ProdMaintenanceList],
			[GSTDGrp], [BUMGrp], [LaborCostList], [Notification])
	SELECT	SecurityID, [Type], [Name], [Administrator], [QAManager], [QATGrp], [QAProdRelGrp], [ProdTLGrp], [ReworkMembersGrp],
			[PhysicalGrp], [Physical5Grp], [GSTDNotificationGrp], [DeleteHRD], [EditHRDApproveRework], [ProdMaintenanceList],
			[GSTDGrp], [BUMGrp], [LaborCostList], [Notification]
		   FROM OldHRD.dbo.tSecurity

SET IDENTITY_INSERT dbo.[Security] OFF
     
GO

/*********************************************/
/***** TestCost  ********/
/*********************************************/
SET IDENTITY_INSERT dbo.TestCost ON 
INSERT INTO [dbo].[TestCost] (Id, [Year], TestName, TestCost)
	SELECT EntryID, [Year], TestName, TestCost
		FROM OldHRD.dbo.tTestCost

SET IDENTITY_INSERT dbo.TestCost OFF 
GO

/*********************************************/
/***** HRDTestCost  ********/
/*********************************************/
SET IDENTITY_INSERT dbo.HRDTestCost ON 
INSERT INTO [dbo].[HRDTestCost] ([Id], HRDId, TestName, Qty, Cost)
	SELECT EntryID, HRDId, TestName, Qty, Cost
		FROM OldHRD.dbo.tTestTracking trk
		WHERE trk.HRDID in (select AutoNum from OldHRD.dbo.HRD)

SET IDENTITY_INSERT dbo.HRDTestCost OFF		
GO

/*********************************************/
/***** User  ********/
/*********************************************/
INSERT INTO [dbo].[Users] ([UserID], Name)
	SELECT UserID,Name
		FROM OldHRD.dbo.Users
GO

/*********************************************/
/***** DROPDOWNTYPE  ********/
/*********************************************/

SET IDENTITY_INSERT [dbo].DropDownType ON 
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (1,  N'Category')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (2,  N'SubCategory')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (3,  N'Classification')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (4,  N'Line')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (5,  N'Shift')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (6,  N'LCL')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (7,  N'YesNo')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (8,  N'YesNoNA')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (9,  N'CodingType')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (10, N'CodingDetails')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (11, N'Area')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (12, N'NR Category')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (13, N'Tagged')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (14, N'FM Type')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (15, N'Responsibility')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (16, N'Equipment')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (17, N'Pest Type')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (18, N'Hold or Concern')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (19, N'Day of Week')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (20, N'When')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (21, N'Sauce Type')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (22, N'Starch Type')
INSERT [dbo].DropDownType ([Id], [Name]) VALUES (23, N'Organism')
SET IDENTITY_INSERT [dbo].DropDownType OFF 
GO


/*********************************************/
/***** DROPDOWNItem  ********/
/*********************************************/
INSERT INTO [dbo].[DropDownItem] ([DropDownTypeId], [Value], SortOrder, IsActive)
SELECT DISTINCT t.[Id] as DropDownTypeId ,PrimaryCategory as [Value], PrimarySort as SortOrder, 1 as IsActive
FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'Category'
	WHERE PrimaryCategory is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, SubCategory as [Value], SubCategorySort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'SubCategory'
	WHERE SubCategory is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, [Classification] as [Value], ClassificationSort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'Classification'
	WHERE [Classification] is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, Line as [Value], LineSort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'Line'
	WHERE Line is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, [Shift] as [Value], ShiftSort as SortOrder, ShiftActive as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'Shift'
	WHERE [Shift] is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, LCLLine as [Value], LCLSort as SortOrder, LCLActive as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'LCL'
	WHERE LCLLine is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, Schedule as [Value], ScheduleSort as SortOrder, ScheduleActive as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'YesNo'
	WHERE Schedule is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, YNNA as [Value], YNNASort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'YesNoNa'
	WHERE YNNA is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, CodingType as [Value], CodingTypeSort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'CodingType'
	WHERE CodingType is not null
UNION ALL
SELECT DISTINCT t.[Id] as DropDownTypeId, CodingDetails as [Value], CodingDetailsSort as SortOrder, 1 as IsActive
	FROM OldHRD.dbo.DDLs
	JOIN [dbo].DropDownType t ON t.Name = 'CodingDetails'
	WHERE CodingDetails is not null
GO

USE [HRD]                                                                               
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (4, N'WH', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (4, N'CP', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (4, N'Grounds', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (4, N'Infrastructure', 11, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (4, N'Other', 12, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (5, N'Multiple', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (1, N'GMP', 19, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (1, N'Housekeeping', 20, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (1, N'Pest Sighting', 21, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (1, N'Recipe Deviation', 22, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (1, N'Other…', 23, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (9, N'Pre-op', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (9, N'Operational', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Base Room', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Can Opening', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Cooler Prep', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Dries', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Fish Prep', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Liquid Prep', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'NAP Room', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'New Rice Room', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Old Rice Room', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Old Wine Cooler Room', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Oven Room', 11, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Pan Room', 12, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Raw Meat Room', 13, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Steam Room', 14, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Stovex Room', 15, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (11, N'Other…', 16, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (12, N'Pre-Op SSOP', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (12, N'SPS', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (12, N'HACCP', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (12, N'Currently Not Available', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (13, N'Yes', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (13, N'No', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Ceiling tiles', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'EPSU', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Facilities', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'GMP Product Handling', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'GMP Storage', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'GMP Tool Handling', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Housekeeping', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Labeling', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Multiple', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (2, N'Condensation', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Gasket', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Contherm Blade', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Glass', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Metal', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'O-ring', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Paper/Cardboard', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Plastic - Hard', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Plastic - Soft', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Rubber', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Scraper Blade', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Wood', 11, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (14, N'Other…', 12, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (15, N'In-House', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (15, N'Vendor', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Stovex System', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Milk System', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rietz Extructor ', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder w/ Wheat Gluten ', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/Wheat Gluten', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Fat Melter', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler(s) C ', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Graco Pump', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rice Cooker ', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler(s) B ', 11, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Kramer & Grebe ', 12, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Model D Dicer ', 13, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Kliklok', 14, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Tote Dumper', 15, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Colborne Blender ', 16, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Mondini ', 17, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Laser Coder(s)', 18, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Scholle System', 19, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Mepaco Blender', 20, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Bridge Machine', 21, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'MPO - 52''', 22, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Lightnin'' Mixer', 23, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rietz Mixer w/o Steam ', 24, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Raque Topping Dispenser', 25, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Bulk Tomatoes System', 26, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Glaze Kettle', 27, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Final Grinder ', 28, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Hi-Mech Meat Dicer', 29, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Model M Dicer ', 30, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder  ', 31, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher  ', 32, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Pre-Breaker  ', 33, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler(s) A ', 34, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Shear Pump', 35, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Model L Dicer  ', 36, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Model CC Shredder  ', 37, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder ', 38, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Watson-Marlow Pump', 39, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Risco Dispenser', 40, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Tu-Way Cheese Cutter ', 41, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Pasta / Rice Mixer', 42, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Tray Dispenser', 43, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Prepweigh Tables', 44, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Deville Cheese Shredder', 45, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand 10', 46, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand 2', 47, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/Wheat Gluten  ', 48, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Pasta Oil Spray System', 49, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rotary Dispensers', 50, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Net Weigh Fillers ', 51, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Potato System', 52, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Potato Pump', 53, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Potato Fillers ', 54, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rotary Dispensers - B', 55, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'AVF''s', 56, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'10A Cookstand', 57, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand 6', 58, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'MPO - 35'' or 52''', 59, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rotary Dispensers - A', 60, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Auto Canopener', 61, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Automated Ketchup Dispenser', 62, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Tilt Kettle', 63, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Semi-Auto Canopener', 64, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler B Settings', 65, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder w/Wht Gltn & Garlic', 66, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/Wht Gltn & Garlic ', 67, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Drain Conveyor or Table', 68, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler A Settings', 69, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'ABCO Blancher', 70, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'10A Fat Melter', 71, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Potato Pump Settings', 72, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler C Settings', 73, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand 5', 74, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder w/Cyclone WW System ', 75, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/Whole Wheat Blend', 76, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler Settings - Production', 77, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rotary Dispensers - C', 78, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Rietz Mixer w/ Steam ', 79, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Urschel Diversacut 2110', 80, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Twister AT Dicer', 81, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Paprika Dispensers', 82, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Fryer', 83, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Breddo Mixer', 84, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Roller before Heatseal - L8', 85, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Cookstand 8 (Steam Jacketed Kettle)', 86, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/ Egg   ', 87, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Sauce Filler - Foodservice  ', 88, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Crimper', 89, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Quadrel Label Machine', 90, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Videojet Printer', 91, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Extruder w/ Egg ', 92, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Blancher w/ Egg  ', 93, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Steamer', 94, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'MPO - 35''', 95, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Syntron', 96, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Murzan Pump - Large', 97, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Small Seydelmann  ', 98, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Base Pump System', 99, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'ADCO FoodService Cartoner', 100, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Delkor Box Former', 101, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Hayssen Pouch Machine', 102, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'X Ray Machine', 103, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Small Portion Dispenser', 104, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'Portable Traveling Head', 105, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (16, N'NA', 106, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Ant', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Bee', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Beetle', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Fly', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Generic', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Roach', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Insect - Stink Bug/Kudzu Bug', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Rodent', 8, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Bird', 9, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Mammal', 10, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (17, N'Other', 11, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (18, N'Concern', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (18, N'Hold', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Monday', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Tuesday', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Wednesday', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Thursday', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Friday', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Saturday', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (19, N'Sunday', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (20, N'Start-Up', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (20, N'Changover', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (20, N'Other', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (21, N'Water', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (21, N'Tomato', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (21, N'Milk', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (22, N'Pasta', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (22, N'Rice', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (22, N'Potato', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (22, N'Quinoa', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'APC', 1, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'EB', 2, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'E. coli', 3, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'Staph', 4, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'Salmonella', 5, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'Listeria', 6, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'Listeria Mono', 7, 1)
INSERT [dbo].[DropDownItem] ([DropdownTypeId], [Value], [SortOrder], [IsActive]) VALUES (23, N'Spore-Formers', 8, 1)
GO
