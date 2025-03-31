CREATE PROCEDURE UserLogin
    @EmailOrUsername NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashedPassword NVARCHAR(255);
    DECLARE @UserId INT;

    -- Get the user details using the new procedure
    EXEC GetUserByEmailOrUsername @EmailOrUsername;

    -- Check if the user exists and get the hashed password
    SELECT @HashedPassword = hashed_password, @UserId = user_id
    FROM Users
    WHERE username = @EmailOrUsername OR email = @EmailOrUsername;

    -- Check if the user exists and the password matches
    IF @UserId IS NOT NULL AND BCrypt.Net.BCrypt.Verify(@Password, @HashedPassword)
    BEGIN
        -- Return user data
        SELECT user_id, username, email, developer, created_at, last_login
        FROM Users
        WHERE user_id = @UserId;
    END
    ELSE
    BEGIN
        -- Return NULL if login failed
        SELECT NULL AS user_id,
               NULL AS username,
               NULL AS email,
               NULL AS developer,
               NULL AS created_at,
               NULL AS last_login;
    END
END 

-- Insert Users with a placeholder hashed password
INSERT INTO Users (email, username, hashed_password, developer, created_at, last_login)
VALUES 
    ('john.doe@example.com', 'JohnDoe', 'hashedpassword123', 0, GETDATE(), GETDATE()),
    ('jane.smith@example.com', 'JaneSmith', 'hashedpassword123', 1, GETDATE(), GETDATE()),
    ('mike.wilson@example.com', 'MikeWilson', 'hashedpassword123', 0, GETDATE(), GETDATE()),
    ('sarah.jones@example.com', 'SarahJones', 'hashedpassword123', 1, GETDATE(), GETDATE()),
    ('alex.brown@example.com', 'AlexBrown', 'hashedpassword123', 0, GETDATE(), GETDATE());

-- Insert UserProfiles
INSERT INTO UserProfiles (user_id, profile_picture, bio, equipped_frame, equipped_hat, equipped_pet, equipped_emoji, last_modified)
VALUES 
    (1, 'ms-appx:///Assets/ProfilePictures/default.png', 'Gaming enthusiast and software developer', 'ms-appx:///Assets/Frames/gold_frame.png', 'ms-appx:///Assets/Hats/crown.png', 'ms-appx:///Assets/Pets/dog.png', 'ms-appx:///Assets/Emojis/smile.png', GETDATE()),
    (2, 'ms-appx:///Assets/ProfilePictures/default.png', 'Game developer and tech lover', 'ms-appx:///Assets/Frames/silver_frame.png', 'ms-appx:///Assets/Hats/cap.png', 'ms-appx:///Assets/Pets/cat.png', 'ms-appx:///Assets/Emojis/star.png', GETDATE()),
    (3, 'ms-appx:///Assets/ProfilePictures/default.png', 'Casual gamer and streamer', 'ms-appx:///Assets/Frames/bronze_frame.png', 'ms-appx:///Assets/Hats/helmet.png', 'ms-appx:///Assets/Pets/bird.png', 'ms-appx:///Assets/Emojis/heart.png', GETDATE()),
    (4, 'ms-appx:///Assets/ProfilePictures/default.png', 'Game designer and artist', 'ms-appx:///Assets/Frames/platinum_frame.png', 'ms-appx:///Assets/Hats/beret.png', 'ms-appx:///Assets/Pets/rabbit.png', 'ms-appx:///Assets/Emojis/rocket.png', GETDATE()),
    (5, 'ms-appx:///Assets/ProfilePictures/default.png', 'Gaming community manager', 'ms-appx:///Assets/Frames/diamond_frame.png', 'ms-appx:///Assets/Hats/headphones.png', 'ms-appx:///Assets/Pets/hamster.png', 'ms-appx:///Assets/Emojis/fire.png', GETDATE());
GO

-- Insert Friendships
INSERT INTO Friendships (user_id, friend_id)
VALUES 
    (1, 2),  -- John is friends with Jane
    (1, 3),  -- John is friends with Mike
    (1, 4),  -- John is friends with Sarah
    (2, 1),  -- Jane is friends with John
    (2, 3),  -- Jane is friends with Mike
    (2, 5),  -- Jane is friends with Alex
    (3, 1),  -- Mike is friends with John
    (3, 2),  -- Mike is friends with Jane
    (3, 4),  -- Mike is friends with Sarah
    (4, 1),  -- Sarah is friends with John
    (4, 3),  -- Sarah is friends with Mike
    (4, 5),  -- Sarah is friends with Alex
    (5, 2),  -- Alex is friends with Jane
    (5, 4);  -- Alex is friends with Sarah
GO 

