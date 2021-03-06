USE [notify]
GO
/****** Object:  Table [dbo].[ErrorMessage]    Script Date: 22/02/2019 16:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ErrorMessage](
	[ErrorMessageID] [int] NOT NULL,
	[ErrorMessage] [varchar](500) NULL,
	[Severity] [int] NULL,
 CONSTRAINT [PK_ErrorMessage] PRIMARY KEY CLUSTERED 
(
	[ErrorMessageID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
