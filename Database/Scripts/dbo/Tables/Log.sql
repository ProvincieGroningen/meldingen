IF Install.TableExistsYN('dbo', 'Log') = 'N'
BEGIN
  CREATE TABLE dbo.Log
  (
    Id              INT IDENTITY NOT NULL
   ,AangemaaktOp    SMALLDATETIME NOT NULL
   ,Soort           VARCHAR( 50 ) NOT NULL
   ,Omschrijving    VARCHAR( 4000 ) NOT NULL
    --
   ,CONSTRAINT PK_Log PRIMARY KEY ( Id )
  )
END
