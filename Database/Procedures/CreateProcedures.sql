USE [issEduarda]
GO

CREATE OR ALTER PROCEDURE CreateUserProfile
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO UserProfiles (user_id)
    VALUES (@user_id);

    SELECT 
        profile_id,
        user_id,
        profile_picture,
        bio,
        equipped_frame,
        equipped_hat,
        equipped_pet,
        equipped_emoji,
        last_modified
    FROM UserProfiles
    WHERE profile_id = SCOPE_IDENTITY();
END;
GO

CREATE OR ALTER PROCEDURE GetUserProfileByUserId
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        profile_id,
        user_id,
        profile_picture,
        bio,
        equipped_frame,
        equipped_hat,
        equipped_pet,
        equipped_emoji,
        last_modified
    FROM UserProfiles
    WHERE user_id = @user_id;
END;
GO

CREATE OR ALTER PROCEDURE UpdateUserProfile
    @profile_id INT,
    @user_id INT,
    @profile_picture NVARCHAR(255),
    @bio NVARCHAR(1000),
    @equipped_frame NVARCHAR(255),
    @equipped_hat NVARCHAR(255),
    @equipped_pet NVARCHAR(255),
    @equipped_emoji NVARCHAR(255)
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE UserProfiles
    SET 
        profile_picture = @profile_picture,
        bio = @bio,
        equipped_frame = @equipped_frame,
        equipped_hat = @equipped_hat,
        equipped_pet = @equipped_pet,
        equipped_emoji = @equipped_emoji,
        last_modified = GETDATE()
    WHERE profile_id = @profile_id AND user_id = @user_id;

    SELECT 
        profile_id,
        user_id,
        profile_picture,
        bio,
        equipped_frame,
        equipped_hat,
        equipped_pet,
        equipped_emoji,
        last_modified
    FROM UserProfiles
    WHERE profile_id = @profile_id;
END;
GO

CREATE OR ALTER PROCEDURE GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.user_id,
        u.email,
        u.username,
        up.profile_picture,
        up.bio,
        u.developer,
        u.created_at,
        u.last_login
    FROM Users u
    LEFT JOIN UserProfiles up ON u.user_id = up.user_id
    ORDER BY u.username;
END;
GO

CREATE OR ALTER PROCEDURE GetUserById
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        u.user_id,
        u.email,
        u.username,
        u.developer,
        u.created_at,
        u.last_login,
        up.profile_picture,
        up.bio,
        up.equipped_frame,
        up.equipped_hat,
        up.equipped_pet,
        up.equipped_emoji
    FROM Users u
    LEFT JOIN UserProfiles up ON u.user_id = up.user_id
    WHERE u.user_id = @user_id;
END;
GO

CREATE OR ALTER PROCEDURE GetUserFriends
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.friend_id,
        u.username,
        u.email,
        up.profile_picture,
        up.bio,
        u.developer,
        u.created_at,
        u.last_login
    FROM Friendships f
    JOIN Users u ON f.friend_id = u.user_id
    LEFT JOIN UserProfiles up ON u.user_id = up.user_id
    WHERE f.user_id = @user_id
    ORDER BY u.username;
END;
GO

CREATE OR ALTER PROCEDURE AddFriend
    @user_id INT,
    @friend_id INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Friendships (user_id, friend_id)
    VALUES (@user_id, @friend_id);
END;
GO

CREATE OR ALTER PROCEDURE RemoveFriend
    @friendship_id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Friendships
    WHERE friendship_id = @friendship_id;
END;
GO

CREATE OR ALTER PROCEDURE CheckFriendship
    @user_id INT,
    @friend_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) as friendship_count
    FROM Friendships
    WHERE user_id = @user_id AND friend_id = @friend_id;
END;
GO

CREATE OR ALTER PROCEDURE GetFriendCount
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) as friend_count
    FROM Friendships
    WHERE user_id = @user_id;
END;
GO

CREATE OR ALTER PROCEDURE GetAllFriendships
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.friendship_id,
        f.user_id,
        u1.username as user_username,
        f.friend_id,
        u2.username as friend_username
    FROM Friendships f
    JOIN Users u1 ON f.user_id = u1.user_id
    JOIN Users u2 ON f.friend_id = u2.user_id
    ORDER BY f.user_id, f.friend_id;
END;
GO

CREATE OR ALTER PROCEDURE GetFriendshipsForUser
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        f.friendship_id,
        f.user_id,
        f.friend_id,
        u.username as friend_username,
        up.profile_picture as friend_profile_picture
    FROM Friendships f
    JOIN Users u ON f.friend_id = u.user_id
    LEFT JOIN UserProfiles up ON u.user_id = up.user_id
    WHERE f.user_id = @user_id
    ORDER BY u.username;
END;
GO 