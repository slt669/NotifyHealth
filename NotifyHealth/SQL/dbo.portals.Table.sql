USE [notify]
GO
/****** Object:  Table [dbo].[portals]    Script Date: 22/02/2019 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[portals](
	[Portal_ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Logo] [nvarchar](200) NULL
)

GO
