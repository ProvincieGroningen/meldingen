-- Relaties
EXEC Install.EnsureRelationship 'dbo.Bijlage', 'IdMelding', 'dbo.Melding', 'Id'


IF Install.CheckConstraintExistsYN( 'dbo', 'Bijlage', 'CC_Bijlage_PrimaireFoto') = 'N'
BEGIN
   ALTER TABLE Bijlage
   ADD CONSTRAINT CC_Bijlage_PrimaireFoto CHECK (dbo.BijlageCheckPrimareBijlage( IdMelding ) = 1)
END