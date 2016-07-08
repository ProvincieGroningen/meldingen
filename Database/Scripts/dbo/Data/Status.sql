SET NOCOUNT ON;

WITH StatusData AS
(
            SELECT Id = 1, Naam = 'Ontvangen'
  UNION ALL SELECT 2, 'In behandeling'
  UNION ALL SELECT 100, 'Afgesloten'
)

INSERT INTO dbo.Status
( Id
 ,Naam )
SELECT 
  bd.Id
 ,bd.Naam
FROM      StatusData bd
LEFT JOIN dbo.Status b  ON b.Id = bd.Id
WHERE b.Id IS NULL

DECLARE
  @l_Rowcount INT

SET @l_Rowcount = @@ROWCOUNT
IF @l_Rowcount <>0
  PRINT CONVERT( VARCHAR, @l_Rowcount ) + ' rijen toegevoegd.'
