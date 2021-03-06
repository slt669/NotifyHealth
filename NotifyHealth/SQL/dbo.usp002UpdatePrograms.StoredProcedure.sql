USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp002UpdatePrograms]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp002UpdatePrograms]
(
@OrganizationId integer,
@ProgramId integer,
@Description varchar(200)='',
@Name varchar(200)='',
@Created_By integer,
@Edited_By integer,
@Created_When datetime,
@Edited_When datetime,
@StatusId integer,
@Delete char(1)='N'
)

as

/*************************************************************************************************************
**	Object:			dbo.usp002UpdatePrograms.
**	Date:			09/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure allows updates to the Programs table.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	IF (SELECT count(*) FROM Programs WHERE Organization_ID=@OrganizationId AND Program_ID=@ProgramId)=1
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			UPDATE Programs
			SET Description=@Description, 
				Name=@Name,
				 Status_ID=@StatusId,
				 Edited_By=@Edited_By,
				 Edited_When=@Edited_When
			WHERE Organization_ID=@OrganizationId AND Program_ID=@ProgramId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		ELSE
			BEGIN
			DELETE Programs
			WHERE Organization_ID=@OrganizationId AND Program_ID=@ProgramId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END
	ELSE
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			INSERT INTO Programs (Organization_ID,Description,Name,Status_ID,Created_By,Created_When,Edited_By,Edited_When)
			SELECT @OrganizationId,@Description,@Name,@StatusId,@Created_By,@Created_When,null,null;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END

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
