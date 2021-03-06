USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp118GetProgramDDL]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp118GetProgramDDL]
(
@OrganizationID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp118GetProgramDDL.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns all Programs for a DropDownList.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	SELECT
	Program_ID
      ,Name
	FROM programs 
    where Organization_ID = @OrganizationID

	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

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
