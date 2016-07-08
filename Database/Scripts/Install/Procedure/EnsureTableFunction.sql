EXECUTE Install.EnsureProcedure 'Install.EnsureTableFunction'
GO

ALTER PROCEDURE Install.EnsureTableFunction
(
  @p_FunctionName SYSNAME
)
AS
BEGIN
  IF NOT EXISTS 
    (
      SELECT 1
      FROM sys.objects 
      WHERE object_id = OBJECT_ID( @p_FunctionName ) 
        AND type = 'TF'
    )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE FUNCTION ' + @p_FunctionName + '() RETURNS @t_RETURN TABLE ( DUMMY INT ) AS BEGIN INSERT INTO @t_RETURN SELECT 1 RETURN END';
    EXECUTE sp_executesql @statement = @l_Sql;
  END

END
GO