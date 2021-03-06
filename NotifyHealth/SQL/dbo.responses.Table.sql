USE [notify]
GO
/****** Object:  Table [dbo].[responses]    Script Date: 22/02/2019 16:07:10 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[responses](
	[Response_ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Value] [nvarchar](160) NOT NULL,
 CONSTRAINT [PK_client_responses] PRIMARY KEY CLUSTERED 
(
	[Response_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
