USE [notify]
GO
/****** Object:  UserDefinedFunction [dbo].[ChangeToPascalCase]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[ChangeToPascalCase] 
(
	@String varchar(100)
)
	RETURNS varchar(100) AS
BEGIN
	DECLARE @outputString VARCHAR(100);
	DECLARE @stringLength INT;
	DECLARE @loopCounter INT;
	DECLARE @charAtPos VARCHAR(1);
	DECLARE @wordStart INT;

	-- Initialize variables
	SET @outputString = '';
	SET @stringLength = LEN(@String);
	SET @loopCounter = 1;
	SET @wordStart = 1;

	-- Loop over the string
	WHILE (@loopCounter <= @stringLength)
	BEGIN
		-- Get the single character off the string
		SET @charAtPos = SUBSTRING(@String, @loopCounter, 1);

		-- If we are the start of a word, uppercase the character
		-- and reset the work indicator
		IF (@wordStart = 1)
		BEGIN
			SET @charAtPos = UPPER(@charAtPos);
			SET @wordStart = 0;
		END

		-- If we encounter a white space, indicate that we
		-- are about to start a word
		IF ((@charAtPos = ' ') OR (@charAtPos = '-'))
			SET @wordStart = 1;

		-- Form the output string
		SET @outputString = @outputString + @charAtPos;

		SET @loopCounter = @loopCounter + 1;
	END

	-- Return the final output
	RETURN (@outputString);
END

GO
