USE [notify]
GO
/****** Object:  Table [dbo].[notifications]    Script Date: 22/02/2019 16:07:08 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[notifications](
	[Notification_ID] [int] IDENTITY(1,1) NOT NULL,
	[N_Type_ID] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Text] [nvarchar](160) NOT NULL,
	[Status_ID] [int] NOT NULL,
	[Campaign_ID] [int] NULL,
	[Organization_ID] [int] NOT NULL,
	[Created_By] [int] NULL,
	[Edited_By] [int] NULL,
	[Created_When] [datetime] NULL,
	[Edited_When] [datetime] NULL,
 CONSTRAINT [PK_notifications] PRIMARY KEY CLUSTERED 
(
	[Notification_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
