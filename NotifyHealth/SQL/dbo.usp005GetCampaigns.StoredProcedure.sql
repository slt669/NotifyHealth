USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp005GetCampaigns]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp005GetCampaigns]
(
@Organization_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp005GetCampaigns.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Campaigns for an Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	  	SELECT campaigns.Campaign_ID
      ,campaigns.Name as 'Campaign'
      ,campaigns.Description
      ,campaigns.Status_ID
      ,campaigns.Program_ID
      ,campaigns.Organization_ID
      ,campaigns.Created_By
      ,campaigns.Edited_By
      ,campaigns.Created_When
      ,campaigns.Edited_When
	  ,status.Value
	  ,status.Status_ID
	  ,programs.Name as 'Program'
	  , count(notifications.Campaign_ID) as 'RelatedNotifications'   
	FROM Campaigns 
	LEFT JOIN status  ON campaigns.Status_ID=status.Status_ID
	LEFT JOIN programs  ON campaigns.Program_ID=programs.Program_ID
	left join notifications on (campaigns.Campaign_ID = notifications.Campaign_ID)
	WHERE campaigns.Organization_ID=@Organization_ID
group by
	  campaigns.[Campaign_ID]
      ,campaigns.[Name]
      ,campaigns.[Description]
      ,campaigns.[Status_ID]
      ,campaigns.[Program_ID]
      ,campaigns.[Organization_ID]
      ,campaigns.[Created_By]
      ,campaigns.[Edited_By]
      ,campaigns.[Created_When]
      ,campaigns.[Edited_When]
	    ,status.Status_ID,
		status.Value,
		programs.name



	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

	SET NOCOUNT OFF;

	RETURN 0

ErrorHandler:

	IF (@@trancount > 0)
		ROLLBACK TRANSACTION;
			
	IF (@SQLError <> 0)
		BEGIN
			SELECT	@ReturnMessage = 'ErrorNo: ' + CONVERT(varchar(20), @SQLError) + '; Description: Error occurred during stored procedure execution. Please look up error number for information.'
			RAISERROR(@ReturnMessage, 16, 1);
		END
	ELSE
		BEGIN
			RETURN @SQLError;
		END
	END



GO
