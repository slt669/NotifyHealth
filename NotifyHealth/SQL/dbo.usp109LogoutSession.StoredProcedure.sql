USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp109LogoutSession]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp109LogoutSession]
	@SessionID int,
	@SessionGUID varchar(36),
	@ValidationMessage varchar(500) OUTPUT,
	@ValidationErrorNo varchar(10) OUTPUT
AS

/***************************************************************************************
**	Object:			dbo.usp109LogOutSession.
**	Date:			08-Aug-2017.
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
****************************************************************************************/

BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE	@ValidationError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SqlError = 0;
	SET @ValidationError = 0;
	
	-- Validate the parameter being passed in and only after it passes do we proceed
	SELECT	@ValidationError = 
			CASE	-- Is this a valid set of session credentials and is account in correct state
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession
							WHERE	UserSessionID = @SessionID
							AND		UserSessionGUID = @SessionGUID) < 1 THEN 10011
					-- Is this session already logged out?
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession
							WHERE	UserSessionID = @SessionID
							AND		UserSessionGUID = @SessionGUID
							AND		SessionStatus = 'LoggedOut') = 1 THEN 10014
					-- None of the above has validated true so the logon credentials passed are correct
					ELSE 0
		 	END;
		 	
	IF (@ValidationError <> 0)
	BEGIN
		SELECT @ValidationMessage=ErrorMessage FROM ErrorMessage WHERE ErrorMessageId=@ValidationError;	
		SELECT @ValidationErrorNo=CONVERT(varchar(10),@ValidationError);			
		GOTO ErrHandler;
	END

	-- We've passed validation so log the session out
	UPDATE	UserSession
	SET		SessionStatus = 'LoggedOut'
	WHERE	UserSessionID = @SessionID
	AND		UserSessionGUID = @SessionGUID;

	SELECT	@SqlError = @@error;				
	IF (@SqlError <> 0) GOTO ErrHandler;
	
	SELECT	*
	FROM	UserSession
	WHERE	UserSessionID = @SessionID
	AND		UserSessionGUID = @SessionGUID
	AND		SessionStatus = 'LoggedOut';

	SELECT	@SqlError = @@error, @SessionId = @@IdENTITY;
	IF (@SqlError <> 0) GOTO ErrHandler;

	SET NOCOUNT OFF;
	SET ARITHIGNORE OFF;

	SET @ValIdationErrorNo = 0;
	SET @ValIdationMessage =  'Logged On';
	
	RETURN 0

ErrHandler:
	IF (@@trancount > 0)
		ROLLBACK TRANSACTION;
		
	IF (@SqlError <> 0)
	BEGIN
		SELECT	@ReturnMessage = 'ErrorNo: ' + CONVERT(varchar(20), @SqlError) + '; Description: Error occurred during stored procedure execution. Please look up error number for information.'
		RAISERROR(@ReturnMessage, 16, 1);
	END
	ELSE
		RETURN @ValidationError;
END

GO
