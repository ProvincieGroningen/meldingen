EXEC Install.EnsureProcedure 'Install.EnsureWindowsLogin';
GO

ALTER PROC Install.EnsureWindowsLogin
(
  @p_LoginName  SYSNAME
)
AS
BEGIN
  DECLARE @l_SQL VARCHAR( MAX )

  IF NOT EXISTS ( SELECT *
                  FROM master.sys.server_principals
                  WHERE name = @p_LoginName
                    AND type_desc IN ( 'WINDOWS_GROUP', 'WINDOWS_LOGIN' ) )
  BEGIN
    -- Om het wachtwoord te achterhalen:
    -- SELECT CAST( password_hash AS VARBINARY( 256 ) ) FROM master.sys.sql_logins WHERE name = 'loginname'

    SELECT * FROM master.sys.sql_logins
    SELECT @l_SQL = 'CREATE LOGIN [' + @p_LoginName + ']' +
                    '  FROM WINDOWS ' +
                    '  WITH DEFAULT_DATABASE = ' + DB_NAME();

    EXEC( @l_SQL );
  END

  IF NOT EXISTS ( SELECT *
                  FROM sys.database_principals
                  WHERE name = @p_LoginName )
  BEGIN
    SET @l_SQL = 'CREATE USER [' + @p_LoginName + '] FOR LOGIN [' + @p_LoginName + ']';
    EXEC( @l_SQL );
  END

END