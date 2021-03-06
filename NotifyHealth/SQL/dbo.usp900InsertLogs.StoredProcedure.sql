USE [notify]
GO
/****** Object:  StoredProcedure [dbo].[usp900InsertLogs]    Script Date: 22/02/2019 16:07:06 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

Create PROCEDURE [dbo].[usp900InsertLogs]
	@log_date datetime2, 
  @log_thread varchar(50), 
  @log_level varchar(50), 
  @log_source varchar(300), 
  @log_message varchar(4000), 
  @exception varchar(4000)
AS
BEGIN
	SET NOCOUNT ON;

  insert into dbo.Logs (LogDate, LogThread, LogLevel, LogSource, LogMessage, Exception)
  values (@log_date, @log_thread, @log_level, @log_source, @log_message, @exception)

END

GO
