USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp121GetNotificationsbyCampaigns]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp121GetNotificationsbyCampaigns]
(
@Organization_ID integer,
@Campaign_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp121GetNotificationsbyCampaigns.
**	Date:			16/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Notifications by Campaigns
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

  SELECT 
       n.Notification_ID
      ,n.N_Type_ID
      ,n.Period
      ,n.Text
      ,n.Status_ID
      ,n.Campaign_ID
      ,n.Organization_ID
      ,n.Created_By
      ,n.Edited_By
      ,n.Created_When
      ,n.Edited_When
	  ,s.Value as 'Value'
	  ,s.Status_ID as 'Status'
	  ,nt.Value  as 'NotificationType'
  FROM notify.dbo.notifications n
  LEFT JOIN status s  ON s.Status_ID=n.Status_ID
  LEFT JOIN notification_type nt  ON nt.N_Type_ID=n.N_Type_ID
  where n.Campaign_ID = @Campaign_ID AND Organization_ID = @Organization_ID

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
