USE [notify]
GO
/****** Object:  Table [dbo].[UserRoleAccess]    Script Date: 22/02/2019 16:07:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserRoleAccess](
	[UserRoleAccessID] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleID] [int] NOT NULL,
	[PageName] [varchar](100) NULL,
 CONSTRAINT [PK_UserRoleAccess] PRIMARY KEY CLUSTERED 
(
	[UserRoleAccessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
