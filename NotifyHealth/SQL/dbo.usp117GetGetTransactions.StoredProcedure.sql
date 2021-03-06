USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp117GetGetTransactions]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp117GetGetTransactions]
(
@Organization_ID integer,
@Client_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp117GetGetTransactions.
**	Date:			14/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Transactions Sent to a Client.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

SELECT T.Transaction_ID
      ,T.Client_ID
      ,T.Notification_ID
      ,T.Result
      ,T.Timestamp
      ,T.Organization_ID
	  , N.Text as 'Notification'
    FROM notify.dbo.transactions T
	LEFT JOIN Notifications N ON T.Notification_ID=N.Notification_ID
	WHERE T.Organization_ID=@Organization_ID AND T.Client_ID=@Client_ID;

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
