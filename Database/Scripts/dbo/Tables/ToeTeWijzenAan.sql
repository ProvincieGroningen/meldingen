IF Install.TableExistsYN('dbo', 'ToeTeWijzenAan') = 'N'
BEGIN
  CREATE TABLE dbo.ToeTeWijzenAan
  (
    Id    INT IDENTITY
   ,Naam  VARCHAR( 100 )
    --
   ,CONSTRAINT PK_ToeTeWijzenAan PRIMARY KEY ( Id )
  )
END
