DROP TABLE IF EXISTS Feature_User;
DROP TABLE IF EXISTS UserAchievements;
DROP TABLE IF EXISTS OwnedGames_Collection;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS Collections;
DROP TABLE IF EXISTS Features;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS UserSessions;
DROP TABLE IF EXISTS Users;
drop table if exists PasswordResetCodes;

drop procedure if exists CreateUser;
drop procedure if exists GetAllUsers;
drop procedure if exists GetUserByEmail
DROP PROCEDURE IF EXISTS ValidateResetCode;
DROP PROCEDURE IF EXISTS ResetPassword;
DROP PROCEDURE IF EXISTS GetUserByEmail;
DROP PROCEDURE IF EXISTS StorePasswordResetCode;
DROP PROCEDURE IF EXISTS VerifyResetCode;
drop procedure if exists CleanupResetCodes;

    CREATE TABLE Users (
        user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE, -- case sensitivity for usernames
    email NVARCHAR(100) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE, -- case sensitivity for emails
    hashed_password NVARCHAR(255) NOT NULL,
        developer BIT NOT NULL DEFAULT 0,
        created_at DATETIME NOT NULL DEFAULT GETDATE(),
    last_login DATETIME NULL
);

CREATE TABLE UserSessions (
    session_id UNIQUEIDENTIFIER PRIMARY KEY,
    user_id INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),  
    expires_at DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);


-- Achievements Table
CREATE TABLE Achievements (
    achievement_id INT PRIMARY KEY identity(1,1),
	achievement_name char(30),
	description char(100),
    achievement_type  NVARCHAR(100) NOT NULL,
    points INT NOT NULL CHECK (points >= 0),
	icon_url NVARCHAR(255) CHECK (icon_url LIKE '%.svg' OR icon_url LIKE '%.png' OR icon_url LIKE '%.jpg')
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
create TABLE Wallet (
    wallet_id INT PRIMARY KEY identity(1,1),
    user_id INT unique,
    points INT NOT NULL DEFAULT 0,
    money_for_games DECIMAL(10,2) NOT NULL DEFAULT 0.00
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);

-- OwnedGames_Collection Table
CREATE TABLE OwnedGames_Collection (
    collection_id INT NOT NULL,
    game_id INT NOT NULL,
    date_added DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (collection_id, game_id),
    FOREIGN KEY (collection_id) REFERENCES Collections(collection_id) ON DELETE CASCADE ON UPDATE CASCADE
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

-- Password Reset Codes Table
CREATE TABLE PasswordResetCodes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    reset_code NVARCHAR(6) NOT NULL,
    expiration_time DATETIME NOT NULL,
    used BIT DEFAULT 0,
	email nvarchar(255),
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);

CREATE TABLE UserAchievements (
    user_id INT NOT NULL,
    achievement_id INT NOT NULL,
    unlocked_at DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (user_id, achievement_id),
    FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id) ON DELETE CASCADE ON UPDATE CASCADE
);

CREATE TABLE OwnedGames (
        owned_game_id INT IDENTITY(1,1) PRIMARY KEY,
        user_id INT NOT NULL,
        game_id INT NOT NULL,
        purchase_date DATETIME DEFAULT GETDATE(),
        CONSTRAINT FK_OwnedGames_User FOREIGN KEY (user_id) REFERENCES Users(user_id)
    );

CREATE TABLE Reviews (
    review_id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    review_text NVARCHAR(MAX),
    review_date DATETIME,
    FOREIGN KEY (user_id) REFERENCES Users(user_id),
);

CREATE TABLE SoldGames (
    sold_game_id INT PRIMARY KEY IDENTITY,
    user_id INT NOT NULL,
    game_id INT NOT NULL,
    sold_date DATETIME,
    FOREIGN KEY (user_id) REFERENCES Users(user_id)
);



INSERT INTO Users (email, username, hashed_password, developer, last_login) VALUES
('alice@example.com', 'AliceGamer', 'hashed_password_1', 1, '2025-03-20 14:25:00'),
('bob@example.com', 'BobTheBuilder', 'hashed_password_2', 0, '2025-03-21 10:12:00'),
('charlie@example.com', 'CharlieX', 'hashed_password_3', 0, '2025-03-22 18:45:00'),
('diana@example.com', 'DianaRocks', 'hashed_password_4', 0, '2025-03-19 22:30:00'),
('eve@example.com', 'Eve99', 'hashed_password_5', 1, '2025-03-23 08:05:00'),
('frank@example.com', 'FrankTheTank', 'hashed_password_6', 0, '2025-03-24 16:20:00'),
('grace@example.com', 'GraceSpeed', 'hashed_password_7', 0, '2025-03-25 11:40:00'),
('harry@example.com', 'HarryWizard', 'hashed_password_8', 0, '2025-03-20 20:15:00'),
('ivy@example.com', 'IvyNinja', 'hashed_password_9', 0, '2025-03-22 09:30:00'),
('jack@example.com', 'JackHacks', 'hashed_password_10', 1, '2025-03-24 23:55:00');

INSERT INTO Users (email, username, hashed_password, developer, last_login) VALUES
('maracocaina77@gmail.com', 'Mara', 'hashed_password_1', 0, '2025-03-20 14:25:00');

go 

select * from Users
select * from Achievements
select * from UserAchievements
insert into UserAchievements values (12, 1, GETDATE())


CREATE PROCEDURE CheckUserExists
    @email NVARCHAR(100),
    @username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check for existing email and username
    SELECT 
        CASE 
            WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @email) THEN 'EMAIL_EXISTS'
            WHEN EXISTS (SELECT 1 FROM Users WHERE Username = @username) THEN 'USERNAME_EXISTS'
            ELSE NULL
        END AS ErrorType;
END;
go

CREATE PROCEDURE CreateUser
    @username NVARCHAR(50),
    @email NVARCHAR(100),
    @hashed_password NVARCHAR(255),
    @developer BIT
AS
BEGIN
    INSERT INTO Users (username, email, hashed_password, developer)
    VALUES (@username, @email, @hashed_password, @developer);

    SELECT 
        user_id,
        username,
        email,
        hashed_password,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = SCOPE_IDENTITY();
END;
go

CREATE PROCEDURE DeleteUser
    @userId INT
AS
BEGIN
    DELETE FROM Users
    WHERE user_id = @userId;
END 

go

CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SELECT 
        user_id,
        username,
        email,
        developer,
        created_at,
        last_login
    FROM Users
    ORDER BY username;
END 

go

CREATE PROCEDURE GetUserByEmailOrUsername
    @EmailOrUsername NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    SELECT user_id, username, email, hashed_password, developer, created_at, last_login
    FROM Users
    WHERE username = @EmailOrUsername OR email = @EmailOrUsername;
END 
go
CREATE PROCEDURE GetUserById
    @userId INT
AS
BEGIN
    SELECT 
        user_id,
        username,
        email,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = @userId;
END 

go
CREATE PROCEDURE UpdateLastLogin
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE Users
    SET last_login = GETDATE()
    WHERE user_id = @user_id;

    SELECT 
        user_id,
        username,
        email,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = @user_id;
END 

go
CREATE PROCEDURE UpdateUser
    @user_id INT,
    @email NVARCHAR(100),
    @username NVARCHAR(50),
    @developer BIT
AS
BEGIN
    UPDATE Users
    SET 
        email = @email,
        username = @username,
        developer = @developer
    WHERE user_id = @user_id;

    SELECT 
        user_id,
        username,
        email,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = @user_id;
END 

go

CREATE PROCEDURE CreateSession
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete any existing sessions for this user
    DELETE FROM UserSessions WHERE user_id = @user_id;

    -- Create new session with 2-hour expiration
    INSERT INTO UserSessions (user_id, session_id, created_at, expires_at)
    VALUES (
        @user_id,
        NEWID(),
        GETDATE(),
        DATEADD(HOUR, 2, GETDATE())
    );

    -- Return the session details
    SELECT 
        us.session_id,
        us.created_at,
        us.expires_at,
        u.user_id,
        u.username,
        u.email,
        u.developer,
        u.created_at as user_created_at,
        u.last_login
    FROM UserSessions us
    JOIN Users u ON us.user_id = u.user_id
    WHERE us.user_id = @user_id;
END; 

go
CREATE PROCEDURE DeleteSession
    @session_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM UserSessions WHERE session_id = @session_id;
END; 
go

CREATE PROCEDURE GetSessionById
    @session_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    SELECT session_id, user_id, created_at, expires_at
    FROM UserSessions
    WHERE session_id = @session_id;
END 

go

CREATE PROCEDURE GetUserFromSession
    @session_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if session exists and is not expired
    IF EXISTS (
        SELECT 1 
        FROM UserSessions 
        WHERE session_id = @session_id 
        AND expires_at > GETDATE()
    )
    BEGIN
        -- Return user details
        SELECT 
            u.user_id,
            u.username,
            u.email,
            u.developer,
            u.created_at,
            u.last_login
        FROM UserSessions us
        JOIN Users u ON us.user_id = u.user_id
        WHERE us.session_id = @session_id;
    END
    ELSE
    BEGIN
        -- If session is expired or doesn't exist, delete it
        DELETE FROM UserSessions WHERE session_id = @session_id;
    END
END; 

go
CREATE PROCEDURE LoginUser
    @EmailOrUsername NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get user data including password hash
    SELECT user_id,
        username,
        email,
        hashed_password,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE username = @EmailOrUsername OR email = @EmailOrUsername;
END 
go


-- Validate Reset Code
CREATE PROCEDURE ValidateResetCode
    @email NVARCHAR(255),
    @reset_code NVARCHAR(6)
AS
BEGIN
    DECLARE @isValid BIT = 0;
    
    -- Check if the code exists, is not used, and hasn't expired
    IF EXISTS (
        SELECT 1 
        FROM PasswordResetCodes 
        WHERE email = @email 
        AND reset_code = @reset_code 
        AND used = 0 
        AND expiration_time > GETDATE()
    )
    BEGIN
        -- Mark the code as used
        UPDATE PasswordResetCodes 
        SET used = 1 
        WHERE email = @email 
        AND reset_code = @reset_code;
        
        SET @isValid = 1;
    END
    
    SELECT @isValid AS isValid;
END
GO


CREATE PROCEDURE StorePasswordResetCode     
    @userId int,
    @resetCode nvarchar(6),
    @expirationTime datetime
AS
BEGIN
    INSERT INTO PasswordResetCodes (user_id, reset_code, expiration_time)
    VALUES (@userId, @resetCode, @expirationTime)
END

go
CREATE PROCEDURE VerifyResetCode
    @email nvarchar(255),
    @resetCode nvarchar(6)
AS
BEGIN
    DECLARE @userId int
    SELECT @userId = user_id FROM Users WHERE email = @email

    IF EXISTS (
        SELECT 1 
        FROM PasswordResetCodes 
        WHERE user_id = @userId 
        AND reset_code = @resetCode 
        AND expiration_time > GETUTCDATE()
        AND used = 0
    )
        SELECT 1 as Result
    ELSE
        SELECT 0 as Result
END

-- SELECT @result AS VerificationResult;


go

CREATE PROCEDURE ResetPassword
    @email nvarchar(255),
    @resetCode nvarchar(6),
    @newPassword nvarchar(max)
AS
BEGIN
    BEGIN TRANSACTION
    
    DECLARE @userId int
    SELECT @userId = user_id FROM Users WHERE email = @email

    IF EXISTS (
        SELECT 1 
        FROM PasswordResetCodes 
        WHERE user_id = @userId 
        AND reset_code = @resetCode 
        AND expiration_time > GETUTCDATE()
        AND used = 0
    )
    BEGIN
        UPDATE Users 
        SET hashed_password = @newPassword 
        WHERE user_id = @userId

        --UPDATE PasswordResetCodes 
       -- SET used = 1 
       -- WHERE user_id = @userId 
        --AND reset_code = @resetCode

		-- Delete the used reset code
        DELETE FROM PasswordResetCodes
        WHERE user_id = @UserId
        AND reset_code = @ResetCode

        COMMIT
        SELECT 1 as Result
    END
    ELSE
    BEGIN
        ROLLBACK
        SELECT 0 as Result
    END
END
go 
CREATE PROCEDURE CleanupResetCodes
AS
BEGIN
    -- Delete expired codes
    DELETE FROM PasswordResetCodes 
    WHERE expiration_time < GETUTCDATE()
END
GO

go
CREATE PROCEDURE GetUserByEmail
    @email NVARCHAR(255)
AS
BEGIN
    SELECT * FROM Users
    WHERE email = @email
END

select * from Wallet

-------- WALLET PROCEDURES-------------
go
create or alter procedure GetWalletById @wallet_id int as
begin
	select * from Wallet where @wallet_id = wallet_id
end
go

create or alter procedure WinPoints @amount int, @userId int 
as 
begin
	update  Wallet 
	set points = points + @amount
	where user_id = @userId
end
go
create or alter procedure CreateWallet @user_id int as
begin
	insert into Wallet (user_id, points, money_for_games)
	values (@user_id,0,0)

	update Wallet
	set user_id = wallet_id
	where wallet_id = (select max(wallet_id) from Wallet)
end
go

create or alter procedure AddMoney @amount decimal, @userId int as
begin 
	update wallet  
	set money_for_games = money_for_games + @amount
	where user_id = @userId
end
go

create or alter procedure BuyPoints @price decimal, @numberOfPoints int, @userId int 
as
begin
	update Wallet 
	set points = points + @numberOfPoints
	where user_id = @userId;

	update Wallet
	set money_for_games = money_for_games - @price 
	where user_id = @userId
end

go

create or alter procedure BuyWithMoney @amount decimal, @userId int 
as 
begin
	update  Wallet 
	set money_for_games = money_for_games - @amount
	where user_id = @userId
end

go

create or alter procedure BuyWithPoints @amount int, @userId int 
as 
begin
	update  Wallet 
	set points = points - @amount
	where user_id = @userId
end
go
create or alter procedure CreateWallet @user_id int as
begin
	insert into Wallet (user_id, points, money_for_games)
	values (@user_id,0,0)

	update Wallet
	set user_id = wallet_id
	where wallet_id = (select max(wallet_id) from Wallet)
end


------Create table PointsOffers ----
create table PointsOffers(
    offerId  INT IDENTITY(1,1) PRIMARY KEY,
    numberOfPoints int not null,
    value int not null
);
insert into PointsOffers(numberOfPoints, value) values
(5, 2),
(25, 8), 
(50, 15), 
(100, 20),
(500, 50)

go
create or alter procedure GetAllPointsOffers as 
begin
	select numberOfPoints, value from PointsOffers
end
go
create or alter procedure GetPointsOfferByID @offerId int as
begin
	select numberOfPoints, value from PointsOffers where offerId = @offerId
end
go

create or alter procedure RemoveWallet @wallet_id int as
begin
	delete from Wallet where wallet_id = @wallet_id
end



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
--:r Data\Procedures\Users\GetAllUsers.sql
--:r Data\Procedures\Users\GetUserById.sql
--:r Data\Procedures\Users\UserLogin.sql
--:r Data\Procedures\Users\CreateUser.sql
--:r Data\Procedures\Users\UpdateUser.sql
--- initialize wallet for first 11 users
exec createWallet @user_id = 1;
exec createWallet @user_id = 2;
exec createWallet @user_id = 3;
exec createWallet @user_id = 4;
exec createWallet @user_id = 5;
exec createWallet @user_id = 6;
exec createWallet @user_id = 7;
exec createWallet @user_id = 8;
exec createWallet @user_id = 9;
exec createWallet @user_id = 11;


-- Add more :r commands for other stored procedures as they are created
-- :r Data\Procedures\Achievements\...
-- :r Data\Procedures\Collections\...
-- :r Data\Procedures\Features\...
-- :r Data\Procedures\Wallet\... 

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


