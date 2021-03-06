USE [notify]
GO
/****** Object:  Table [dbo].[UserRole]    Script Date: 22/02/2019 16:07:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserRole](
	[UserRoleID] [int] IDENTITY(1,1) NOT NULL,
	[RoleDescription] [varchar](50) NULL,
	[SessionTimeout] [int] NULL,
	[RoleType] [varchar](20) NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[UserRoleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
