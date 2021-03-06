USE [notify]
GO
/****** Object:  Table [dbo].[UserRoleDataAccess]    Script Date: 22/02/2019 16:07:12 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[UserRoleDataAccess](
	[UserRoleDataAccessID] [int] IDENTITY(1,1) NOT NULL,
	[DataType] [varchar](100) NULL,
	[DataDescription] [varchar](1000) NULL,
 CONSTRAINT [PK_UserRoleDataAccess] PRIMARY KEY CLUSTERED 
(
	[UserRoleDataAccessID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
