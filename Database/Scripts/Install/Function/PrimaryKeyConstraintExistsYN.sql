EXEC Install.EnsureScalarFunction 'Install.PrimaryKeyConstraintExistsYN'
GO

ALTER FUNCTION Install.PrimaryKeyConstraintExistsYN
( 
  @p_TableSchema    SYSNAME
 ,@p_TableName      SYSNAME
)
RETURNS CHAR( 1 )
AS
BEGIN
  DECLARE
    @l_Return CHAR( 1 )
   ,@l_ConstraintName SYSNAME;
   
  SET @l_ConstraintName = 'PK_' + @p_TableName;

  IF EXISTS 
    (
      SELECT * 
      FROM sys.KEY_constraints 
      WHERE object_id = OBJECT_ID( @p_TableSchema + '.' + @l_ConstraintName ) 
       AND parent_object_id = OBJECT_ID( @p_TableSchema + '.' + @p_TableName )
       AND type = 'PK'
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