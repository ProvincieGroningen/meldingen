EXEC Install.EnsureProcedure 'Install.EnsureSqlLogin';
GO

ALTER PROC Install.EnsureSqlLogin
(
  @p_LoginName VARCHAR( 100 )
 ,@p_Password  VARBINARY( 256 )
)
AS
BEGIN
  DECLARE @l_SQL VARCHAR( MAX )

  IF NOT EXISTS ( SELECT *
                  FROM master.sys.sql_logins
                  WHERE name = @p_LoginName
                    AND type_desc = 'SQL_LOGIN' )
  BEGIN
    -- Om het wachtwoord te achterhalen:
    -- SELECT CAST( password_hash AS VARBINARY( 256 ) ) FROM master.sys.sql_logins WHERE name = 'loginname'

    SELECT * FROM master.sys.sql_logins
    SELECT @l_SQL = 'CREATE LOGIN ' + @p_LoginName + 
                    '  WITH PASSWORD = ' + sys.fn_varbintohexstr( @p_Password ) + ' HASHED ' +
                    '      ,DEFAULT_DATABASE = ' + DB_NAME();
    print @l_sql;
    EXEC( @l_SQL );
  END

  IF NOT EXISTS ( SELECT *
                  FROM sys.database_principals
                  WHERE name = @p_LoginName
                    AND type_desc = 'SQL_USER' )
  BEGIN
    SELECT @l_SQL = 'CREATE USER ' + @p_LoginName + ' FOR LOGIN ' + @p_LoginName;
    print @l_sql;
    EXEC( @l_SQL );
  END

END