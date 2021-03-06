USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp009UpdateClients]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp009UpdateClients]
(
@OrganizationId integer,
@ClientId integer,
@FirstName varchar(200)='',
@LastName varchar(200)='',
@CStatusId integer,
@PStatusID integer,
@ATypeID integer,
@PhoneNumber varchar(200)='',
@PhoneCarrier varchar(200)='',
@MessageAddress varchar(200)='',
@ParticipationID integer,
@Created_By integer,
@Edited_By integer,
@Created_When datetime,
@Edited_When datetime,
@Delete char(1)='N',
@IDENTITY int output
)

as

/*************************************************************************************************************
**	Object:			dbo.usp009UpdateClients.
**	Date:			12/001/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure allows updates to the Clients table.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	IF (SELECT count(*) FROM Clients WHERE Organization_ID=@OrganizationId AND Client_ID=@ClientId)=1
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			UPDATE Clients
			SET First_Name=@FirstName, 
				Last_Name=@LastName,
				 C_Status_ID=@CStatusId,
				  P_Status_ID=@PStatusId,
				  A_Type_ID=@ATypeID,
				  Phone_Number=@PhoneNumber,
				  Phone_Carrier=@PhoneCarrier,
				  Message_Address=@MessageAddress,
				  Edited_By=@Edited_By,
				 Edited_When=@Edited_When
			WHERE Organization_ID=@OrganizationId AND Client_ID=@ClientId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		ELSE
			BEGIN
			DELETE Clients
			WHERE Organization_ID=@OrganizationId AND Client_ID=@ClientId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END
	ELSE
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			INSERT INTO Clients (Organization_ID,First_Name,Last_Name,C_Status_ID,P_Status_ID,A_Type_ID,Phone_Number,Phone_Carrier,Message_Address,Participation_ID,Created_By,Created_When,Edited_By,Edited_When)
			SELECT @OrganizationId,@FirstName,@LastName,@CStatusId,@PStatusId, @ATypeID,@PhoneNumber,@PhoneCarrier,@MessageAddress,@ParticipationID,@Created_By,@Created_When,null,null;
			select @IDENTITY = @@IDENTITY
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
