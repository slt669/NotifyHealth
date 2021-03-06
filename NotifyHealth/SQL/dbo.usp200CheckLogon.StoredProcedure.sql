USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp200CheckLogon]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp200CheckLogon] 
(@Username nvarchar(50)='',
 @Password varchar(50)=''
)
AS

/***********************************************************************************************************************
**	Object:			dbo.usp200CheckLogon.
**	Date:			19/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Checks a Username and Password combination are valid.
************************************************************************************************************************/

BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE @SQLError int;
	DECLARE @ValidationError int;
	DECLARE @ReturnMessage varchar(300);
	DECLARE @WorkingUsername varchar(50), @WorkingAccountStatus varchar(50), @UserSessionId int;
	DECLARE @WorkingUserLogonId int;
	
	SET	@SQLError = 0;
	
	SELECT UL.[Username]
			,UL.[Password]
			,UL.[AccountStatus]
			,UL.[SecurityQuestionId]
			,UL.[SecurityAnswer]
			,UL.[StartDate]
			,UL.[EndDate]
			,(SELECT TOP 1 TUL.Organization_ID 
				FROM OrganizationUserLogon TUL 
				WHERE TUL.UserLogonID=UL.UserLogonID ORDER BY TUL.[Organization_ID] ASC) as 'DefaultTenantId'
	FROM UserLogon UL
	WHERE UL.Username=@Username;

	IF (@SQLError <> 0) GOTO ErrorHandler;
	
	RETURN 0

	ErrorHandler:
		IF (@@trancount > 0)
			ROLLBACK TRANSACTION;
			
		IF (@SQLError <> 0)
		BEGIN
			SELECT	@ReturnMessage = 'ErrorNo: ' + CONVERT(varchar(20), @SQLError) + '; Description: Error occurred during stored procedure execution. Please look up error number for information.'
			RAISERROR(@ReturnMessage, 16, 1);
			RETURN @SQLError;
		END
		ELSE
			BEGIN
			RETURN @ValidationError;
			END
END


GO
