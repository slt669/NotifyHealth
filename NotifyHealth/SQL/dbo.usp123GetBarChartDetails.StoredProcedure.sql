USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp123GetBarChartDetails]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE PROCEDURE [dbo].[usp123GetBarChartDetails]
(
@OrganizationID Integer
)

as

/*************************************************************************************************************
**	Object:			dbo.usp123GetBarChartDetails.
**	Date:			09/01/2019
**	Author:			Stephen Thompson
**	Description:	This procedure returns the Bar Chart for a specified Organization.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);


		Select DateName( month , DateAdd( month , MONTH(t.timestamp) , -1 ) )as 'ReportingMonth',
	     count(t.[Organization_ID])   as 'Notifications',
		  DateName( year , DateAdd(  year, year(t.timestamp),-1 ) )as 'ReportingYear'
		FROM [notify].[dbo].[transactions] t
		where t.timestamp between  DateAdd(yy, -1, GetDate()) and GetDate() 
		AND t.[Organization_ID] =@OrganizationID 
		group by MONTH(t.timestamp)
	    ,t.[Organization_ID], year(t.timestamp)
		order by  year(t.timestamp),MONTH(t.timestamp)
		,t.[Organization_ID]

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
