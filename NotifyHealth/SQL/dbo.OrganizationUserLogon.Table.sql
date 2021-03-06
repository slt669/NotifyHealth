USE [notify]
GO
/****** Object:  Table [dbo].[OrganizationUserLogon]    Script Date: 22/02/2019 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrganizationUserLogon](
	[OrganizationUserLogonID] [int] IDENTITY(1,1) NOT NULL,
	[Organization_ID] [int] NOT NULL,
	[UserLogonID] [int] NOT NULL,
 CONSTRAINT [PK_OrganizationUserLogon] PRIMARY KEY CLUSTERED 
(
	[OrganizationUserLogonID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
