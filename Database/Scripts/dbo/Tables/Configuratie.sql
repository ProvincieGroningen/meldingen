IF Install.TableExistsYN('dbo', 'Configuratie') = 'N'
BEGIN
  CREATE TABLE dbo.Configuratie
  (
    Id    INT IDENTITY
   ,Naam  VARCHAR( 100 )
   ,Waarde NVARCHAR( MAX )
    --
   ,CONSTRAINT PK_Configuratie PRIMARY KEY ( Id )
  )
END
