IF NOT EXISTS ( SELECT 1
                FROM dbo.sysusers
                WHERE name = 'Meldingen_WebRole'
                  AND issqlrole = 1 )
BEGIN
  EXEC sp_addrole 'Meldingen_WebRole'
END