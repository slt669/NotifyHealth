USE [notify]
GO
/****** Object:  Table [dbo].[users]    Script Date: 22/02/2019 16:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[users](
	[User_ID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [nvarchar](50) NOT NULL,
	[Password] [nvarchar](50) NULL,
	[Photo] [varbinary](max) NULL,
	[First_Name] [nvarchar](50) NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Organization_ID] [int] NOT NULL,
	[Status_ID] [int] NULL
)

GO
SET ANSI_PADDING OFF
GO
