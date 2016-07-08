IF Install.TableExistsYN('dbo', 'MeldingHistorie') = 'N'
BEGIN
  CREATE TABLE dbo.MeldingHistorie
  (
    Id              INT IDENTITY NOT NULL
   ,IdMelding       INT NOT NULL
   ,IdBron          INT NOT NULL
   ,IdStatus        INT NOT NULL
   ,VerzondenOp     SMALLDATETIME NOT NULL
   ,Melder          VARCHAR( 200 ) NOT NULL
   ,Onderwerp       VARCHAR( 200 ) NULL
   ,Latitude        NUMERIC( 38, 20 ) NULL
   ,Longitude       NUMERIC( 38, 20 ) NULL
   ,GewijzigdOp     SMALLDATETIME NOT NULL
   ,GewijzigdDoor   VARCHAR( 100 ) NOT NULL
    --
   ,CONSTRAINT PK_MeldingHistorie PRIMARY KEY ( Id )
  )
END
