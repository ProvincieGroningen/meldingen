IF Install.TableExistsYN('dbo', 'Opmerking') = 'N'
BEGIN
  CREATE TABLE dbo.Opmerking
  (
    Id              INT IDENTITY NOT NULL
   ,IdMelding       INT NOT NULL
   ,Tekst           VARCHAR( MAX ) NULL
   ,AangemaaktOp    SMALLDATETIME NOT NULL
   ,AangemaaktDoor  VARCHAR( 100 ) NOT NULL
    --
   ,CONSTRAINT PK_Opmerking PRIMARY KEY ( Id )
  )
END