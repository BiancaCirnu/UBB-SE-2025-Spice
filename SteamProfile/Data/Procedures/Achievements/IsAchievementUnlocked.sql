CREATE PROCEDURE IsAchievementUnlocked
	@user_id INT,
	@achievement_id INT
AS
BEGIN
	IF EXISTS (
        SELECT 1 FROM UserAchievements 
        WHERE user_id = @user_id 
        AND achievement_id = @achievement_id
    )
        SELECT 1;
	ELSE 
		SELECT 0;
END;