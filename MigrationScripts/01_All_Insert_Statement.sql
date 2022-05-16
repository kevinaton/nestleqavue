USE HRD

SET IDENTITY_INSERT dbo.HRD ON 
INSERT INTO dbo.HRD 
	(
		[Id],
		[Plant],
		[Originator],
		[HRD],
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
		[HRD],
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

SET IDENTITY_INSERT [dbo].DropDownType OFF 
GO


/*********************************************/
/***** DROPDOWNTYPE  ********/
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