-- Drop existing objects if they exist
:r Data\DropExistingObjects.sql

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Users')
BEGIN
    CREATE TABLE Users (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
        username NVARCHAR(50) NOT NULL UNIQUE,
        email NVARCHAR(100) NOT NULL UNIQUE,
        hashed_password NVARCHAR(255) NOT NULL,
        developer BIT NOT NULL DEFAULT 0,
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        last_login DATETIME NULL
    );
END

-- Create Achievements table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Achievements')
BEGIN
    CREATE TABLE Achievements (
        achievement_id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
        description NVARCHAR(MAX),
        icon_url NVARCHAR(MAX),
        created_at DATETIME NOT NULL DEFAULT GETDATE()
    );
END

-- Create UserAchievements table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'UserAchievements')
BEGIN
    CREATE TABLE UserAchievements (
        user_id INT NOT NULL,
        achievement_id INT NOT NULL,
        unlocked_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, achievement_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id)
    );
END

-- Create Collections table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Collections')
BEGIN
    CREATE TABLE Collections (
        collection_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        name NVARCHAR(100) NOT NULL,
        cover_picture NVARCHAR(MAX),
        is_public BIT DEFAULT 0,
        created_at DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_Collections_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
END

-- Create UserCollections table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'UserCollections')
BEGIN
    CREATE TABLE UserCollections (
        user_id INT NOT NULL,
        collection_id INT NOT NULL,
        added_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, collection_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (collection_id) REFERENCES Collections(collection_id)
    );
END

-- Create Features table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Features')
BEGIN
    CREATE TABLE Features (
        feature_id INT IDENTITY(1,1) PRIMARY KEY,
        name NVARCHAR(255) NOT NULL,
        description NVARCHAR(MAX),
        created_at DATETIME NOT NULL DEFAULT GETDATE()
    );
END

-- Create UserFeatures table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'UserFeatures')
BEGIN
    CREATE TABLE UserFeatures (
        user_id INT NOT NULL,
        feature_id INT NOT NULL,
        added_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, feature_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (feature_id) REFERENCES Features(feature_id)
    );
END

-- Create Wallet table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Wallet')
BEGIN
    CREATE TABLE Wallet (
        wallet_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL UNIQUE,
        balance DECIMAL(10,2) NOT NULL DEFAULT 0,
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        last_updated DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
END

-- Create Transaction table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Transactions')
BEGIN
    CREATE TABLE Transactions (
        transaction_id INT IDENTITY(1,1) PRIMARY KEY,
        wallet_id INT NOT NULL,
        amount DECIMAL(10,2) NOT NULL,
        type NVARCHAR(50) NOT NULL, -- 'deposit', 'withdrawal', 'purchase'
        description NVARCHAR(MAX),
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (wallet_id) REFERENCES Wallet(wallet_id)
    );
END

-- Create Friends table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Friends')
BEGIN
    CREATE TABLE Friends (
        user_id INT NOT NULL,
        friend_id INT NOT NULL,
        status NVARCHAR(50) NOT NULL, -- 'pending', 'accepted', 'blocked'
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        updated_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, friend_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (friend_id) REFERENCES Users(user_id)
    );
END

-- Create Configurations table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Configurations')
BEGIN
    CREATE TABLE Configurations (
        config_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        setting_name NVARCHAR(255) NOT NULL,
        setting_value NVARCHAR(MAX),
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        updated_at DATETIME NOT NULL DEFAULT GETDATE(),
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
END

-- Create OwnedGames table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'OwnedGames')
BEGIN
    CREATE TABLE OwnedGames (
        owned_game_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        game_id INT NOT NULL,
        purchase_date DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_OwnedGames_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
END

-- Create OwnedGames_Collection table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'OwnedGames_Collection')
BEGIN
    CREATE TABLE OwnedGames_Collection (
        collection_id INT NOT NULL,
        game_id INT NOT NULL,
        PRIMARY KEY (collection_id, game_id),
        FOREIGN KEY (collection_id) REFERENCES Collections(collection_id) ON DELETE CASCADE ON UPDATE CASCADE,
        FOREIGN KEY (game_id) REFERENCES OwnedGames(game_id) ON DELETE CASCADE ON UPDATE CASCADE
    );
END

-- Create Friendships table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Friendships')
BEGIN
    CREATE TABLE Friendships (
        friendship_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        friend_id INT NOT NULL,
        CONSTRAINT FK_Friendships_User FOREIGN KEY (user_id) REFERENCES Users(user_id),
        CONSTRAINT FK_Friendships_Friend FOREIGN KEY (friend_id) REFERENCES Users(user_id),
        CONSTRAINT UQ_Friendship UNIQUE (user_id, friend_id),
        CONSTRAINT CHK_FriendshipUsers CHECK (user_id != friend_id)
    );
END

-- Add indexes for better query performance
IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Friendships_UserId')
BEGIN
    CREATE INDEX IX_Friendships_UserId ON Friendships(user_id);
END

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE name = 'IX_Friendships_FriendId')
BEGIN
    CREATE INDEX IX_Friendships_FriendId ON Friendships(friend_id);
END

-- Insert stored procedures
:r Data\Procedures\Users\GetAllUsers.sql
:r Data\Procedures\Users\GetUserById.sql
:r Data\Procedures\Users\UserLogin.sql
:r Data\Procedures\Users\CreateUser.sql
:r Data\Procedures\Users\UpdateUser.sql

-- Add more :r commands for other stored procedures as they are created
-- :r Data\Procedures\Achievements\...
:r Data\Procedures\Collections\GetPublicCollectionsForUser.sql
:r Data\Procedures\Collections\GetPrivateCollectionsForUser.sql
:r Data\Procedures\Collections\GetAllCollectionsForUser.sql
:r Data\Procedures\Collections\MakeCollectionPrivate.sql
:r Data\Procedures\Collections\MakeCollectionPublic.sql
:r Data\Procedures\Collections\DeleteCollection.sql
:r Data\Procedures\Collections\CreateCollection.sql
:r Data\Procedures\Collections\UpdateCollection.sql
:r Data\Procedures\OwnedGames\GetAllOwnedGames.sql
:r Data\Procedures\OwnedGames\GetOwnedGameById.sql
:r Data\Procedures\OwnedGames\AddGameToCollection.sql
:r Data\Procedures\OwnedGames\RemoveGameFromCollection.sql
:r Data\Procedures\OwnedGames\GetGamesInCollection.sql
:r Data\Procedures\Friendships\GetFriendsForUser.sql
:r Data\Procedures\Friendships\GetFriendshipCountForUser.sql
:r Data\Procedures\Friendships\AddFriend.sql
:r Data\Procedures\Friendships\RemoveFriend.sql


