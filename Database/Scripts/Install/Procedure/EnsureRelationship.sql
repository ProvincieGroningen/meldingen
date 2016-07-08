EXEC Install.EnsureProcedure 'Install.EnsureRelationship';
GO

ALTER PROC Install.EnsureRelationship
(
  @p_FromTableName SYSNAME
 ,@p_FromFieldName SYSNAME
 ,@p_ToTableName   SYSNAME
 ,@p_ToFieldName   SYSNAME
)
AS
BEGIN

  IF EXISTS
  ( SELECT * 
    FROM sys.foreign_keys f
    WHERE f.parent_object_id = OBJECT_ID( @p_FromTableName )
      AND f.referenced_object_id = OBJECT_ID( @p_ToTableName )
  )
    RETURN
  
  DECLARE
    @l_ConstraintName SYSNAME
   ,@l_Sql            NVARCHAR( MAX )
  
  SET @l_ConstraintName = 'FK_' + REPLACE( @p_FromTableName, '.', '_' )
                        + '_'   + REPLACE( @p_ToTableName, '.', '_' )
                        + '_'   + @p_FromFieldName
                        + '_'   + @p_ToFieldName
  
  SET @l_Sql = 'ALTER TABLE ' + @p_FromTableName + '
                WITH CHECK ADD CONSTRAINT ' + @l_ConstraintName + '
                FOREIGN KEY ( ' + @p_FromFieldName + ' )
                REFERENCES ' + @p_ToTableName + ' (' + @p_ToFieldName + ')'
  EXEC sp_executesql @l_Sql

END
