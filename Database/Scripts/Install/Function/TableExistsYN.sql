EXEC Install.EnsureScalarFunction 'Install.TableExistsYN'
GO

ALTER FUNCTION Install.TableExistsYN
( 
  @p_TableSchema SYSNAME
 ,@p_TableName   SYSNAME
)
RETURNS CHAR( 1 )
AS
BEGIN
  DECLARE
    @l_Return CHAR( 1 );
    
  IF EXISTS 
    (
      SELECT *
      FROM INFORMATION_SCHEMA.TABLES
      WHERE TABLE_SCHEMA = @p_TableSchema
        AND TABLE_NAME = @p_TableName
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