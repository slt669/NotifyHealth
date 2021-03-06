USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp101CheckSession]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



Create PROCEDURE [dbo].[usp101CheckSession]
	@SessionId int,
	@SessionGUId varchar(36),
	@PageName varchar(100)=null,
	@ValIdationMessage varchar(500) OUTPUT,
	@ValIdationErrorNo varchar(10) OUTPUT
AS

/*********************************************************************************
**	Object:			dbo.usp101CheckSession.
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Checks a SessionId and SessionGUId are valId and not timed out.
**********************************************************************************/

BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE	@ValIdationError int;
	DECLARE @ReturnMessage varchar(300);
	DECLARE @SessionTimeout int;
	DECLARE	@PageData varchar(500);
	DECLARE @PageAccess char(1);
	
	SET	@SqlError = 0;
	SET @ValIdationError = 0;
	SET @ValIdationErrorNo = 0;
	SET @PageAccess='N';
	
	-- Firstly we need to retrieve the session timeout. If there isn't one defined then set the default to 15 minutes
	SELECT	@SessionTimeout = ISNULL(MAX(UR.SessionTimeout), 15)
	FROM	UserLogon UL
			INNER JOIN UserSession US 
				ON UL.UserLogonId = US.UserLogonId
			INNER JOIN UserRoleMapping URM
				ON UL.UserLogonId = URM.UserLogonId
			INNER JOIN UserRole UR
				ON URM.UserRoleId = UR.UserRoleId
				AND UR.RoleType = 'UserAccount'
	WHERE	US.UserSessionId = @SessionId 
	AND		LTRIM(RTRIM(CONVERT(varchar(36), US.UserSessionGUId))) = @SessionGUId
	
	SELECT	@SqlError = @@error;
	IF (@SqlError <> 0) GOTO ErrHandler;

	-- Secondly check page access.
	SELECT  @PageAccess =CASE WHEN 

	(SELECT count(*)

	 FROM	UserLogon UL
			INNER JOIN UserSession US 
				ON UL.UserLogonId = US.UserLogonId
			INNER JOIN UserRoleMapping URM
				ON UL.UserLogonId = URM.UserLogonId
			INNER JOIN UserRole UR
				ON URM.UserRoleId = UR.UserRoleId

	        INNER JOIN [UserRoleDataAccessMapping] URAM ON URAM.UserRoleId=UR.UserRoleId

			INNER JOIN UserRoleDataAccess URA ON URA.[UserRoleDataAccessID]=URAM.[UserRoleDataAccessID]

	WHERE	US.UserSessionId = @SessionId 
	AND		LTRIM(RTRIM(CONVERT(varchar(36), US.UserSessionGUId)))= @SessionGUId 	AND @PageName LIKE URA.DataType OR @PageName LIKE '%' )> 0 THEN 'Y' ELSE 'N' END;
	
	
	SELECT	@SqlError = @@error;
	IF (@SqlError <> 0) GOTO ErrHandler;
	
	-- Now valIdate the parameters being passed in and only after it passes do we proceed
	SELECT	@ValIdationError = 
			CASE	-- Is this a valId set of session credentials and is account in correct state
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession
							WHERE	UserSessionId = ISNULL(@SessionId, 0)
							AND		UserSessionGUId = ISNULL(@SessionGUId, '')) < 1 THEN 10011
					WHEN (	SELECT	LastAccessed 
							FROM	UserSession
							WHERE	UserSessionId = @SessionId 
							AND		LTRIM(RTRIM(CONVERT(varchar(36), UserSessionGUId))) = @SessionGUId) < DATEADD(mi, (-1 * @SessionTimeout), getdate()) THEN 10012
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession US 
									INNER JOIN UserLogon UL
										ON US.UserLogonId = UL.UserLogonId
							WHERE	US.UserSessionId = @SessionId 
							AND		LTRIM(RTRIM(CONVERT(varchar(36), US.UserSessionGUId))) = @SessionGUId
							AND		(LOWER(UL.AccountStatus) = 'locked' OR ISNULL(UL.EndDate, GETDATE()) < GETDATE())) > 0 THEN 10006
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession
							WHERE	UserSessionId = @SessionId 
							AND		LTRIM(RTRIM(CONVERT(varchar(36), UserSessionGUId))) = @SessionGUId
							AND		LOWER(SessionStatus) IN ('loggedout', 'timedout')) > 0 THEN 10012
					WHEN @PageAccess='N' THEN 10015
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession US 
									INNER JOIN UserLogon UL
										ON US.UserLogonId = UL.UserLogonId
							WHERE	US.UserSessionId = @SessionId 
							AND		LTRIM(RTRIM(CONVERT(varchar(36), US.UserSessionGUId))) = @SessionGUId
							AND		UL.MustChangePwd=1) > 0 THEN 10105
					ELSE 0
		 	END;
		 	
	IF (@ValIdationError <> 0)
	BEGIN
		IF (@ValIdationError = 10012)
		BEGIN
			BEGIN TRANSACTION

			UPDATE	UserSession
			SET		SessionStatus = 'TimedOut'
			WHERE	UserSessionId = @SessionId
			AND		UserSessionGUId = @SessionGUId
			AND		SessionStatus <> 'TimedOut'

			SELECT	@SqlError = @@error;				
			IF (@SqlError <> 0) GOTO ErrHandler;
			
			COMMIT TRANSACTION
		END
		SELECT @ValIdationMessage=ErrorMessage FROM ErrorMessage WHERE ErrorMessageId=@ValIdationError;	
		SELECT @ValIdationErrorNo=CONVERT(varchar(10),@ValIdationError);			
		GOTO ErrHandler;
	END
	ELSE
	BEGIN
		BEGIN TRANSACTION

		UPDATE	UserSession
		SET		LastAccessed = GETDATE()
		WHERE	UserSessionId = @SessionId
		AND		UserSessionGUId = @SessionGUId

		SELECT	@SqlError = @@error;				
		IF (@SqlError <> 0) GOTO ErrHandler;

		SET @ValIdationErrorNo = 0;
		SET @ValIdationMessage =  'Logged On';
		
		COMMIT TRANSACTION
	END
	
	-- List the session
	SELECT	US.*, U.Forename + ' ' + U.Surname AS 'EndUserName',UL.Organization_ID as 'OrganizationID', O.name AS 'Organization' , UL.MustChangePwd , P.Name as 'Portal',P.Logo
	FROM	UserSession US
			INNER JOIN UserLogon UL
				ON US.UserLogonId = UL.UserLogonId
			INNER JOIN [User] U
				ON UL.UserId = U.UserId
				LEFT JOIN [organization] O
				ON UL.Organization_ID = O.Organization_ID
					LEFT JOIN [portals] P
				ON O.[Portal_ID] = P.[Portal_ID]


	WHERE	US.UserSessionId = @SessionId
	AND		US.UserSessionGUId = @SessionGUId;

	SELECT	@SqlError = @@error;
	IF (@SqlError <> 0) GOTO ErrHandler;

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
