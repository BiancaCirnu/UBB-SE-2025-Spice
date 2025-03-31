CREATE PROCEDURE GetAllFeatures
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT feature_id, name, value, description, type, source
    FROM Features
    ORDER BY type, value DESC;
END
go