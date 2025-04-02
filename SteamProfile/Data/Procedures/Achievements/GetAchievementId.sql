CREATE or Alter PROCEDURE GetAchievementId
	@type NVARCHAR(50),
	@count int
AS
BEGIN
	IF @type = 'Friendships'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP4';
		ELSE IF @count = 100 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP5';
	END
	ELSE IF @type = 'Owned Games'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES3';		
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES4';
	END
	ELSE IF @type = 'Sold Games'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES4';
	END
	ELSE IF @type = 'Number of Reviews'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW4';
	END
END

