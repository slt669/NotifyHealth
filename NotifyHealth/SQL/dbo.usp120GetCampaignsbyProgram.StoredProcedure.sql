USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp120GetCampaignsbyProgram]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp120GetCampaignsbyProgram]
(
@Organization_ID integer,
@ProgramId integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp120GetCampaignsbyProgram.
**	Date:			16/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Campaigns by Program.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

   SELECT c.Campaign_ID
      ,c.Name
      ,c.Description
      ,c.Status_ID
      ,c.Program_ID
      ,c.Organization_ID
      ,c.Created_By
      ,c.Edited_By
      ,c.Created_When
      ,c.Edited_When
	  ,s.Value
  FROM notify.dbo.campaigns c
  LEFT JOIN status s  ON s.Status_ID=c.Status_ID
  where Program_ID = @ProgramId AND Organization_ID = @Organization_ID



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
