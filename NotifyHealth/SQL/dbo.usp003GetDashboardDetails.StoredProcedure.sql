USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp003GetDashboardDetails]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO




CREATE PROCEDURE [dbo].[usp003GetDashboardDetails]
(
@OrganizationID Integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp003GetDashboardDetails.
**	Date:			09/01/2019
**	Author:			Stephen Thompson
**	Description:	This procedure returns the Dashboard details for a specified Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);

	DECLARE @ThisMonth datetime, @LastMonth datetime, @today date = GETDATE();
	DECLARE @NewClientsLast30 integer,@NotificationsSentLast30 integer, @NoOfClients integer;
	DECLARE @Notifications integer,@ReportingMonth integer,@NotificationsSentToday integer, @NoOfClientDate datetime;

	SET	@SQLError = 0;

	SELECT @ThisMonth=getdate() ;
	SELECT @LastMonth=dateadd(mm,-1,@ThisMonth);

	SELECT @NewClientsLast30=count(*)
	FROM Clients C
	WHERE  C.Organization_ID=@OrganizationID AND C.Created_When>=@LastMonth AND C.Created_When<@ThisMonth

	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;
	
	SELECT @NotificationsSentLast30=count(*)
	FROM [notify].[dbo].[transactions] T
	WHERE  T.Organization_ID=@OrganizationID AND T.Timestamp>=@LastMonth AND T.Timestamp<@ThisMonth

	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

	SELECT @NoOfClients=count(*)
	FROM Clients 
	WHERE Organization_ID=@OrganizationID

	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

	SELECT  top 1 @NoOfClientDate =  Created_When
	FROM   Clients
	order by Created_When asc

	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

	SELECT @NotificationsSentToday=count(*)
	FROM [notify].[dbo].[transactions] T
	WHERE  T.Organization_ID=@OrganizationID AND 
	T.Timestamp >= @today
    AND T.Timestamp <  DATEADD(DAY, 1, @today);


	SELECT	@SQLError = @@error;				
	IF (@SQLError <> 0) GOTO ErrorHandler;

	SELECT  
		    isnull(@NewClientsLast30,0) as 'NewClientsLastThirty',
		    isnull(@NotificationsSentLast30,0) as 'NotificationsSentLastThirty',
		    isnull(@NoOfClients,0) as 'NoOfClients',
			isnull(@NotificationsSentToday,0) as 'NotificationsSentToday',
			isnull(@NoOfClientDate,0) as 'NoOfClientDate'

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
