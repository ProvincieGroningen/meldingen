IF Install.TableExistsYN('dbo', 'Bron') = 'N'
BEGIN
  CREATE TABLE dbo.Bron
  (
    Id    INT
   ,Naam  VARCHAR( 100 )
    --
   ,CONSTRAINT PK_Bron PRIMARY KEY ( Id )
  )
END