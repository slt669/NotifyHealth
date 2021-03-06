USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp119GetCampaignDDL]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp119GetCampaignDDL]
(
@Program_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp119GetCampaignDDL.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns all Campaigns for a DropDownList by Program_ID.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	SELECT
	Campaign_ID
	,Name
	FROM campaigns 
	where Program_ID = @Program_ID
  
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
