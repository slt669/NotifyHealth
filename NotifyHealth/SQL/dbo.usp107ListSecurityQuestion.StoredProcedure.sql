USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp107ListSecurityQuestion]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

/*********************************************************************************
**	Object:				dbo.usp107ListSecurityQuestion
**	Date:				19/01/2019.
**	Author:				Stephen Thompson.
**	Description:		Lists the security question for a valid username.
**********************************************************************************/
Create PROCEDURE [dbo].[usp107ListSecurityQuestion]
AS
BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SqlError = 0;
	
	SELECT [SecurityQuestionID], [SecurityQuestion] FROM SecurityQuestion SQ;

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
