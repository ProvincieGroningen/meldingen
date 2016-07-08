EXEC Install.EnsureScalarFunction 'dbo.BijlageCheckPrimareBijlage'
GO

ALTER FUNCTION dbo.BijlageCheckPrimareBijlage( @p_IdMelding INT )
RETURNS BIT
AS
BEGIN

  DECLARE
    @l_Return BIT
  
  SET @l_Return = 1

  IF ( SELECT
         COUNT( * )
       FROM dbo.Bijlage
       WHERE IdMelding = @p_IdMelding
         AND IsPrimaireFoto  = 1 ) > 1
    SET @l_Return = 0

  RETURN @l_Return
END
GO

