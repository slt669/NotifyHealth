USE [notify]
GO
/****** Object:  Table [dbo].[clients]    Script Date: 22/02/2019 16:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[clients](
	[Client_ID] [int] IDENTITY(10000,1) NOT NULL,
	[C_Status_ID] [int] NOT NULL,
	[First_Name] [nvarchar](50) NOT NULL,
	[Last_Name] [nvarchar](50) NULL,
	[Photo] [varbinary](max) NULL,
	[Phone_Number] [nvarchar](12) NULL,
	[Phone_Carrier] [nvarchar](50) NULL,
	[Message_Address] [nvarchar](50) NULL,
	[P_Status_ID] [int] NULL,
	[Participation_ID] [int] NOT NULL,
	[Added] [date] NULL,
	[A_Type_ID] [int] NOT NULL,
	[Organization_ID] [int] NOT NULL,
	[Created_By] [int] NULL,
	[Edited_By] [int] NULL,
	[Created_When] [datetime] NULL,
	[Edited_When] [datetime] NULL,
 CONSTRAINT [PK_clients] PRIMARY KEY CLUSTERED 
(
	[Client_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
