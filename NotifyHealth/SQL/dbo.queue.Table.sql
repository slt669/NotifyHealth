USE [notify]
GO
/****** Object:  Table [dbo].[queue]    Script Date: 22/02/2019 16:07:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[queue](
	[Queue_ID] [int] IDENTITY(100,1) NOT NULL,
	[Client_ID] [int] NOT NULL,
	[Notification_ID] [int] NOT NULL,
	[Timestamp] [datetime] NOT NULL,
	[Organization_ID] [int] NOT NULL,
 CONSTRAINT [PK_queue] PRIMARY KEY CLUSTERED 
(
	[Queue_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
