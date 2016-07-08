IF NOT EXISTS
(
  SELECT 1
  FROM sys.procedures p
  WHERE p.object_id = OBJECT_ID( 'Install.EnsureProcedure' )
    AND p.type = 'P'
)
BEGIN
  DECLARE
    @l_Sql NVARCHAR( MAX )
  SET @l_Sql = N'CREATE PROCEDURE Install.EnsureProcedure AS SELECT Dummy = 1';
  EXECUTE sp_executesql @l_Sql;
END
GO

ALTER PROCEDURE Install.EnsureProcedure
(
  @p_ProcedureName SYSNAME
)
AS
BEGIN
  IF NOT EXISTS
  (
    SELECT 1
    FROM sys.procedures p
    WHERE p.object_id = OBJECT_ID( @p_ProcedureName )
      AND p.type = 'P'
  )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE PROCEDURE ' + @p_ProcedureName + ' AS SELECT Dummy = 1';
    EXECUTE sp_executesql @l_Sql;
  END
END
GO
-- Install.EnsureProcedure 'Install.EnsureProcedure'
