IF Install.TableExistsYN('dbo', 'Bijlage') = 'N'
BEGIN
  CREATE TABLE dbo.Bijlage
  (
    Id              INT IDENTITY NOT NULL
   ,IdMelding       INT NOT NULL
   ,Naam            VARCHAR( 200 ) NOT NULL
   ,BestandsNaam    VARCHAR( 200 ) NOT NULL
   ,Inhoud          VARBINARY( MAX ) NOT NULL
   ,ThumbnailSmall  VARBINARY( MAX ) NULL
   ,ThumbnailBig    VARBINARY( MAX ) NULL
   ,Mimetype        VARCHAR( 200 ) NOT NULL
   ,IsPrimaireFoto  BIT NOT NULL
   ,AangemaaktOp    SMALLDATETIME NOT NULL
   ,AangemaaktDoor  VARCHAR( 100 ) NOT NULL
    --
   ,CONSTRAINT PK_Bijlage PRIMARY KEY ( Id )
  )
END