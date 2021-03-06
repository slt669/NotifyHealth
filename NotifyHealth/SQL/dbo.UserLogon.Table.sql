USE [notify]
GO
/****** Object:  Table [dbo].[UserLogon]    Script Date: 22/02/2019 16:07:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserLogon](
	[UserLogonID] [int] IDENTITY(1,1) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Password] [varchar](50) NOT NULL,
	[AccountStatus] [varchar](50) NOT NULL,
	[SecurityQuestionID] [int] NULL,
	[SecurityAnswer] [varchar](50) NULL,
	[StartDate] [datetime] NULL,
	[EndDate] [datetime] NULL,
	[LogonAttempts] [int] NULL,
	[SecurityQuestionAttempts] [int] NULL,
	[AutomaticUnlock] [char](1) NULL,
	[MustChangePwd] [tinyint] NULL,
	[PwdExpiry] [datetime] NULL,
	[UserID] [int] NULL,
	[Organization_ID] [int] NULL,
 CONSTRAINT [PK_UserLogon] PRIMARY KEY CLUSTERED 
(
	[UserLogonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON),
 CONSTRAINT [IX_UserLogon] UNIQUE NONCLUSTERED 
(
	[Username] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
