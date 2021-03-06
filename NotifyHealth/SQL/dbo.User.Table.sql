USE [notify]
GO
/****** Object:  Table [dbo].[User]    Script Date: 22/02/2019 16:07:11 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[User](
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[Title] [varchar](10) NULL,
	[Forename] [varchar](50) NULL,
	[Surname] [varchar](50) NULL,
	[UserStatus] [varchar](50) NULL,
	[Photo] [nvarchar](200) NULL,
	[EmailAddress] [varchar](50) NULL,
	[MobileTelephoneNo] [varchar](50) NULL,
	[HomeTelephoneNo] [varchar](50) NULL,
	[WorkTelephoneNo] [varchar](50) NULL,
	[FaxTelephoneNo] [varchar](50) NULL,
	[UserNotes] [varchar](100) NULL,
	[JobTitle] [varchar](50) NULL,
	[LastUpdatedByUserSessionID] [int] NULL,
	[LastUpdatedByIISAuthUser] [varchar](50) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[UserID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
