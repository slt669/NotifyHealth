USE [notify]
GO
/****** Object:  Table [dbo].[organization]    Script Date: 22/02/2019 16:07:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[organization](
	[Organization_ID] [int] IDENTITY(1,1) NOT NULL,
	[Photo] [varbinary](max) NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Phone_Number] [nchar](10) NULL,
	[SMS_Account] [nvarchar](50) NULL,
	[SMS_Password] [nvarchar](50) NULL,
	[API_Username] [nvarchar](50) NULL,
	[API_Password] [nvarchar](50) NULL,
	[Status_ID] [int] NOT NULL,
	[Portal_ID] [int] NOT NULL
)

GO
SET ANSI_PADDING OFF
GO
