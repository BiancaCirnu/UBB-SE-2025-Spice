-- Drop existing objects if they exist
--:r Data\DropExistingObjects.sql

-- Create Users table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Users')
BEGIN
    CREATE TABLE Users (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
        email NVARCHAR(255) NOT NULL UNIQUE,
        username NVARCHAR(255) NOT NULL UNIQUE,
        password NVARCHAR(255) NOT NULL,
        profile_picture NVARCHAR(MAX),
        description NVARCHAR(MAX),
        developer BIT NOT NULL DEFAULT 0,
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        last_login DATETIME
    );
END

-- Create Achievements table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Achievements')
BEGIN
    CREATE TABLE Achievements (
        achievement_id INT IDENTITY(1,1) PRIMARY KEY,
        description NVARCHAR(MAX),
		achievement_type NVARCHAR(MAX), 
		points int,
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
        name NVARCHAR(255) NOT NULL,
        description NVARCHAR(MAX),
        created_at DATETIME NOT NULL DEFAULT GETDATE()
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





-- Insert stored procedures
--:r Data\Procedures\Users\GetAllUsers.sql
--:r Data\Procedures\Users\GetUserById.sql
--:r Data\Procedures\Users\UserLogin.sql
--:r Data\Procedures\Users\CreateUser.sql
--:r Data\Procedures\Users\UpdateUser.sql

-- Add more :r commands for other stored procedures as they are created
-- :r Data\Procedures\Achievements\...
-- :r Data\Procedures\Collections\...
-- :r Data\Procedures\Features\...
-- :r Data\Procedures\Wallet\... 


CREATE TABLE SoldGames (
    sold_game_id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    game_id INT NOT NULL,
    sold_date DATETIME,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

insert into SoldGames(user_id, game_id) values (1, 1)

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Reviews')
BEGIN
	CREATE TABLE Reviews (
    review_id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    review_text NVARCHAR(MAX),
    review_date DATETIME,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
);
END

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('FRIENDSHIP1', 'You made a friend, you get a point', 'Friendships', 1)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('FRIENDSHIP2', 'You made 5 friends, you get 3 points', 'Friendships', 3)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('FRIENDSHIP3', 'You made 10 friends, you get 5 points', 'Friendships', 5)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('FRIENDSHIP4', 'You made 50 friends, you get 10 points', 'Friendships', 10)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('FRIENDSHIP5', 'You made 100 friends, you get 15 points', 'Friendships', 15)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('OWNEDGAMES1', 'You own 1 game, you get 1 point', 'Owned Games', 1)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('OWNEDGAMES2', 'You own 5 games, you get 3 poinst', 'Owned Games', 3)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('OWNEDGAMES3', 'You own 10 games, you get 5 points', 'Owned Games', 5)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('OWNEDGAMES4', 'You own 50 games, you get 10 points', 'Owned Games', 10)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('SOLDGAMES1', 'You sold 1 game, you get 1 point', 'Sold Games', 1)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('SOLDGAMES2', 'You sold 5 games, you get 3 points', 'Sold Games', 3)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('SOLDGAMES3', 'You sold 10 games, you get 5 points', 'Sold Games', 5)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('SOLDGAMES4', 'You sold 50 games, you get 10 points', 'Sold Games', 10)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('REVIEW1', 'You gave 1 review, you get 1 point', 'Number of Reviews', 1)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('REVIEW2', 'You gave 5 reviews, you get 3 points', 'Number of Reviews', 3)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('REVIEW3', 'You gave 10 reviews, you get 5 points', 'Number of Reviews', 5)

insert into Achievements(achievement_name, description, achievement_type, points) 
values ('REVIEW4', 'You gave 50 reviews, you get 10 points', 'Number of Reviews', 10)

update Achievements 
set icon_url = 'https://cdn-icons-png.flaticon.com/512/5139/5139999.png'
where achievement_id > 0