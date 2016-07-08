EXEC Install.EnsureTrigger 'Melding_AfterUpdate', 'dbo.Melding', 'AFTER UPDATE'
GO

ALTER TRIGGER Melding_AfterUpdate
  ON dbo.Melding
  AFTER UPDATE
AS
BEGIN

  INSERT INTO dbo.MeldingHistorie
  (
    IdMelding
   ,IdBron
   ,IdStatus
   ,VerzondenOp
   ,Melder
   ,Onderwerp
   ,Latitude
   ,Longitude
   ,GewijzigdOp
   ,GewijzigdDoor
  )
  SELECT
    Id
   ,IdBron
   ,IdStatus
   ,VerzondenOp
   ,Melder
   ,Onderwerp
   ,Latitude
   ,Longitude
   ,GewijzigdOp
   ,GewijzigdDoor
  FROM deleted;
    
END