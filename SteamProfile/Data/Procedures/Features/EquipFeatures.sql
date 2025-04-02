CREATE PROCEDURE EquipFeature
    @userId INT,
    @featureId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if the feature-user relationship exists
    IF EXISTS (SELECT 1 FROM Feature_User WHERE user_id = @userId AND feature_id = @featureId)
    BEGIN
        -- Update existing relationship
        UPDATE Feature_User
        SET equipped = 1
        WHERE user_id = @userId AND feature_id = @featureId;
    END
    ELSE
    BEGIN
        -- Create new relationship
        INSERT INTO Feature_User (user_id, feature_id, equipped)
        VALUES (@userId, @featureId, 1);
    END
    
    RETURN 1;
END