EXECUTE Install.EnsureProcedure 'Install.EnsureRole'
GO

ALTER PROCEDURE Install.EnsureRole
(
  @p_RoleName SYSNAME
)
AS
BEGIN
  IF NOT EXISTS 
    (
      SELECT 1
      FROM sys.database_principals
      WHERE name = @p_RoleName
        AND type = 'R'
    )
  BEGIN
    DECLARE
      @l_Sql NVARCHAR( MAX )
    SET @l_Sql = N'CREATE ROLE ' + @p_RoleName;
    EXECUTE sp_executesql @statement = @l_Sql;
  END

END
GO