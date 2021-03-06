USE [notify]
GO
/****** Object:  Table [dbo].[UserRoleDataAccessMapping]    Script Date: 22/02/2019 16:07:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserRoleDataAccessMapping](
	[UserRoleDataAccessMappingID] [int] IDENTITY(1,1) NOT NULL,
	[UserRoleDataAccessID] [int] NOT NULL,
	[UserRoleID] [int] NOT NULL,
 CONSTRAINT [PK_UserRoleDataAccessMapping] PRIMARY KEY CLUSTERED 
(
	[UserRoleDataAccessMappingID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
