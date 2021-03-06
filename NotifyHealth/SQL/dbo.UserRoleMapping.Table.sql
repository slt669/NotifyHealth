USE [notify]
GO
/****** Object:  Table [dbo].[UserRoleMapping]    Script Date: 22/02/2019 16:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleMapping](
	[UserRoleMappingID] [int] IDENTITY(1,1) NOT NULL,
	[UserLogonID] [int] NOT NULL,
	[UserRoleID] [int] NOT NULL,
	[Organization_ID] [int] NULL,
 CONSTRAINT [PK_UserRoleMapping] PRIMARY KEY CLUSTERED 
(
	[UserRoleMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
