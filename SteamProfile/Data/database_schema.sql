-- Drop tables in reverse order of dependencies
DROP TABLE IF EXISTS Feature_User;
DROP TABLE IF EXISTS User_Achievement;
DROP TABLE IF EXISTS OwnedGames_Collection;
DROP TABLE IF EXISTS User_Wallet;
DROP TABLE IF EXISTS Friendships;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS Collections;
DROP TABLE IF EXISTS Features;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS Users;

-- Drop stored procedures
DROP PROCEDURE IF EXISTS GetAllUsers;
DROP PROCEDURE IF EXISTS GetUserById;
DROP PROCEDURE IF EXISTS GetUserFriends;
DROP PROCEDURE IF EXISTS AddFriend;
DROP PROCEDURE IF EXISTS RemoveFriend;
DROP PROCEDURE IF EXISTS CheckFriendship;
DROP PROCEDURE IF EXISTS GetFriendCount;
DROP PROCEDURE IF EXISTS GetAllFriendships;
DROP PROCEDURE IF EXISTS GetFriendshipsForUser;
DROP PROCEDURE IF EXISTS GetFriendshipCountForUser;

-- User Table
CREATE TABLE Users (
    user_id INT PRIMARY KEY identity(1,1),
    email NVARCHAR(255) UNIQUE NOT NULL CHECK (email LIKE '%@%._%'),
    username NVARCHAR(100) UNIQUE NOT NULL,
    password_hash NVARCHAR(255) NOT NULL,
    profile_picture NVARCHAR(255) CHECK (profile_picture LIKE '%.svg' OR profile_picture LIKE '%.png' OR profile_picture LIKE '%.jpg'),
    description NVARCHAR(1000),
    developer BIT DEFAULT 0,
    created_at DATETIME DEFAULT GETDATE(),
    last_login DATETIME NULL
);

-- Create GetAllUsers stored procedure
GO
CREATE OR ALTER PROCEDURE GetAllUsers
AS
BEGIN
    SELECT user_id, email, username, profile_picture, description, developer, created_at, last_login
    FROM Users
    ORDER BY username;
END
GO

-- Create GetUserById stored procedure
GO
CREATE OR ALTER PROCEDURE GetUserById
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        user_id,
        email,
        username,
        password_hash,
        profile_picture,
        description,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = @user_id;
END
GO

-- Create GetUserFriends stored procedure
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
        u.profile_picture,
        u.description,
        u.developer,
        u.created_at,
        u.last_login
    FROM Friendships f
    JOIN Users u ON f.friend_id = u.user_id
    WHERE f.user_id = @user_id
    ORDER BY u.username;
END
GO

-- Create AddFriend stored procedure
GO
CREATE OR ALTER PROCEDURE AddFriend
    @user_id INT,
    @friend_id INT
AS
BEGIN
    SET NOCOUNT ON;

    INSERT INTO Friendships (user_id, friend_id)
    VALUES (@user_id, @friend_id);
END
GO

-- Create RemoveFriend stored procedure
GO
CREATE OR ALTER PROCEDURE RemoveFriend
    @user_id INT,
    @friend_id INT
AS
BEGIN
    SET NOCOUNT ON;

    DELETE FROM Friendships
    WHERE user_id = @user_id AND friend_id = @friend_id;
END
GO

-- Create CheckFriendship stored procedure
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
END
GO

-- Create GetFriendCount stored procedure
GO
CREATE OR ALTER PROCEDURE GetFriendCount
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) as friend_count
    FROM Friendships
    WHERE user_id = @user_id;
END
GO

-- Achievements Table
CREATE TABLE Achievements (
    achievement_id INT PRIMARY KEY identity(1,1),
    type NVARCHAR(100) NOT NULL,
    points INT NOT NULL CHECK (points >= 0),
    icon NVARCHAR(255) CHECK (icon LIKE '%.svg' OR icon LIKE '%.png' OR icon LIKE '%.jpg' OR icon LIKE '%.jpg')
);

-- Features Table
CREATE TABLE Features (
    feature_id INT PRIMARY KEY identity(1,1),
    name NVARCHAR(100) NOT NULL,
    value INT NOT NULL CHECK (value >= 0),
    description NVARCHAR(255),
    type NVARCHAR(50) CHECK (type IN ('frame', 'emoji', 'background', 'pet', 'hat')),
    source NVARCHAR(255) CHECK (source LIKE '%.svg' OR source LIKE '%.png' OR source LIKE '%.jpg')
);

-- Collections Table
CREATE TABLE Collections (
    collection_id INT PRIMARY KEY identity(1,1),
    name NVARCHAR(100) NOT NULL,
    picture NVARCHAR(255) CHECK (picture LIKE '%.svg' OR picture LIKE '%.png' OR picture LIKE '%.jpg'),
    description NVARCHAR(100),
    is_public BIT DEFAULT 1
);

-- Wallet Table
CREATE TABLE Wallet (
    wallet_id INT PRIMARY KEY identity(1,1),
    points INT NOT NULL DEFAULT 0,
    achievement_points INT NOT NULL DEFAULT 0,
    game_points INT NOT NULL DEFAULT 0,
    money_for_games DECIMAL(10,2) NOT NULL DEFAULT 0.00
);

-- User_Wallet Connection Table
CREATE TABLE User_Wallet (
    user_id INT NOT NULL,
    wallet_id INT NOT NULL,
    PRIMARY KEY (user_id, wallet_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (wallet_id) REFERENCES Wallet(wallet_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- OwnedGames_Collection Table
CREATE TABLE OwnedGames_Collection (
    collection_id INT NOT NULL,
    game_id INT NOT NULL,
    date_added DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (collection_id, game_id),
    FOREIGN KEY (collection_id) REFERENCES Collections(collection_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- User_Achievement Table
CREATE TABLE User_Achievement (
    user_id INT NOT NULL,
    achievement_id INT NOT NULL,
    date_unlocked DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (user_id, achievement_id),
    FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id) ON DELETE CASCADE ON UPDATE CASCADE
);

-- Feature_User Table
CREATE TABLE Feature_User (
    user_id INT NOT NULL,
    feature_id INT NOT NULL,
    equipped BIT DEFAULT 0,
    PRIMARY KEY (user_id, feature_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (feature_id) REFERENCES Features(feature_id) ON DELETE CASCADE ON UPDATE CASCADE
);

INSERT INTO Users (email, username, password_hash, profile_picture, description, developer, last_login) VALUES
('alice@example.com', 'AliceGamer', 'hashed_password_1', 'alice.png', 'Passionate gamer and developer.', 1, '2025-03-20 14:25:00'),
('bob@example.com', 'BobTheBuilder', 'hashed_password_2', 'bob.jpg', 'Strategy game enthusiast.', 0, '2025-03-21 10:12:00'),
('charlie@example.com', 'CharlieX', 'hashed_password_3', 'charlie.svg', 'Loves open-world RPGs.', 0, '2025-03-22 18:45:00'),
('diana@example.com', 'DianaRocks', 'hashed_password_4', 'diana.png', 'Competitive FPS player.', 0, '2025-03-19 22:30:00'),
('eve@example.com', 'Eve99', 'hashed_password_5', 'eve.jpg', 'Indie game developer.', 1, '2025-03-23 08:05:00'),
('frank@example.com', 'FrankTheTank', 'hashed_password_6', 'frank.svg', 'MOBA and strategy geek.', 0, '2025-03-24 16:20:00'),
('grace@example.com', 'GraceSpeed', 'hashed_password_7', 'grace.png', 'Speedrunner and puzzle solver.', 0, '2025-03-25 11:40:00'),
('harry@example.com', 'HarryWizard', 'hashed_password_8', 'harry.jpg', 'Lover of fantasy games.', 0, '2025-03-20 20:15:00'),
('ivy@example.com', 'IvyNinja', 'hashed_password_9', 'ivy.svg', 'Stealth and action-adventure expert.', 0, '2025-03-22 09:30:00'),
('jack@example.com', 'JackHacks', 'hashed_password_10', 'jack.png', 'Cybersecurity and hacking sim fan.', 1, '2025-03-24 23:55:00');

select * from Users;

SELECT COUNT(*) FROM Users;

-- Create Friendships table
CREATE TABLE Friendships (
    friendship_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    friend_id INT NOT NULL,
    CONSTRAINT FK_Friendships_User FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT FK_Friendships_Friend FOREIGN KEY (friend_id) REFERENCES Users(user_id),
    CONSTRAINT UQ_Friendship UNIQUE (user_id, friend_id),
    CONSTRAINT CHK_FriendshipUsers CHECK (user_id != friend_id)
);

-- Add indexes for better query performance
CREATE INDEX IX_Friendships_UserId ON Friendships(user_id);
CREATE INDEX IX_Friendships_FriendId ON Friendships(friend_id);

-- Add some mock data for testing
INSERT INTO Friendships (user_id, friend_id)
VALUES 
    (1, 2),
    (1, 3),
    (2, 3),
    (2, 4);
GO

CREATE OR ALTER PROCEDURE GetAllFriendships
AS
BEGIN
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
END
GO

GO
CREATE OR ALTER PROCEDURE GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

    SELECT 
        user_id,
        email,
        username,
        password_hash,
        profile_picture,
        description,
        developer,
        created_at,
        last_login
    FROM Users
    ORDER BY username;
END

-- Create GetFriendshipsForUser stored procedure
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
        u.profile_picture as friend_profile_picture
    FROM Friendships f
    JOIN Users u ON f.friend_id = u.user_id
    WHERE f.user_id = @user_id
    ORDER BY u.username;
END
GO

-- Create GetFriendshipCountForUser stored procedure
GO
CREATE OR ALTER PROCEDURE GetFriendshipCountForUser
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    SELECT COUNT(*) as friend_count
    FROM Friendships
    WHERE user_id = @user_id;
END
GO

