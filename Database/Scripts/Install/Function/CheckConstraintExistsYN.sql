EXEC Install.EnsureScalarFunction 'Install.CheckConstraintExistsYN'
GO

ALTER FUNCTION Install.CheckConstraintExistsYN
( 
  @p_TableSchema    SYSNAME
 ,@p_TableName      SYSNAME
 ,@p_ConstraintName SYSNAME
)
RETURNS CHAR( 1 )
AS
BEGIN
  DECLARE
    @l_Return CHAR( 1 );

  IF EXISTS 
    (
      SELECT * 
      FROM sys.check_constraints 
      WHERE object_id = OBJECT_ID( @p_TableSchema + '.' + @p_ConstraintName ) 
       AND parent_object_id = OBJECT_ID( @p_TableSchema + '.' + @p_TableName )
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