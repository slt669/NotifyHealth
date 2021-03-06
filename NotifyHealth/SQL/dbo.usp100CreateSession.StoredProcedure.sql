USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp100CreateSession]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp100CreateSession]
	@Username varchar(50),
	@Password varchar(50),
	@PageName varchar(100)=null,
	@ValidationMessage varchar(500) OUTPUT,
	@ValidationErrorNo varchar(10) OUTPUT
AS

/***************************************************************************************
**	Object:			dbo.usp100CreateSession.
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Checks a Username and Password combination are Valid and return
**					session details.
****************************************************************************************/

BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE	@ValidationError int;
	DECLARE @ReturnMessage varchar(300);
	DECLARE @UserLogonId int;
	DECLARE @TenantID int;
	DECLARE @MustChangePwd int;
	DECLARE @SuperUser bit;
	DECLARE @SystemAdmin bit;
	DECLARE @InternalAdmin bit;
	DECLARE @SessionId int;
	DECLARE @PageAccess char(1);
	
	SET	@SqlError = 0;
	SET @ValidationError = 0;

	-- Firstly check page access.
	SELECT @PageAccess=CASE WHEN (SELECT count(*) FROM	UserLogon UL
--			INNER JOIN UserSession US 
--				ON UL.UserLogonId = US.UserLogonId
			INNER JOIN UserRoleMapping URM
				ON UL.UserLogonId = URM.UserLogonId
			INNER JOIN UserRole UR
				ON URM.UserRoleId = UR.UserRoleId
			INNER JOIN UserRoleAccess URA ON URA.UserRoleId=UR.UserRoleId
	WHERE	UL.Username = @Username
	AND @PageName LIKE URA.PageName)>0 THEN 'Y' ELSE 'N' END;
	
	SELECT	@SqlError = @@error;
	IF (@SqlError <> 0) GOTO ErrHandler;
	
	-- Secondly Validate the parameters being passed in and only after they pass do we insert a session and return the session details
	SELECT	@ValidationError = 
			CASE	-- If the username passed doesn't have any length (should happen as prevented on client but can't assume
					WHEN LEN(LTRIM(RTRIM(@Username))) < 1 THEN 10000
					-- If the users account is locked
					WHEN (	SELECT	COUNT(*)
							FROM	UserLogon
							WHERE	Username = @Username
							AND		LOWER(AccountStatus) = 'Locked') > 0 THEN 10001
					-- If the users account has expired
					WHEN (	SELECT	COUNT(*)
							FROM	UserLogon
							WHERE	Username = @Username
							AND		(LOWER(AccountStatus) = 'Expired' OR EndDate < getdate())) > 0 THEN 10002
					-- If the users account has been set to inactive
					WHEN (	SELECT	COUNT(*)
							FROM	UserLogon
							WHERE	Username = @Username
							AND		LOWER(AccountStatus) = 'Inactive') > 0 THEN 10003
					-- If the usename and password passed aren't Valid
					WHEN (	SELECT	COUNT(*) 
							FROM	UserLogon
							WHERE	Username = @Username 
							AND		[Password] = @Password) < 1 THEN 10004
					-- If the username doesn't have any user roles associated with it
					WHEN (	SELECT	COUNT(*)
							FROM	UserLogon UL
									INNER JOIN UserRoleMapping URM
										ON UL.UserLogonId = URM.UserLogonId
									INNER JOIN UserRole UR
										ON URM.UserRoleId = UR.UserRoleId
							WHERE	UL.Username = @Username) < 1 THEN 10005
					-- None of the above has Validated true so the logon credentials passed are correct
					WHEN @PageAccess='N' THEN 10015
					ELSE 0
		 	END;
		 	
	IF (@ValidationError <> 0)
	BEGIN
		-- If this is a failed login attempt
		IF (@ValidationError = 10004)
		BEGIN
			BEGIN TRANSACTION;
			
			-- Update the UserLogon table to reflect the failed logon attempt
			UPDATE	UserLogon
			SET		LogonAttempts = ISNULL(LogonAttempts, 0) + 1
			WHERE	Username = @Username;

			SELECT	@SqlError = @@error;				
			IF (@SqlError <> 0) GOTO ErrHandler;
			
			-- If we've now breached the maximum number of logon attempts, lock the account
			IF (SELECT	LogonAttempts 
				FROM	UserLogon
				WHERE	Username = @Username) > 2
			BEGIN
				UPDATE	UserLogon
				SET		AccountStatus = 'Locked'
				WHERE	Username = @Username;

				SELECT	@SqlError = @@error;				
				IF (@SqlError <> 0) GOTO ErrHandler;
			END
			
			COMMIT TRANSACTION;
		END
		
		SELECT @ValidationMessage=ErrorMessage FROM ErrorMessage WHERE ErrorMessageId=@ValidationError;
		SELECT @ValidationErrorNo=CONVERT(varchar(10),@ValidationError);			
		GOTO ErrHandler;
	END
	-- We've passed Validation so process the logon request, updated the UserLogon table and create the session
	ELSE
	BEGIN
		BEGIN TRANSACTION
		
		-- Reset the logon
		UPDATE	UserLogon
		SET		LogonAttempts = 0
		WHERE	Username = @Username
		AND		LogonAttempts > 0;
		
		SELECT	@SqlError = @@error;
		IF (@SqlError <> 0) GOTO ErrHandler;
		
		SELECT	@UserLogonId = UserLogonId,
				@TenantID = Organization_ID,
				@MustChangePwd = MustChangePwd
		FROM	UserLogon
		WHERE	Username = @Username;

		SELECT	@SqlError = @@error;
		IF (@SqlError <> 0) GOTO ErrHandler;
		
		-- Is this login a super user or internal administrator?
		SELECT	@SuperUser = MAX(CASE WHEN RoleType = 'SuperUser' THEN '1' ELSE 0 END),
				@SystemAdmin = MAX(CASE WHEN RoleType = 'SystemAdmin' THEN '1' ELSE 0 END),
				@InternalAdmin = MAX(CASE WHEN RoleType = 'InternalAdmin' THEN '1' ELSE 0 END)
		FROM	UserRoleMapping URM
				INNER JOIN UserRole UR
					ON UR.UserRoleId = URM.UserRoleId
		WHERE	URM.UserLogonId = @UserLogonId

		SELECT	@SqlError = @@error;
		IF (@SqlError <> 0) GOTO ErrHandler;

		INSERT 
		INTO	UserSession
		(
				UserSessionGUId,
				SessionStatus,
				UserLogonId,
				LastAccessed,
				PageData,
				Organization_ID
		)
		SELECT 	NEWId()
				,'Logged On'
				,@UserLogonId
				,GETDATE()
				,NULL
				,(SELECT TOP 1 Organization_ID FROM OrganizationUserLogon TUL WHERE TUL.UserLogonID=@UserLogonId ORDER BY Organization_ID ASC)

		SELECT	@SqlError = @@error, @SessionId = @@IdENTITY;
		IF (@SqlError <> 0) GOTO ErrHandler;

		SELECT	UserSessionId,
				LOWER(CONVERT(varchar(36), UserSessionGUId)) AS 'UserSessionGUId',
				SessionStatus as 'SessionStatus',
				@MustChangePwd AS 'MustChangePwd',
				@SuperUser AS 'SuperUser',
				@SystemAdmin AS 'SystemAdmin',
				@InternalAdmin AS 'InternalAdmin',
				Organization_ID
		FROM	UserSession
		WHERE	UserSessionId = @SessionId;

		SELECT	@SqlError = @@error, @SessionId = @@IdENTITY;
		IF (@SqlError <> 0) GOTO ErrHandler;

		SET NOCOUNT OFF;
		SET ARITHIGNORE OFF;

		SET @ValidationErrorNo = 0;
		SET @ValidationMessage =  'Logged On';

		COMMIT TRANSACTION;
	END	
	
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
