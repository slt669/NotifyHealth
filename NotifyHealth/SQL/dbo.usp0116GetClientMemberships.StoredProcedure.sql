USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp0116GetClientMemberships]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp0116GetClientMemberships]
(
@Organization_ID integer,
@Client_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp0116GetClientMemberships.
**	Date:			14/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Client Memberships for an Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;
	SELECT
	   CM.Client_Membership_ID
      ,CM.Client_ID
      ,CM.Campaign_ID
      ,CM.Start
      ,CM.Appointment
      ,CM.Organization_ID
	  ,PR.Name as 'Program'
	  ,C.Name as 'Campaign'
  FROM [notify].[dbo].[client_memberships] CM
	LEFT JOIN Campaigns C ON CM.Campaign_ID = C.Campaign_ID
	LEFT JOIN programs PR ON C.Program_ID=PR.Program_ID
	WHERE CM.Organization_ID=@Organization_ID 
	AND CM.Client_ID = @Client_ID;

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
