USE [issEduarda]
GO

-- Insert Users with hashed passwords
INSERT INTO Users (email, username, hashed_password, developer, created_at, last_login)
VALUES 
    ('john.doe@example.com', 'JohnDoe', 'hashed_password_123', 0, GETDATE(), GETDATE()),
    ('jane.smith@example.com', 'JaneSmith', 'hashed_password_456', 1, GETDATE(), GETDATE()),
    ('bob.wilson@example.com', 'BobWilson', 'hashed_password_789', 0, GETDATE(), GETDATE()),
    ('alice.brown@example.com', 'AliceBrown', 'hashed_password_abc', 1, GETDATE(), GETDATE()),
    ('charlie.davis@example.com', 'CharlieDavis', 'hashed_password_def', 0, GETDATE(), GETDATE())
GO

-- Insert UserProfiles
INSERT INTO UserProfiles (user_id, profile_picture, bio, equipped_frame, equipped_hat, equipped_pet, equipped_emoji)
VALUES 
    (1, 'ms-appx:///Assets/100_achievement.jpeg', 'Gaming enthusiast', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg'),
    (2, 'ms-appx:///Assets/100_achievement.jpeg', 'Game developer', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg'),
    (3, 'ms-appx:///Assets/100_achievement.jpeg', 'Casual gamer', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg'),
    (4, 'ms-appx:///Assets/100_achievement.jpeg', 'Indie developer', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg'),
    (5, 'ms-appx:///Assets/100_achievement.jpeg', 'Game collector', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg', 'ms-appx:///Assets/100_achievement.jpeg')
GO

-- Insert Friendships
INSERT INTO Friendships (user_id, friend_id, status, created_at)
VALUES 
    (1, 2, 'accepted', GETDATE()),  -- John and Jane are friends
    (1, 3, 'accepted', GETDATE()),  -- John and Bob are friends
    (2, 3, 'pending', GETDATE()),   -- Jane sent friend request to Bob
    (4, 5, 'accepted', GETDATE()),  -- Alice and Charlie are friends
    (1, 4, 'pending', GETDATE())    -- John sent friend request to Alice
GO 