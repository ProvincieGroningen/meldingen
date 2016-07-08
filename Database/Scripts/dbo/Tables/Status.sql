IF Install.TableExistsYN('dbo', 'Status') = 'N'
BEGIN
  CREATE TABLE dbo.Status
  (
    Id    INT
   ,Naam  VARCHAR( 100 )
    --
   ,CONSTRAINT PK_Status PRIMARY KEY ( Id )
  )
END