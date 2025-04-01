go
CREATE OR ALTER PROCEDURE GetFriendsForUser
    @user_id INT
AS
BEGIN
    SELECT 
        f.friendship_id,
        f.user_id,
        f.friend_id,
        u.username as friend_username,
        p.profile_picture as friend_profile_picture
    FROM Friendships f
    JOIN Users u ON f.friend_id = u.user_id
	JOIN UserProfiles p ON p.user_id = f.friend_id
    WHERE f.user_id = @user_id
    ORDER BY u.username;
END
GO 
