USE [notify]
GO
/****** Object:  Table [dbo].[client_memberships]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[client_memberships](
	[Client_Membership_ID] [int] IDENTITY(1,1) NOT NULL,
	[Client_ID] [int] NOT NULL,
	[Campaign_ID] [int] NOT NULL,
	[Start] [datetime] NULL,
	[Appointment] [datetime] NULL,
	[Organization_ID] [int] NOT NULL,
 CONSTRAINT [PK_memberships] PRIMARY KEY CLUSTERED 
(
	[Client_Membership_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
