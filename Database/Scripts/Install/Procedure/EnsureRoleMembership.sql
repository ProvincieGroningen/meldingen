EXEC Install.EnsureProcedure 'Install.EnsureRoleMembership';
GO

ALTER PROC Install.EnsureRoleMembership
(
  @p_LoginName  SYSNAME
 ,@p_RoleName   SYSNAME
)
AS
BEGIN
  DECLARE
    @l_SQL VARCHAR( MAX )

  IF NOT EXISTS ( SELECT
                    1
                  FROM       sys.database_role_members  m
                  INNER JOIN sys.database_principals    mp ON mp.principal_id = m.member_principal_id
                  INNER JOIN sys.database_principals    rp ON rp.principal_id = m.role_principal_id
                  WHERE mp.name = @p_RoleName
                    AND rp.name = @p_LoginName )
  BEGIN
    EXEC sp_addrolemember @p_RoleName, @p_LoginName
  END

END