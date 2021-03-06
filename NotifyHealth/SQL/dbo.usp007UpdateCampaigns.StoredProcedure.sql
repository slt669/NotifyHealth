USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp007UpdateCampaigns]    Script Date: 22/02/2019 16:07:05 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[usp007UpdateCampaigns]
(
@OrganizationId integer,
@CampaignId integer,
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
**	Object:			dbo.usp007UpdateCampaigns.
**	Date:			12/01/2019.
**	Author:			Stephen Thompson.
**	Description:	This procedure allows updates to the Campaigns table.
**************************************************************************************************************/

BEGIN

	SET NOCOUNT ON
	SET ARITHIGNORE ON;

	DECLARE @SQLError int;
	DECLARE @ReturnMessage varchar(300);
	
	SET	@SQLError = 0;

	IF (SELECT count(*) FROM campaigns WHERE Campaign_ID=@CampaignId AND Organization_ID=@OrganizationId)=1
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			UPDATE campaigns
			SET Description=@Description, 
				Name=@Name,
				 Status_ID=@StatusId,
				 Edited_By=@Edited_By,
				 Edited_When=@Edited_When
			WHERE Organization_ID=@OrganizationId AND Campaign_ID=@CampaignId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		ELSE
			BEGIN
			DELETE campaigns
			WHERE Organization_ID=@OrganizationId AND Campaign_ID=@CampaignId;

			SELECT	@SQLError = @@error;				
			IF (@SQLError <> 0) GOTO ErrorHandler;
			END
		END
	ELSE
		BEGIN
		IF @Delete<>'Y'
			BEGIN
			INSERT INTO campaigns (Organization_ID,Description,Program_ID,Name,Status_ID,Created_By,Created_When,Edited_By,Edited_When)
			SELECT @OrganizationId,@Description,@ProgramId,@Name,@StatusId,@Created_By,@Created_When,null,null;

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
