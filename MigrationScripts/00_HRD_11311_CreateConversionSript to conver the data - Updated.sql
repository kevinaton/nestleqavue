USE [HRD]
GO


IF OBJECT_ID('HRD', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[HRD](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Plant] [nchar](4) NULL,
	[Originator] [nvarchar](50) NULL,
	[IsHRD] [bit] NULL,
	[DayCode] [nvarchar](7) NULL,
	[Line] [nvarchar](50) NULL,
	[Shift] [nvarchar](50) NULL,
	[LCL] [nvarchar](50) NULL,
	[Schedule] [nvarchar](50) NULL,
	[HourCode] [nvarchar](100) NULL,
	[Cases] [int] NULL,
	[UnpalletizedProductHeld] [bit] NULL,
	[RejectOrNinePack] [bit] NULL,
	[ReworkOutsideNormalProduction] [bit] NULL,
	[ContactOtherPlant] [bit] NULL,
	[GLOBENum] [nvarchar](10) NULL,
	[ShortDescription] [nvarchar](75) NULL,
	[Problem] [nvarchar](max) NULL,
	[ReworkInstructions] [nvarchar](max) NULL,
	[ReworkApproved] [bit] NULL,
	[ReworkApprovedBy] [nvarchar](50) NULL,
	[ReworkStarted] [datetime] NULL,
	[ReworkComplete] [datetime] NULL,
	[ReworkCompletedBy] [nvarchar](50) NULL,
	[CauseMan] [bit] NULL,
	[CauseMethod] [bit] NULL,
	[CauseMaterial] [bit] NULL,
	[CauseMachine] [bit] NULL,
	[TLforFU] [nvarchar](50) NULL,
	[Classification] [nvarchar](50) NULL,
	[FTQCases] [int] NULL,
	[NonFTQCases] [int] NULL,
	[DCDate] [datetime] NULL,
	[DCUser] [nvarchar](50) NULL,
	[FCDate] [datetime] NULL,
	[FCUser] [nvarchar](50) NULL,
	[OtherHRDAffected] [bit] NULL,
	[OtherHRDNum] [nvarchar](50) NULL,
	[DateHeld] [datetime] NULL,
	[HoldCategory] [nvarchar](50) NULL,
	[HoldSubCategory] [nvarchar](50) NULL,
	[CostofProductonHold] [money] NULL,
	[DateofDisposition] [datetime] NULL,
	[Clear] [int] NULL,
	[Scrap] [int] NULL,
	[ThriftStore] [int] NULL,
	[Samples] [int] NULL,
	[Cancelled] [bit] NULL,
	[Complete] [bit] NULL,
	[HRDCompletedBy] [nvarchar](50) NULL,
	[AllCasesAccountedFor] [bit] NULL,
	[DateReleased] [datetime] NULL,
	[TestingCost] [money] NULL,
	[LaborHours] [numeric](10, 4) NULL,
	[GSTDRequired] [bit] NULL,
	[HighRisk] [bit] NULL,
	[PhysicalIsolationNeeded] [bit] NULL,
	[Physical5DaysGSTD] [bit] NULL,
	[LastChange] [datetime] NULL,
	[LastChangeUser] [nvarchar](100) NULL,
	[ReworkApprovedDate] [datetime] NULL,
	[CodingType] [nvarchar](20) NULL,
	[CodingDetails] [nvarchar](10) NULL,
	[YearHeld] [nvarchar](4) NULL,
	[QAComments] [nvarchar](max) NULL,
	[SecondaryNotification] [bit] NULL,
	[IsPest] [bit] NULL,
	[IsSMI] [bit] NULL,
	[IsNR] [bit] NULL,
	[IsFM] [bit] NULL,
	[IsMicro] [bit] NULL,
 CONSTRAINT [PK_HRD] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF OBJECT_ID('DropDownType', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[DropDownType](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Name] [varchar](100) NOT NULL,
 CONSTRAINT [PK_DropDownType] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


IF OBJECT_ID('DropDownItem', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[DropDownItem](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[DropDownTypeId] [int] NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
	[SortOrder] [smallint] NOT NULL,
	[IsActive] [bit] NOT NULL,
    FOREIGN KEY (DropDownTypeId) REFERENCES DropDownType(Id),
 CONSTRAINT [PK_DropDownItem] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO


IF OBJECT_ID('HRDPO', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[HRDPO](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HRDId] [int] NOT NULL,
	[PONumber] [nvarchar](10) NOT NULL,
    FOREIGN KEY (HRDId) REFERENCES HRD(Id),
 CONSTRAINT [PK_HRDPO] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('HRDFC', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[HRDFC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HRDId] [int] NOT NULL,
	[Location] [nvarchar](10) NOT NULL,
	[NumberOfCases] [int] NOT NULL,
    FOREIGN KEY (HRDId) REFERENCES HRD(Id),
 CONSTRAINT [PK_HRDFC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('HRDDC', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[HRDDC](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HRDId] [int] NOT NULL,
	[Location] [nvarchar](10) NOT NULL,
	[NumberOfCases] [int] NOT NULL,
    FOREIGN KEY (HRDId) REFERENCES HRD(Id),
 CONSTRAINT [PK_HRDDC] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('LaborCost', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[LaborCost](
	[Year] [char](4) NOT NULL,
	[LaborCost] [smallmoney] NULL,
 CONSTRAINT [PK_LaborCost] PRIMARY KEY CLUSTERED 
(
	[Year] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('Product', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[Product](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [nchar](4) NOT NULL,
	[GPN] [nvarchar](10) NOT NULL,
	[Description] [nvarchar](100) NULL,
	[CostPerCase] [money] NULL,
	[Country] [nchar](2) NULL,
	[NoBBDate] [bit] NULL,
	[Holiday] [bit] NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [DF_Product_Year]  DEFAULT ((0)) FOR [Year]
GO
ALTER TABLE [dbo].[Product] ADD CONSTRAINT [DF_Product_GPN]  DEFAULT ((0)) FOR [GPN]
GO


IF OBJECT_ID('HRDNotes', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[HRDNotes](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HRDId] [int] NOT NULL,
	[Category] [nvarchar](50) NULL,
	[Description] [nvarchar](max) NULL,
	[UserID] [nvarchar](50) NULL,
	[Date] [datetime] NULL,
	[FileName] [nvarchar](400) NULL,
	[Path] [nvarchar](400) NULL,
	[Size] [nvarchar](20) NULL,
    FOREIGN KEY (HRDId) REFERENCES HRD(Id),
 CONSTRAINT [PK_HRDNotes] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO

IF OBJECT_ID('Settings', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[Settings](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](25) NULL,
	[Description] [nvarchar](255) NULL,
	[Value] [nvarchar](255) NULL,
	[Type] [nvarchar](10) NULL,
 CONSTRAINT [PK_Settings] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('Security', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[Security](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](5) NULL,
	[Name] [nvarchar](50) NULL,
	[Administrator] [bit] NULL,
	[QAManager] [bit] NULL,
	[QATGrp] [bit] NULL,
	[QAProdRelGrp] [bit] NULL,
	[ProdTLGrp] [bit] NULL,
	[ReworkMembersGrp] [bit] NULL,
	[PhysicalGrp] [bit] NULL,
	[Physical5Grp] [bit] NULL,
	[GSTDNotificationGrp] [bit] NULL,
	[DeleteHRD] [bit] NULL,
	[EditHRDApproveRework] [bit] NULL,
	[ProdMaintenanceList] [bit] NULL,
	[GSTDGrp] [bit] NULL,
	[BUMGrp] [bit] NULL,
	[LaborCostList] [bit] NULL,
	[Notification] [bit] NULL,
 CONSTRAINT [PK_Security] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 95, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('TestCost', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[TestCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Year] [nvarchar](50) NULL,
	[TestName] [nvarchar](50) NULL,
	[TestCost] [money] NULL,
 CONSTRAINT [PK_TestCost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF OBJECT_ID('HRDTestCost', 'U') IS  NULL 
BEGIN	
CREATE TABLE [dbo].[HRDTestCost](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[HRDId] [int] NOT NULL,
	[TestName] [nvarchar](50) NULL,
	[Qty] [int] NULL,
	[Cost] [money] NULL,
    FOREIGN KEY (HRDId) REFERENCES HRD(Id),
 CONSTRAINT [PK_HRDTestCost] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO


IF OBJECT_ID('Users', 'U') IS  NULL 
BEGIN
CREATE TABLE [dbo].[Users](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserID] [nvarchar](50) NULL,
	[Name] [nvarchar](100) NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, FILLFACTOR = 90, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_Administrator]  DEFAULT ((0)) FOR [Administrator]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_QAManager]  DEFAULT ((0)) FOR [QAManager]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_QATGrp]  DEFAULT ((0)) FOR [QATGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_QAProdRelGrp]  DEFAULT ((0)) FOR [QAProdRelGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_ProdTLGrp]  DEFAULT ((0)) FOR [ProdTLGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_ReworkMembersGrp]  DEFAULT ((0)) FOR [ReworkMembersGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_PhysicalGrp]  DEFAULT ((0)) FOR [PhysicalGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_Physical5Grp]  DEFAULT ((0)) FOR [Physical5Grp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_GSTDNotificationGrp]  DEFAULT ((0)) FOR [GSTDNotificationGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_DeleteHRD]  DEFAULT ((0)) FOR [DeleteHRD]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_EditHRDApproveRework]  DEFAULT ((0)) FOR [EditHRDApproveRework]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_ProdMaintenanceList]  DEFAULT ((0)) FOR [ProdMaintenanceList]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_GSTDGrp]  DEFAULT ((0)) FOR [GSTDGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_BUMGrp]  DEFAULT ((0)) FOR [BUMGrp]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_http://localhost:61188/Playground/HRD/ProductMaintenance.aspx]  DEFAULT ((0)) FOR [LaborCostList]
GO
ALTER TABLE [dbo].[Security] ADD  CONSTRAINT [DF_Security_Notification]  DEFAULT ((0)) FOR [Notification]
GO
