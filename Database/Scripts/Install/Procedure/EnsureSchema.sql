EXECUTE Install.EnsureProcedure 'Install.EnsureSchema'
GO

ALTER PROCEDURE Install.EnsureSchema
(
  @p_SchemaName SYSNAME
)
AS
BEGIN
  IF NOT EXISTS 
    (
      SELECT *
      FROM sys.schemas
      WHERE name = @p_SchemaName
    )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE SCHEMA ' + @p_SchemaName;
    
    EXECUTE sp_executesql @statement = @l_Sql;
  END

END
GO