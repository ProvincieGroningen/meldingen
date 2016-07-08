IF Install.TableExistsYN('dbo', 'Melding') = 'N'
BEGIN
  CREATE TABLE dbo.Melding
  (
    Id              INT IDENTITY NOT NULL
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
   ,CONSTRAINT PK_Melding PRIMARY KEY ( Id )
  )
END

IF Install.ColumnExistsYN( 'dbo', 'Melding', 'ToegewezenAan' ) = 'N'
	ALTER TABLE Melding ADD ToegewezenAan NVARCHAR( 1000 )


IF Install.ColumnExistsYN( 'dbo', 'Melding', 'DatumToegewezenAanGewijzigd' ) = 'N'
	ALTER TABLE Melding ADD DatumToegewezenAanGewijzigd DATE
