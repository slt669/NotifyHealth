USE [notify]
GO
/****** Object:  Table [dbo].[audit]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[audit](
	[Audit_ID] [int] IDENTITY(1,1) NOT NULL,
	[Type] [nvarchar](50) NOT NULL,
	[Type_ID] [int] NOT NULL,
	[Attribute] [nvarchar](200) NOT NULL,
	[Attribute_Value_Was] [nvarchar](200) NOT NULL,
	[Attribute_Value_Is] [nvarchar](200) NOT NULL,
	[User_ID] [int] NULL,
	[TimeStamp] [datetime] NOT NULL,
	[Security_Group_ID] [int] NULL,
 CONSTRAINT [PK_audit] PRIMARY KEY CLUSTERED 
(
	[Audit_ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)
)

GO
ALTER TABLE [dbo].[audit] ADD  CONSTRAINT [DF_audit_TimeStamp]  DEFAULT (getdate()) FOR [TimeStamp]
GO
