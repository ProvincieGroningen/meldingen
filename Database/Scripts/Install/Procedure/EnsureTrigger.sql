EXEC Install.EnsureProcedure 'Install.EnsureTrigger'
GO

ALTER PROCEDURE Install.EnsureTrigger
(
  @p_TriggerName  SYSNAME
 ,@p_TargetObject SYSNAME
 ,@p_EventName    SYSNAME
)
AS
BEGIN
  IF OBJECT_ID( @p_TargetObject ) IS NULL
    RAISERROR( 'Ongeldige waarde voor @p_TargetObject.', 16, 1 )

  IF NOT EXISTS
  (
    SELECT *
    FROM sys.triggers t
    WHERE t.object_id = OBJECT_ID( @p_TriggerName )
      AND t.type = 'TR'
      AND t.parent_class_desc = 'OBJECT_OR_COLUMN'
  )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE TRIGGER ' + @p_TriggerName 
                + ' ON ' + @p_TargetObject
                + ' ' + @p_EventName
                + ' AS BEGIN DECLARE @l_Dummy INT SET @l_Dummy = 1 END';
    EXECUTE sp_executesql @l_Sql;
  END
END
GO
--
