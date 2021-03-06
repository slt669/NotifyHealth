USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp004GetClients]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp004GetClients]
(
@Organization_ID integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp004GetClients.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure returns Clients for an Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	SELECT CL.Client_ID
      ,CL.C_Status_ID
      ,CL.First_Name
      ,CL.Last_Name
      ,CL.Photo
      ,CL.Phone_Number
      ,CL.Phone_Carrier
      ,CL.Message_Address
      ,CL.P_Status_ID
      ,CL.Participation_ID
      ,CL.Added
      ,CL.A_Type_ID
      ,CL.Organization_ID
      ,CL.Created_By
      ,CL.Edited_By
      ,CL.Created_When
      ,CL.Edited_When
	  ,PR.Value as 'ParticipationReason'
	  ,CS.Value as 'ClientStatus'
	  ,PS.Value as 'PhoneStatus'
	  ,AT.Value as 'AccountType'
	FROM Clients CL
	LEFT JOIN client_status CS ON CL.C_Status_ID=CS.C_Status_ID
	LEFT JOIN participation_reason PR ON CL.Participation_ID = PR.Participation_ID
	LEFT JOIN phone_status PS ON CL.P_Status_ID=PS.P_Status_ID 
	LEFT JOIN account_type AT ON CL.A_Type_ID=AT.A_Type_ID 
	
	WHERE CL.Organization_ID=@Organization_ID;

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
