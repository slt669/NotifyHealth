USE [notify]
GO
/****** Object:  Table [dbo].[campaigns]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[campaigns](
	[Campaign_ID] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](200) NULL,
	[Status_ID] [int] NOT NULL,
	[Program_ID] [int] NULL,
	[Organization_ID] [int] NOT NULL,
	[Created_By] [int] NULL,
	[Edited_By] [int] NULL,
	[Created_When] [datetime] NULL,
	[Edited_When] [datetime] NULL,
 CONSTRAINT [PK_notification_groups] PRIMARY KEY CLUSTERED 
(
	[Campaign_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
