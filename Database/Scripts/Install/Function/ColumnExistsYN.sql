EXEC Install.EnsureScalarFunction 'Install.ColumnExistsYN'
GO

ALTER FUNCTION Install.ColumnExistsYN
( 
  @p_TableSchema SYSNAME
 ,@p_TableName   SYSNAME
 ,@p_ColumnName  SYSNAME
)
RETURNS CHAR( 1 )
AS
BEGIN
  DECLARE
    @l_Return CHAR( 1 );
    
  IF EXISTS 
    (
      SELECT *
      FROM sys.tables t
      JOIN sys.columns c ON t.object_id = c.object_id
      JOIN sys.schemas s on s.schema_id = t.schema_id
      WHERE s.name = @p_TableSchema
        AND t.name = @p_TableName
        AND c.name = @p_ColumnName
    )
  BEGIN
    SET @l_Return = 'Y';
  END
  ELSE
  BEGIN
    SET @l_Return = 'N';
  END
  RETURN @l_Return;
END
