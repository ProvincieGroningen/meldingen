EXECUTE Install.EnsureProcedure 'Install.EnsureView';
GO

ALTER PROCEDURE Install.EnsureView
(
  @p_ViewName SYSNAME
)
AS
BEGIN
  IF NOT EXISTS
  (
    SELECT 1
    FROM sys.views p
    WHERE p.object_id = OBJECT_ID( @p_ViewName )
      AND p.type = 'V'
  )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE VIEW ' + @p_ViewName + ' AS SELECT Dummy = 1';
    EXECUTE sp_executesql @l_Sql;
  END
END
GO

