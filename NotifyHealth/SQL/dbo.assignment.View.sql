USE [notify]
GO
/****** Object:  View [dbo].[assignment]    Script Date: 22/02/2019 16:07:14 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- ==========================================
-- Create View template for Windows Azure SQL Database
-- ==========================================
--IF object_id(N'<schema_name, sysname, dbo>.<view_name, sysname, Top10Sales>', 'V') IS NOT NULL
--	DROP VIEW <schema_name, sysname, dbo>.<view_name, sysname, Top10Sales>
--GO

CREATE VIEW [dbo].[assignment] AS
SELECT programs.Name as "Program"
		,campaigns.Name AS "Campaign"
		,notifications.Type
		,notifications.Period
		,notifications.Text
FROM notifications
LEFT JOIN campaigns ON campaigns.Campaign_ID = notifications.Campaign_ID
LEFT JOIN programs ON programs.Program_ID = campaigns.Program_ID

GO
