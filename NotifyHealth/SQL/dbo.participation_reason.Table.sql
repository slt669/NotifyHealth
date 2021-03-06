USE [notify]
GO
/****** Object:  Table [dbo].[participation_reason]    Script Date: 22/02/2019 16:07:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[participation_reason](
	[Participation_ID] [int] IDENTITY(1,1) NOT NULL,
	[Value] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_participation_reason] PRIMARY KEY CLUSTERED 
(
	[Participation_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
