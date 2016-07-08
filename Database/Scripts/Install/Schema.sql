DECLARE
  @p_SchemaName SYSNAME

SET @p_SchemaName = 'Install'
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

