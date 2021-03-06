USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp006GetNotifications]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp006GetNotifications]
(
@Organization_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp006GetNotifications.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Notifications for an Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	SELECT N.Notification_ID
      ,N.N_Type_ID
      ,N.Period
      ,N.Text
      ,N.Status_ID
      ,N.Campaign_ID
      ,N.Organization_ID
      ,N.Created_By
      ,N.Edited_By
      ,N.Created_When
      ,N.Edited_When
	  ,NT.Value as 'NotificationType'
	  ,C.Name as 'Campaign'
	  ,S.Value 
	  ,S.Status_ID as 'Status'

	FROM Notifications N
	LEFT JOIN status S ON N.Status_ID=S.Status_ID
	LEFT JOIN Notification_type NT ON N.N_Type_ID=NT.N_Type_ID
	LEFT JOIN Campaigns C ON N.Campaign_ID=C.Campaign_ID
	WHERE N.Organization_ID=@Organization_ID;

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
