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
        points int not null,
        icon NVARCHAR(MAX),
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

-- Create Wallet table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Wallet')
BEGIN
    CREATE TABLE Wallet (
        wallet_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL UNIQUE,
        balance DECIMAL(10,2) NOT NULL DEFAULT 0,
        points int NOT NULL DEFAULT 0,
        FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );
END

-- Create User_Achievements table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'User_Achievements')
BEGIN
    CREATE TABLE User_Achievements (
        user_id INT NOT NULL,
        achievement_id INT NOT NULL,
        unlocked_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, achievement_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id)
    );
END


-- Create User_Collections table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'User_Collections')
BEGIN
    CREATE TABLE User_Collections (
        user_id INT NOT NULL,
        collection_id INT NOT NULL,
        added_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, collection_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (collection_id) REFERENCES Collections(collection_id)
    );
END


-- Create User_Features table (junction table)
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'User_Features')
BEGIN
    CREATE TABLE User_Features (
        user_id INT NOT NULL,
        feature_id INT NOT NULL,
        added_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, feature_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (feature_id) REFERENCES Features(feature_id)
    );
END

-- Create UserRelations table
IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'UserRelations')
BEGIN
    CREATE TABLE UserRelations (
        user1_id INT NOT NULL,
        user2_id INT NOT NULL,
        status NVARCHAR(50) NOT NULL, -- 'pending', 'accepted', 'blocked'
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
        PRIMARY KEY (user_id, friend_id),
        FOREIGN KEY (user_id) REFERENCES Users(user_id),
        FOREIGN KEY (friend_id) REFERENCES Users(user_id)
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

IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'PointsOffers')
begin 
    create table PointOffers(
        offerId  INT IDENTITY(1,1) PRIMARY KEY,
        numberOfPoints int not null,
        value int not null
    );
end

-- initialize points offers
insert into PointsOffers(numberOfPoints, value) values
(5, 2),
(25, 8), 
(50, 15), 
(100, 20),
(500, 50)
-- Create Configurations table
--IF NOT EXISTS (SELECT * FROM sys.objects WHERE type = 'U' AND name = 'Configurations')
--BEGIN
--    CREATE TABLE Configurations (
--        config_id INT IDENTITY(1,1) PRIMARY KEY,
--        user_id INT NOT NULL,
--        setting_name NVARCHAR(255) NOT NULL,
--        setting_value NVARCHAR(MAX),
--        created_at DATETIME NOT NULL DEFAULT GETDATE(),
--        updated_at DATETIME NOT NULL DEFAULT GETDATE(),
--        FOREIGN KEY (user_id) REFERENCES Users(user_id)
--    );
--END

-- Insert stored procedures
:r Data\Procedures\Users\GetAllUsers.sql
:r Data\Procedures\Users\GetUserById.sql
:r Data\Procedures\Users\UserLogin.sql
:r Data\Procedures\Users\CreateUser.sql
:r Data\Procedures\Users\UpdateUser.sql

-- Add more :r commands for other stored procedures as they are created
-- :r Data\Procedures\Achievements\...
-- :r Data\Procedures\Collections\...
-- :r Data\Procedures\Features\...
-- :r Data\Procedures\Wallet\... 