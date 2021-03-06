USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp102ManageAccount]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp102ManageAccount]
	@SessionId int,
	@SessionGUId varchar(36),
	@UpdateType varchar(20),
	@UserLogonId int = NULL,
	@Username varchar(50) = NULL,
	@Password varchar(50) = NULL,
	@PasswordRepeat varchar(50) = NULL,
	@Title varchar(10) = NULL,
	@Forename varchar(50) = NULL,
	@Surname varchar(50) = NULL,
	@EmailAddress varchar(50) = NULL,
	@MobileTelephoneNo varchar(50) = NULL,
	@HomeTelephoneNo varchar(50) = NULL,
	@WorkTelephoneNo varchar(50) = NULL,
	@FaxTelephoneNo	varchar(50) = NULL,
	@JobTitle varchar(50) = NULL,
	@SecurityQuestionId int = NULL,
	@SecurityAnswer varchar(50) = NULL, 
	@StartDate datetime = NULL,
	@EndDate datetime = NULL,
	@MustChangePassword bit = 0,
	@AssignedUserRoleIds varchar(100) = NULL,
	@Photo varchar(500) = NULL,
	@ValidationMessage varchar(500) OUTPUT,
	@ValidationErrorNo varchar(10) OUTPUT
AS

/*********************************************************************************
**	Object:			dbo.usp102ManageAccount
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Manages the account details for the system user and issues 
**                  a modification notice to be sent to the registered 
**					email address.
**********************************************************************************/
BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE	@ValidationError int;
	DECLARE @ReturnMessage varchar(300);
	DECLARE @UserId int;

	SET	@SqlError = 0;
	SET @ValidationError = 0;
	SET @ValidationErrorNo = '0';
	
	-- First Validate the parameter being passed in and only after it passes do we proceed
	SELECT	@ValidationError = 
			CASE	-- Is this a Valid set of session credentials and is account in correct state
					WHEN (	SELECT	COUNT(*)
							FROM	UserSession
							WHERE	UserSessionId = @SessionId
							AND		UserSessionGUId = @SessionGUId) < 1 THEN 10011
					WHEN @UpdateType IN ('AdminUpdate', 'EndUserUpdate') THEN
					CASE
						-- The password passed is an inValid length
						-- The security question isn't Valid
						WHEN (	SELECT	COUNT(*)
								FROM	SecurityQuestion
								WHERE	SecurityQuestionId = @SecurityQuestionId) < 1 AND @SecurityQuestionId is not null THEN 10102
						-- Has a Valid security answer been provIded
						WHEN LEN(ISNULL(LTRIM(RTRIM(@SecurityAnswer)), '')) <= 0 AND @SecurityAnswer is not null THEN 10008
						WHEN (LEN(ISNULL(LTRIM(RTRIM(@StartDate)), '')) > 0) AND (LEN(ISNULL(LTRIM(RTRIM(@EndDate)), '')) > 0) AND (@StartDate > @EndDate) THEN 10022
						WHEN LEN(ISNULL(LTRIM(RTRIM(@Forename)), '')) <= 0 THEN 10103
						WHEN LEN(ISNULL(LTRIM(RTRIM(@Surname)), '')) <= 0 THEN 10104
						WHEN LEN(ISNULL(LTRIM(RTRIM(@MobileTelephoneNo)), '')) > 0 AND (dbo.ValidatePhoneNumber(@MobileTelephoneNo, 'Mobile') <> 0) THEN dbo.ValidatePhoneNumber(@MobileTelephoneNo, 'Mobile')
						WHEN LEN(ISNULL(LTRIM(RTRIM(@HomeTelephoneNo)), '')) > 0 AND (dbo.ValidatePhoneNumber(@HomeTelephoneNo, 'Home') <> 0) THEN dbo.ValidatePhoneNumber(@HomeTelephoneNo, 'Home')
						WHEN LEN(ISNULL(LTRIM(RTRIM(@WorkTelephoneNo)), '')) > 0 AND (dbo.ValidatePhoneNumber(@WorkTelephoneNo, 'Work') <> 0) THEN dbo.ValidatePhoneNumber(@WorkTelephoneNo, 'Work')
						WHEN LEN(ISNULL(LTRIM(RTRIM(@FaxTelephoneNo)), '')) > 0 AND (dbo.ValidatePhoneNumber(@FaxTelephoneNo, 'Fax') <> 0) THEN dbo.ValidatePhoneNumber(@FaxTelephoneNo, 'Fax')
						WHEN (ISNULL(@UserLogonId, 0) > 0) AND (SELECT COUNT(*) FROM UserLogon WHERE UserLogonId = @UserLogonId) <= 0 THEN 10123
						WHEN LEN(ISNULL(LTRIM(RTRIM(@Password)), '')) > 0 THEN
						CASE
							WHEN	LEN(@Password) < 6 THEN 10100 
							WHEN	PATINDEX ('%[0-9]%',@Password) = 0 THEN 10101
							WHEN (	SELECT	COUNT(*)
									FROM	UserSession US
											INNER JOIN UserLogon UL
												ON US.UserLogonId = UL.UserLogonId
									WHERE	US.UserSessionId = @SessionId
									AND		US.UserSessionGUId = @SessionGUId
									AND		UL.MustChangePwd = 1
									AND		UL.Password = @Password) > 0 THEN 10105
							WHEN @Password<>@PasswordRepeat AND @PasswordRepeat is not null THEN 10124
							ELSE	0
						END
						ELSE 0
					END
				ELSE	0
		 	END;
	 	
	IF (@ValidationError <> 0) 
		BEGIN
		SELECT @ValidationMessage=ErrorMessage FROM ErrorMessage WHERE ErrorMessageId=@ValidationError;	
		SELECT @ValidationErrorNo=CONVERT(varchar(10),@ValidationError);			
		GOTO ErrHandler;
		END
	ELSE
		BEGIN
		SET @ValidationErrorNo = '0';
		SET @ValidationMessage =  'Details Validated';
		END

	-- If this procedure receives at UserLogonId, then it is being called from the Admin section and we're not modifying the
	-- currently logged in account but the account related to the UserLogonId
	IF (@UpdateType IN ('AdminUpdate', 'EndUserUpdate'))
	BEGIN
		IF (@UpdateType = 'AdminUpdate')
		BEGIN
			SELECT	@Password = [Password],
					@UserId = UserId
			FROM	UserLogon
			WHERE	UserLogonId = @UserLogonId;
			
			SELECT	@SqlError = @@error;				
			IF (@SqlError <> 0) GOTO ErrHandler;
		END
		ELSE
		BEGIN
			SELECT	@UserLogonId = UL.UserLogonId,
					@Username = UL.Username,
					@UserId = UL.UserId
			FROM	UserSession US
					INNER JOIN UserLogon UL
						ON US.UserLogonId = UL.UserLogonId
			WHERE	US.UserSessionId = @SessionId
			AND		US.UserSessionGUId = @SessionGUId;
			
			SELECT	@SqlError = @@error;				
			IF (@SqlError <> 0) GOTO ErrHandler;
		END

		SELECT	@Forename = dbo.ChangeToPascalCase(@Forename);
		SELECT	@Surname = dbo.ChangeToPascalCase(@Surname);
		
		-- If we must change the password (activation request), then let's generate it
		IF (@MustChangePassword = 1)
		BEGIN
			SELECT	@Password = dbo.GetNewPassword(NEWId());
		END

		BEGIN TRANSACTION
		
		UPDATE	UserLogon
		SET		Username = CASE WHEN @UpdateType = 'AdminUpdate' THEN @Username ELSE Username END,
				[Password] = CASE WHEN LEN(ISNULL(LTRIM(RTRIM(@Password)), '')) > 0 THEN @Password ELSE [Password] END,
				SecurityQuestionId = @SecurityQuestionId,
				SecurityAnswer = @SecurityAnswer,
				StartDate = CASE WHEN @UpdateType = 'AdminUpdate' THEN @StartDate ELSE StartDate END,
				EndDate = CASE WHEN @UpdateType = 'AdminUpdate' THEN @EndDate ELSE EndDate END,
				MustChangePwd = @MustChangePassword,
				AccountStatus = CASE WHEN @UpdateType = 'EndUserUpdate' THEN 'Active' ELSE AccountStatus END
		WHERE	UserLogonId = @UserLogonId;

		SELECT	@SqlError = @@error;				
		IF (@SqlError <> 0) GOTO ErrHandler;

		UPDATE	[User]
		SET		Title = @Title,
				Forename = CASE WHEN LTRIM(RTRIM(isnull(@Forename,'')))='' THEN Forename ELSE @Forename END,
				Surname = CASE WHEN LTRIM(RTRIM(isnull(@Surname,'')))='' THEN Surname ELSE @Surname END,
				UserStatus = CASE WHEN @UpdateType = 'EndUserUpdate' THEN 'Active' ELSE UserStatus END,
				EmailAddress = CASE WHEN @UpdateType = 'AdminUpdate' THEN @EmailAddress ELSE EmailAddress END,
				MobileTelephoneNo = CASE WHEN LTRIM(RTRIM(isnull(@MobileTelephoneNo,'')))='' THEN MobileTelephoneNo ELSE @MobileTelephoneNo END,
				HomeTelephoneNo = CASE WHEN LTRIM(RTRIM(isnull(@HomeTelephoneNo,'')))='' THEN HomeTelephoneNo ELSE @HomeTelephoneNo END,
				WorkTelephoneNo = CASE WHEN LTRIM(RTRIM(isnull(@WorkTelephoneNo,'')))='' THEN WorkTelephoneNo ELSE @WorkTelephoneNo END,
				FaxTelephoneNo = CASE WHEN LTRIM(RTRIM(isnull(@FaxTelephoneNo,'')))='' THEN FaxTelephoneNo ELSE @FaxTelephoneNo END,
				JobTitle = CASE WHEN LTRIM(RTRIM(isnull(@JobTitle,'')))='' THEN JobTitle ELSE @JobTitle END,
				Photo = CASE WHEN LEN(ISNULL(LTRIM(RTRIM(@Photo)), '')) > 0 THEN @Photo ELSE Photo END 
		WHERE	UserId = @UserId;

		SELECT	@SqlError = @@error;				
		IF (@SqlError <> 0) GOTO ErrHandler;
		
		-- Now if we've been passed user roles to assign, do so
		IF ((@UpdateType = 'AdminUpdate') AND (LEN(@AssignedUserRoleIds) > 0))
		BEGIN
			DECLARE @UserRoleId int;
			DECLARE @CurrentUserRoles varchar(100);
			DECLARE @CurrentRoleId int;
			DECLARE	@delimiterLoc int;
			DECLARE @startIndex int;
			DECLARE @UserRoleCount int;
			DECLARE @SystemAdmin bit;
			DECLARE @InternalAdmin bit;
			DECLARE @CompanyAdmin bit;
			
			SELECT	@UserRoleCount = COUNT(*)
			FROM	UserRoleMapping
			WHERE	UserLogonId = @UserLogonId
			
			SELECT	@startIndex = 0,
					@CurrentRoleId = 0,
					@CurrentUserRoles = '';

			WHILE (@startIndex < @UserRoleCount)
			BEGIN
				SELECT	TOP 1 @CurrentRoleId = UserRoleId
				FROM	UserRoleMapping
				WHERE	UserLogonId = @UserLogonId
				AND		UserRoleId > @CurrentRoleId
				ORDER
				BY		UserRoleId ASC;

				SELECT	@CurrentUserRoles += CONVERT(varchar, @CurrentRoleId) + '|';			
				SELECT	@startIndex += 1;
			END
			
			IF (@CurrentUserRoles <> @AssignedUserRoleIds)
			BEGIN
				-- Now we need to determine this accounts permissions in order to know what roles this person can delete
				SELECT	@CompanyAdmin = MAX(CASE WHEN UR.RoleType = 'CompanyAdmin' THEN 1 ELSE 0 END),
						@SystemAdmin = MAX(CASE WHEN UR.RoleType = 'SystemAdmin' THEN 1 ELSE 0 END),
						@InternalAdmin = MAX(CASE WHEN UR.RoleType = 'InternalAdmin' THEN 1 ELSE 0 END)
				FROM	UserSession US
						INNER JOIN UserRoleMapping URM
							ON US.UserLogonId = URM.UserLogonId
						INNER JOIN UserRole UR
							ON URM.UserRoleId = UR.UserRoleId
				WHERE	US.UserSessionId = @SessionId
				AND		US.UserSessionGUId = @SessionGUId;

				-- Remove all the current roles as we're about to replace them with the newly assigned list
				DELETE
				FROM	UserRoleMapping
				WHERE	UserLogonId = @UserLogonId
				AND		UserRoleId IN 
				(
					SELECT	UserRoleId
					FROM	UserRole
					WHERE	((@CompanyAdmin = 1) AND (RoleType NOT IN ('CompanyAdmin', 'SystemAdmin', 'InternalAdmin')))
					OR		(@SystemAdmin = 1)
					OR		((@InternalAdmin = 1) AND (RoleType NOT IN ('SystemAdmin', 'InternalAdmin')))
				)
					
				SELECT	@startIndex = 0,
						@delimiterLoc = 0;
				
				WHILE (@startIndex < LEN(@AssignedUserRoleIds))
				BEGIN
					SELECT	@delimiterLoc = CHARINDEX('|', @AssignedUserRoleIds, @startIndex);
					SELECT	@UserRoleId = SUBSTRING(@AssignedUserRoleIds, @startIndex, @delimiterLoc - @startIndex);	
					SELECT	@startIndex = @delimiterLoc + 1;
					
					INSERT
					INTO	UserRoleMapping(UserLogonId, UserRoleId)
					SELECT	@UserLogonId, @UserRoleId
		
					SELECT	@SqlError = @@error;
					IF (@SqlError <> 0) GOTO ErrHandler;	
				END
			END
			
		END
		
		COMMIT TRANSACTION
	END
	ELSE
	BEGIN
		IF (@UpdateType = 'EndUserUnlock')
		BEGIN
			BEGIN TRANSACTION

			UPDATE	UserLogon
			SET		AccountStatus = 'Active',
					LogonAttempts = NULL,
					SecurityQuestionAttempts = NULL
			WHERE	UserLogonId = @UserLogonId;

			SELECT	@SqlError = @@error;				
			IF (@SqlError <> 0) GOTO ErrHandler;

			COMMIT TRANSACTION
		END
	END
	
	-- Now issue the account activation request
	-- *** EMAIL THANG TO THE INSERTED HERE!!! ***

	-- List all the account details for this session
	SELECT	UL.*, U.*, U.Forename + ' ' + U.Surname AS 'EndUserName'
	FROM	UserLogon UL
			INNER JOIN [User] U
				ON UL.UserId = U.UserId
	WHERE	UL.UserLogonId = @UserLogonId

	SELECT	@SqlError = @@error;				
	IF (@SqlError <> 0) GOTO ErrHandler;

	SET NOCOUNT OFF;
	SET ARITHIGNORE OFF;

	SET @ValidationMessage =  'Details Updated';	
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
