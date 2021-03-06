USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp103GetAccountDetails]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*********************************************************************************
**	Object:			dbo.usp103GetAccountDetails
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Lists the account details for the session information passed in
**					or for the UserLogonId passed in.
**********************************************************************************/
CREATE PROCEDURE [dbo].[usp103GetAccountDetails]
	@SessionId int,
	@SessionGUId varchar(36),
	@UserLogonId int = NULL,
	@ValIdationMessage varchar(500) OUTPUT,
	@ValIdationErrorNo varchar(10) OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE	@ValIdationError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SqlError = 0;
	SET @ValIdationError = 0;
	SET @ValIdationErrorNo = 0;
	
	-- First valIdate the parameter being passed in and only after it passes do we proceed
	SELECT	@ValIdationError = 
			CASE	-- Is this a valId set of session credentials and is account in correct state
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession US
									INNER JOIN UserLogon UL
										ON US.UserLogonId = UL.UserLogonId
										AND US.UserSessionId = @SessionId
										AND US.UserSessionGUId = @SessionGUId
										AND LOWER(UL.AccountStatus) IN ('active', 'unactivated')) < 1 THEN 10010
					ELSE 0
		 	END;
		 	
	IF (@ValIdationError <> 0) 
		BEGIN
		SELECT @ValIdationMessage=ErrorMessage FROM ErrorMessage WHERE ErrorMessageId=@ValIdationError;	
		SELECT @ValIdationErrorNo=CONVERT(varchar(10),@ValIdationError);							
		GOTO ErrHandler;
		END
	
	IF (@UserLogonId IS NULL)
	BEGIN
		-- List all the account details for this session
		SELECT	UL.Username,
				UL.Password,
			 UL.UserLogonId,
				UL.SecurityQuestionId,
				UL.SecurityAnswer,
				UL.AccountStatus,
				UL.MustChangePwd,
				UL.Organization_ID,
			    UR.[UserRoleID],
				U.*

		FROM	UserSession US
				INNER JOIN UserLogon UL
					ON US.UserLogonId = UL.UserLogonId
					
				INNER JOIN [User] U
					ON UL.UserId = U.UserId
						left join [notify].[dbo].[UserRoleMapping] UR on  UR.UserLogonID=UL.UserLogonId
		WHERE	US.UserSessionId = @SessionId
		AND		US.UserSessionGUId = @SessionGUId

		SELECT	@SqlError = @@error;
		IF (@SqlError <> 0) GOTO ErrHandler;
	END
	ELSE
	BEGIN
		SELECT	UL.Username,
				UL.Password,
				UL.SecurityQuestionId,
				UL.SecurityAnswer,
				UL.AccountStatus,
				UL.MustChangePwd,
				CONVERT(varchar, UL.StartDate, 103) AS 'StartDate',
				CONVERT(varchar, UL.EndDate, 103) AS 'EndDate',
				UL.LogonAttempts,
				UL.SecurityQuestionAttempts,
				UR.[UserRoleID],
				UL.Organization_ID,
				U.*
		FROM	UserLogon UL
				
				INNER JOIN [User] U
					ON UL.UserId = U.UserId
					left join [notify].[dbo].[UserRoleMapping] UR on  UR.UserLogonID=UL.UserLogonId
		WHERE	UL.UserLogonId = @UserLogonId;

		SELECT	@SqlError = @@error;				
		IF (@SqlError <> 0) GOTO ErrHandler;
	END

	SET NOCOUNT OFF;
	SET ARITHIGNORE OFF;
	
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
		RETURN 0;
END


GO
