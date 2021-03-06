USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp124GetSMSAccount]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp124GetSMSAccount]
(
@OrganizationID Integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp124GetSMSAccount.
**	Date:			09/01/2019
**	Author:			Stephen Thompson
**	Description:	This procedure returns the SMSAccounts for an Organitzation.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);

	SELECT  [Organization_ID]
      ,[Photo]
      ,[Name]
      ,[Phone_Number]
      ,[SMS_Account]
      ,[SMS_Password]
      ,[API_Username]
      ,[API_Password]
      ,[Status_ID]
  FROM [notify].[dbo].[organization]
  where [Organization_ID] =@OrganizationID
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
