USE [notify]
GO
/****** Object:  UserDefinedFunction [dbo].[ValidatePhoneNumber]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create FUNCTION [dbo].[ValidatePhoneNumber] 
(
	@PhoneNumber varchar(30), 
	@NumberType varchar(20)
)
RETURNS integer AS
BEGIN
	-- Parameter @NumberType should be 'Mobile', 'Home', 'Work' or 'Fax'.

	DECLARE @ReturnError varchar(100);
	DECLARE @PhoneNumberValue varchar(30)
	DECLARE @NumberTypeValue varchar(20)
	
	SELECT	@PhoneNumberValue = LTRIM(RTRIM(ISNULL(@PhoneNumber, '')));
	SELECT	@NumberTypeValue = LTRIM(RTRIM(ISNULL(@NumberType, '')));

	-- Validate values passed
	SELECT	@ReturnError = 
			CASE	WHEN ISNUMERIC(@PhoneNumberValue) = 0 THEN 
					CASE	WHEN @NumberTypeValue = 'Mobile' THEN 10106
							WHEN @NumberTypeValue = 'Work' THEN 10107
							WHEN @NumberTypeValue = 'Home' THEN 10108
							WHEN @NumberTypeValue = 'Fax' THEN 10109
					ELSE	10110
					END
					WHEN SUBSTRING(@PhoneNumberValue, 1, 1) <> '0' THEN 
					CASE	WHEN @NumberTypeValue = 'Mobile' THEN 10111
							WHEN @NumberTypeValue = 'Work' THEN 10112
							WHEN @NumberTypeValue = 'Home' THEN 10113
							WHEN @NumberTypeValue = 'Fax' THEN 10114
					ELSE	10115
					END
					WHEN SUBSTRING(@PhoneNumberValue, 1, 2) <> '07' AND	@NumberTypeValue = 'Mobile' THEN 10116
					WHEN LEN(@PhoneNumberValue) <> 11 THEN
					CASE	WHEN @NumberTypeValue = 'Mobile' THEN 10117
							WHEN @NumberTypeValue = 'Work' THEN 10118
							WHEN @NumberTypeValue = 'Home' THEN 10119
							WHEN @NumberTypeValue = 'Fax' THEN 10120
					ELSE	10121
					END
			ELSE	0
			END

	-- Return the final output
	RETURN (@ReturnError)
END

GO
