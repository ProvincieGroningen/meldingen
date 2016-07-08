SET NOCOUNT ON;

WITH BronData AS
(
            SELECT Id = 1, Naam = 'E-mail'
  UNION ALL SELECT 2, 'Applicatie Meldingen'
)

INSERT INTO dbo.Bron
( Id
 ,Naam )
SELECT 
  bd.Id
 ,bd.Naam
FROM      BronData bd
LEFT JOIN dbo.Bron b  ON b.Id = bd.Id
WHERE b.Id IS NULL

DECLARE
  @l_Rowcount INT

SET @l_Rowcount = @@ROWCOUNT
IF @l_Rowcount <>0
  PRINT CONVERT( VARCHAR, @l_Rowcount ) + ' rijen toegevoegd.'
