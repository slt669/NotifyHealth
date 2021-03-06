USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp115QueueOnBoardingNotification]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp115QueueOnBoardingNotification]
(
@OrganizationId integer,
@ClientId integer,
@NotificationType integer,
@ValidationMessage varchar(500) OUTPUT,
@ValidationErrorNo varchar(10) OUTPUT
)

as

/*************************************************************************************************************
**	Object:			dbo.usp115QueueOnBoardingNotification.
**	Date:			12/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure QueueOnBoardingNotification.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;	  	
	DECLARE @Notification_ID integer;		
	DECLARE	@SqlError int;
	DECLARE	@ValidationError int;
	DECLARE @ReturnMessage varchar(300);

	SET	@SqlError = 0;
	SET @ValidationError = 0;
	SET @ValidationErrorNo = '0';

	SET	@SQLError = 0;

  BEGIN

if exists ( SELECT 1 Notification_ID  FROM notifications WHERE notifications.N_Type_ID = @NotificationType AND notifications.Organization_ID = @OrganizationId)
BEGIN
   SET @ValidationError = 0;
 END
ELSE
 BEGIN
   SET @ValidationError = 1;
 END
 IF (@ValidationError = 0) 
		BEGIN
			SET @ValidationErrorNo = '0';
		END
	ELSE
		BEGIN
		SET @ValidationErrorNo = '1';
		SET @ValidationMessage =  'There are no related Notifications set.';
		GOTO ErrHandler;
		END
   IF NOT EXISTS (SELECT * FROM queue 
                   WHERE Client_ID = @ClientId
                   AND Notification_ID =(SELECT  Notification_ID  FROM notifications WHERE notifications.N_Type_ID = @NotificationType AND notifications.Organization_ID = @OrganizationId))	


   SET @Notification_ID =(SELECT  Notification_ID  FROM notifications WHERE notifications.N_Type_ID = @NotificationType AND notifications.Organization_ID = @OrganizationId);		
		  			    
   BEGIN
       INSERT INTO queue (Client_ID,Notification_ID,Timestamp,Organization_ID)
       VALUES (@ClientId,@Notification_ID,GETDATE( ),@OrganizationId)
   END
END

	SET NOCOUNT OFF;

	RETURN 0

ErrHandler:

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
