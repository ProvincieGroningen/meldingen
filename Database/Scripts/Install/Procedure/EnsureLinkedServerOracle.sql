EXEC Install.EnsureProcedure 'Install.EnsureLinkServerOracle'
GO

ALTER PROC Install.EnsureLinkServerOracle
(
  @p_linkedServerName SYSNAME
 ,@p_tnsName          SYSNAME
 ,@p_remoteUsername   SYSNAME
 ,@p_remotePassword   NVARCHAR( 1000 )
)
AS
BEGIN
  DECLARE
    @l_Provider VARCHAR( 200 )
    
  SET @l_Provider = 'OraOLEDB.Oracle'

  IF EXISTS( SELECT *
             FROM sys.servers s
             WHERE s.name = @p_linkedServerName
               AND s.provider = @l_Provider )
  BEGIN
    RETURN
  END

  -- In process moet aan staan voor deze driver  
  EXEC master.dbo.sp_MSset_oledb_prop @l_Provider, 'AllowInProcess', 1
 
  EXEC master.dbo.sp_addlinkedserver
        @server       = @p_linkedServerName
       ,@datasrc      = @p_tnsName
       ,@srvproduct   = 'oracle'
       ,@provider     = @l_Provider
       
  EXEC master.dbo.sp_addlinkedsrvlogin
        @rmtsrvname   = @p_linkedServerName
       ,@rmtuser      = @p_remoteUsername
       ,@rmtpassword  = @p_remotePassword
       ,@useself      = 'False'
       ,@locallogin   = NULL

  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'collation compatible',    'false'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'data access',             'true'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'dist',                    'false'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'pub',                     'false'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'rpc',                     'true'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'rpc out',                 'true'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'sub',                     'false'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'connect timeout',         '0'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'collation name',          NULL
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'lazy schema validation',  'false'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'query timeout',           '0'
  EXEC master.dbo.sp_serveroption @p_linkedServerName, 'use remote collation',    'true'

END