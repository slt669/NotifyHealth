USE [notify]
GO
/****** Object:  Table [dbo].[UserSession]    Script Date: 22/02/2019 16:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserSession](
	[UserSessionId] [int] IDENTITY(1,1) NOT NULL,
	[UserSessionGUID] [uniqueidentifier] NOT NULL,
	[SessionStatus] [varchar](20) NOT NULL,
	[UserLogonId] [int] NOT NULL,
	[LastAccessed] [datetime] NOT NULL,
	[PageData] [varchar](500) NULL,
	[Organization_ID] [int] NULL
)

GO
SET ANSI_PADDING OFF
GO
