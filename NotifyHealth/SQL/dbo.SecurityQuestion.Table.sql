USE [notify]
GO
/****** Object:  Table [dbo].[SecurityQuestion]    Script Date: 22/02/2019 16:07:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[SecurityQuestion](
	[SecurityQuestionID] [int] IDENTITY(1,1) NOT NULL,
	[SecurityQuestion] [varchar](200) NOT NULL,
	[ValidFrom] [datetime] NOT NULL,
	[ValidUntil] [datetime] NULL,
	[CurrentFlag] [int] NOT NULL,
	[ClientID] [int] NULL,
 CONSTRAINT [PK_SecurityQuestion] PRIMARY KEY CLUSTERED 
(
	[SecurityQuestionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
