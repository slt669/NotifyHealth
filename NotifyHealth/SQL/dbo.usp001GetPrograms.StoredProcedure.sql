USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp001GetPrograms]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp001GetPrograms]
(
@Organization_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp001GetPrograms.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Programs for an Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;


  	SELECT programs.*,status.Value, count(campaigns.Program_ID) as 'RelatedCampaigns'        
	from programs
	LEFT JOIN status  ON status.Status_ID=programs.Status_ID
	left join campaigns on (programs.Program_ID = campaigns.Program_ID)WHERE programs.Organization_ID=@Organization_ID

	group by
	programs.name,programs.description,programs.status_ID,programs.Program_ID,programs.Organization_ID
   ,status.Status_ID,status.Value,programs.Created_By,programs.Edited_By,programs.Created_When,programs.Edited_When


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
