USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp008UpdateNotifications]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp008UpdateNotifications]
(
@OrganizationId integer,
@NotificationId integer,
@Text varchar(200)='',
@Period integer,
@StatusId integer,
@NTypeId integer,
@Delete char(1)='N',
@CampaignId integer,
@Created_By integer,
@Edited_By integer,
@Created_When datetime,
@Edited_When datetime
)

as

/*************************************************************************************************************
**	Object:			dbo.usp008UpdateNotifications.
**	Date:			12/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure allows updates to the Notifications table.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	IF (SELECT count(*) FROM Notifications WHERE Organization_ID=@OrganizationId AND Notification_ID=@NotificationId)=1
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			UPDATE Notifications
			SET Text =@Text, 
				Period=@Period,
				 Status_ID=@StatusId,
				 N_Type_ID =@NTypeId,
				 	 Edited_By=@Edited_By,
				 Edited_When=@Edited_When,
				 Campaign_ID=@CampaignId
			WHERE Organization_ID=@OrganizationId AND Notification_ID=@NotificationId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		ELSE
			BEGIN
			DELETE Notifications
			WHERE Organization_ID=@OrganizationId AND Notification_ID=@NotificationId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END
	ELSE
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			INSERT INTO Notifications (Organization_ID,N_Type_ID,Text ,Campaign_ID,Period,Status_ID,Created_By,Created_When,Edited_By,Edited_When)
			SELECT @OrganizationId,@NTypeId,@Text,@CampaignId,@Period,@StatusId,@Created_By,@Created_When,null,null;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END

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
