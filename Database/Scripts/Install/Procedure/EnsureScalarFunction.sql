EXECUTE Install.EnsureProcedure 'Install.EnsureScalarFunction'
GO

ALTER PROCEDURE Install.EnsureScalarFunction
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
        AND type = 'FN'
    )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE FUNCTION ' + @p_FunctionName + '() RETURNS CHAR( 5 ) AS BEGIN RETURN ''Dummy'' END';
    EXECUTE sp_executesql @statement = @l_Sql;
  END

END
GO