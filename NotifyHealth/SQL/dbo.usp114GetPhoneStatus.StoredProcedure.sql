USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp114GetPhoneStatus]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


/*************************************************************************************************************
**	Object:			dbo.usp114GetPhoneStatus.
**	Date:			12/01/2019.
**	Author:			Stephen Thompson.
**	Description:	Lists the Phone Statuses for dropdown
**************************************************************************************************************/

Create PROCEDURE [dbo].[usp114GetPhoneStatus]
AS
BEGIN
	SET NOCOUNT ON;
	SET ARITHIGNORE ON;
	
	DECLARE	@SqlError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SqlError = 0;
	
SELECT TOP 1000 [P_Status_ID]
      ,[Value]
  FROM [notify].[dbo].[phone_status]

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
