EXEC Install.EnsureScalarFunction 'Install.IndexExistsYN'
GO

ALTER FUNCTION Install.IndexExistsYN
( 
  @p_TableSchema  SYSNAME
 ,@p_TableName    SYSNAME
 ,@p_IndexName    SYSNAME
)
RETURNS CHAR( 1 )
AS
BEGIN
  DECLARE
    @l_Return CHAR( 1 );
    
  IF EXISTS
    (
      SELECT
        1
      FROM sys.indexes 
      WHERE object_id = OBJECT_ID( @p_TableSchema + '.' + @p_TableName ) 
        AND name = @p_IndexName
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