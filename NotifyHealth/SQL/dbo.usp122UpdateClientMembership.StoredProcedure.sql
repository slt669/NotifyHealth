USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp122UpdateClientMembership]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp122UpdateClientMembership]
(
@OrganizationId integer,
@CampaignId integer,
@ClientID integer,
@Start datetime,
@Appointment datetime,
@Delete char(1)='N'
)
as

/*************************************************************************************************************
**	Object:			dbo.usp122UpdateClientMembership.
**	Date:			12/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure allows updates to the Client Memberships table.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	IF (SELECT count(*) FROM client_memberships WHERE Campaign_ID=@CampaignId AND Organization_ID=@OrganizationId AND Client_ID =@ClientID)=1
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			UPDATE client_memberships
			SET
				Client_ID = @ClientID
				,Campaign_ID = @CampaignId
				,Start = @Start
				,Appointment = @Appointment
				,Organization_ID = @OrganizationId
			WHERE Campaign_ID=@CampaignId AND Client_ID =@ClientID

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		ELSE
			BEGIN
			DELETE client_memberships
			WHERE Campaign_ID=@CampaignId AND Client_ID=@ClientID;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END
	ELSE
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			INSERT INTO client_memberships (Client_ID,Campaign_ID,Start,Appointment,Organization_ID)
			SELECT				  @ClientID,@CampaignId,@Start,@Appointment, @OrganizationId;

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
