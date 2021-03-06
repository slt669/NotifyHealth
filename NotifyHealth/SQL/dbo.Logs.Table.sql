USE [notify]
GO
/****** Object:  Table [dbo].[Logs]    Script Date: 22/02/2019 16:07:07 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Logs](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[LogDate] [datetime2](7) NOT NULL,
	[LogThread] [varchar](50) NOT NULL,
	[LogLevel] [varchar](50) NOT NULL,
	[LogSource] [varchar](300) NOT NULL,
	[LogMessage] [varchar](4000) NOT NULL,
	[Exception] [varchar](4000) NULL,
 CONSTRAINT [PK_Logs] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
SET ANSI_PADDING OFF
GO
