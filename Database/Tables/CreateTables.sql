USE [issEduarda]
GO

-- Create Users table first (parent table)
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE,
    email NVARCHAR(100) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE,
    hashed_password NVARCHAR(255) NOT NULL,
    developer BIT NOT NULL DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    last_login DATETIME NULL
);
GO

-- Create UserProfiles table
CREATE TABLE UserProfiles (
    profile_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL UNIQUE,
    profile_picture NVARCHAR(255) CHECK (profile_picture LIKE '%.svg' OR profile_picture LIKE '%.png' OR profile_picture LIKE '%.jpg'),
    bio NVARCHAR(1000),
    equipped_frame NVARCHAR(255),
    equipped_hat NVARCHAR(255),
    equipped_pet NVARCHAR(255),
    equipped_emoji NVARCHAR(255),
    last_modified DATETIME NOT NULL DEFAULT GETDATE(),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);
GO

-- Create Achievements table
CREATE TABLE Achievements (
    achievement_id INT PRIMARY KEY IDENTITY(1,1),
    type NVARCHAR(100) NOT NULL,
    points INT NOT NULL CHECK (points >= 0),
    icon NVARCHAR(255) CHECK (icon LIKE '%.svg' OR icon LIKE '%.png' OR icon LIKE '%.jpg')
);
GO

-- Create Features table
CREATE TABLE Features (
    feature_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    value INT NOT NULL CHECK (value >= 0),
    description NVARCHAR(255),
    type NVARCHAR(50) CHECK (type IN ('frame', 'emoji', 'background', 'pet', 'hat')),
    source NVARCHAR(255) CHECK (source LIKE '%.svg' OR source LIKE '%.png' OR source LIKE '%.jpg')
);
GO

-- Create Collections table
CREATE TABLE Collections (
    collection_id INT PRIMARY KEY IDENTITY(1,1),
    name NVARCHAR(100) NOT NULL,
    picture NVARCHAR(255) CHECK (picture LIKE '%.svg' OR picture LIKE '%.png' OR picture LIKE '%.jpg'),
    description NVARCHAR(100),
    is_public BIT DEFAULT 1
);
GO

-- Create Wallet table
CREATE TABLE Wallet (
    wallet_id INT PRIMARY KEY IDENTITY(1,1),
    points INT NOT NULL DEFAULT 0,
    achievement_points INT NOT NULL DEFAULT 0,
    game_points INT NOT NULL DEFAULT 0,
    money_for_games DECIMAL(10,2) NOT NULL DEFAULT 0.00
);
GO

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
GO

-- Create User_Wallet table
CREATE TABLE User_Wallet (
    user_id INT NOT NULL,
    wallet_id INT NOT NULL,
    PRIMARY KEY (user_id, wallet_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (wallet_id) REFERENCES Wallet(wallet_id) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- Create OwnedGames_Collection table
CREATE TABLE OwnedGames_Collection (
    collection_id INT NOT NULL,
    game_id INT NOT NULL,
    date_added DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (collection_id, game_id),
    FOREIGN KEY (collection_id) REFERENCES Collections(collection_id) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- Create User_Achievement table
CREATE TABLE User_Achievement (
    user_id INT NOT NULL,
    achievement_id INT NOT NULL,
    date_unlocked DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (user_id, achievement_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- Create Feature_User table
CREATE TABLE Feature_User (
    user_id INT NOT NULL,
    feature_id INT NOT NULL,
    equipped BIT DEFAULT 0,
    PRIMARY KEY (user_id, feature_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (feature_id) REFERENCES Features(feature_id) ON DELETE CASCADE ON UPDATE CASCADE
);
GO

-- Create indexes for Friendships table
CREATE INDEX IX_Friendships_UserId ON Friendships(user_id);
CREATE INDEX IX_Friendships_FriendId ON Friendships(friend_id);
GO 