DROP TABLE IF EXISTS UserAchievements;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS Feature_User;
DROP TABLE IF EXISTS Features;
DROP TABLE IF EXISTS OwnedGames_Collection;
DROP TABLE IF EXISTS OwnedGames;
DROP TABLE IF EXISTS Collections;
DROP TABLE IF EXISTS Friendships;
DROP TABLE IF EXISTS UserProfiles;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS PointsOffers
DROP TABLE IF exists PasswordResetCodes;
DROP TABLE IF EXISTS UserSessions;
DROP TABLE IF EXISTS Users;


DROP PROCEDURE IF EXISTS DeleteUser;
DROP PROCEDURE IF EXISTS AddFriend;
DROP PROCEDURE IF EXISTS AddGameToCollection;
DROP PROCEDURE IF EXISTS AddMoney;
DROP PROCEDURE IF EXISTS BuyPoints;
DROP PROCEDURE IF EXISTS BuyWithMoney;
DROP PROCEDURE IF EXISTS BuyWithPoints;
DROP PROCEDURE IF EXISTS ChangeEmailForUserId;
DROP PROCEDURE IF EXISTS ChangePassword;
DROP PROCEDURE IF EXISTS ChangeUsername;
DROP PROCEDURE IF EXISTS CreateCollection;
DROP PROCEDURE IF EXISTS CreateUser;
DROP PROCEDURE IF EXISTS CreateUserProfile;
DROP PROCEDURE IF EXISTS CreateWallet;
DROP PROCEDURE IF EXISTS DeleteCollection;
DROP PROCEDURE IF EXISTS GetAchievementId;
DROP PROCEDURE IF EXISTS GetAllCollections;
DROP PROCEDURE IF EXISTS GetAllCollectionsForUser;
DROP PROCEDURE IF EXISTS GetAllFriendships;
DROP PROCEDURE IF EXISTS GetAllOwnedGames;
DROP PROCEDURE IF EXISTS GetAllPointsOffers;
DROP PROCEDURE IF EXISTS GetFriendsForUser;
DROP PROCEDURE IF EXISTS GetFriendshipCountForUser;
DROP PROCEDURE IF EXISTS GetGamesInCollection;
DROP PROCEDURE IF EXISTS GetOwnedGameById;
DROP PROCEDURE IF EXISTS GetPointsOfferByID;
DROP PROCEDURE IF EXISTS GetPrivateCollectionsForUser;
DROP PROCEDURE IF EXISTS GetPublicCollectionsForUser;
DROP PROCEDURE IF EXISTS GetUserByEmail;
DROP PROCEDURE IF EXISTS GetUserById;
DROP PROCEDURE IF EXISTS GetUserByUsername;
DROP PROCEDURE IF EXISTS GetUserProfileByUserId;
DROP PROCEDURE IF EXISTS GetWalletById;
DROP PROCEDURE IF EXISTS MakeCollectionPrivate;
DROP PROCEDURE IF EXISTS MakeCollectionPublic;
DROP PROCEDURE IF EXISTS RemoveFriend;
DROP PROCEDURE IF EXISTS RemoveGameFromCollection;
DROP PROCEDURE IF EXISTS UpdateCollection;
DROP PROCEDURE IF EXISTS WinPoints;
DROP PROCEDURE IF EXISTS GetAllUsers;
DROP PROCEDURE IF EXISTS GetUserByEmailOrUsername;
DROP PROCEDURE IF EXISTS CheckUserExists;
DROP PROCEDURE IF EXISTS UpdateLastLogin;
DROP PROCEDURE IF EXISTS UpdateUser;
DROP PROCEDURE IF EXISTS CreateSession;
DROP PROCEDURE IF EXISTS DeleteSession;
DROP PROCEDURE IF EXISTS GetSessionById;
DROP PROCEDURE IF EXISTS GetUserFromSession;
DROP PROCEDURE IF EXISTS LoginUser;
DROP PROCEDURE IF EXISTS LogoutUser;
DROP PROCEDURE IF EXISTS UpdateUserProfile;
DROP PROCEDURE IF EXISTS ValidateResetCode;
DROP PROCEDURE IF EXISTS StorePasswordResetCode;
DROP PROCEDURE IF EXISTS VerifyResetCode;
DROP PROCEDURE IF EXISTS ResetPassword;
DROP PROCEDURE IF EXISTS CleanupResetCodes;
DROP PROCEDURE IF EXISTS GetCollectionById;
DROP PROCEDURE IF EXISTS GetGamesNotInCollection;
DROP PROCEDURE IF EXISTS GetAllFeatures;
DROP PROCEDURE IF EXISTS GetFeaturesByType;
DROP PROCEDURE IF EXISTS CheckFeatureOwnership;
DROP PROCEDURE IF EXISTS DeleteWallet;;
DROP PROCEDURE IF EXISTS CheckFeaturePurchase;
DROP PROCEDURE IF EXISTS EquipFeature;
DROP PROCEDURE IF EXISTS GetAllFeatures;
DROP PROCEDURE IF EXISTS GetFeaturesByType;
DROP PROCEDURE IF EXISTS GetAllFeaturesWithOwnership;
DROP PROCEDURE IF EXISTS GetAllFeaturesWithOwnership;
DROP PROCEDURE IF EXISTS UnequipFeature;
DROP PROCEDURE IF EXISTS UnequipFeaturesByType;
DROP PROCEDURE IF EXISTS GetAllAchievements;
DROP PROCEDURE IF EXISTS GetNumberOfOwnedGames;
DROP PROCEDURE IF EXISTS GetNumberOfReviews;
DROP PROCEDURE IF EXISTS GetNumberOfSoldGames;
DROP PROCEDURE IF EXISTS GetUnlockedAchievements;
DROP PROCEDURE IF EXISTS GetUnlockedDataForAchievement;
DROP PROCEDURE IF EXISTS IsAchievementUnlocked;
DROP PROCEDURE IF EXISTS RemoveAchievement;
DROP PROCEDURE IF EXISTS dbo.AddFriend;
DROP PROCEDURE IF EXISTS AddGameToCollection;
DROP PROCEDURE IF EXISTS AddMoney;
DROP PROCEDURE IF EXISTS BuyPoints;
DROP PROCEDURE IF EXISTS BuyWithMoney;
DROP PROCEDURE IF EXISTS BuyWithPoints;
DROP PROCEDURE IF EXISTS CascadeDeleteUser;
DROP PROCEDURE IF EXISTS ChangeEmailForUserId;
DROP PROCEDURE IF EXISTS ChangePassword;
DROP PROCEDURE IF EXISTS ChangeUsername;
DROP PROCEDURE IF EXISTS CreateCollection;
DROP PROCEDURE IF EXISTS CreateUser;
DROP PROCEDURE IF EXISTS CreateUserProfile;
DROP PROCEDURE IF EXISTS CreateWallet;
DROP PROCEDURE IF EXISTS DeleteCollection;
DROP PROCEDURE IF EXISTS DeleteFriendshipsForUser;
DROP PROCEDURE IF EXISTS DeleteWallet;
DROP PROCEDURE IF EXISTS GetAchievementId;
DROP PROCEDURE IF EXISTS GetAllCollections;
DROP PROCEDURE IF EXISTS GetAllCollectionsForUser;
DROP PROCEDURE IF EXISTS GetAllFriendships;
DROP PROCEDURE IF EXISTS GetAllOwnedGames;
DROP PROCEDURE IF EXISTS GetAllPointsOffers;
DROP PROCEDURE IF EXISTS GetFriendsForUser;
DROP PROCEDURE IF EXISTS GetFriendshipCountForUser;
DROP PROCEDURE IF EXISTS GetGamesInCollection;
DROP PROCEDURE IF EXISTS GetOwnedGameById;
DROP PROCEDURE IF EXISTS GetPointsOfferByID;
DROP PROCEDURE IF EXISTS GetPrivateCollectionsForUser;
DROP PROCEDURE IF EXISTS GetPublicCollectionsForUser;
DROP PROCEDURE IF EXISTS GetUserByEmail;
DROP PROCEDURE IF EXISTS GetUserById;
DROP PROCEDURE IF EXISTS GetUserByUsername;
DROP PROCEDURE IF EXISTS GetUserProfileByUserId;
DROP PROCEDURE IF EXISTS GetWalletById;
DROP PROCEDURE IF EXISTS MakeCollectionPrivate;
DROP PROCEDURE IF EXISTS MakeCollectionPublic;
DROP PROCEDURE IF EXISTS RemoveFriend;
DROP PROCEDURE IF EXISTS RemoveGameFromCollection;
DROP PROCEDURE IF EXISTS UpdateCollection;
DROP PROCEDURE IF EXISTS UpdateUserProfileBio;
DROP PROCEDURE IF EXISTS UpdateUserProfilePicture;
DROP PROCEDURE IF EXISTS WinPoints;


----------------------------- USERS --------------------------------
go
CREATE TABLE Users (
    user_id INT IDENTITY(1,1) PRIMARY KEY,
    username NVARCHAR(50) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE, -- case sensitivity for usernames
    email NVARCHAR(100) COLLATE SQL_Latin1_General_CP1254_CS_AS NOT NULL UNIQUE, -- case sensitivity for emails
    hashed_password NVARCHAR(255) NOT NULL,
    developer BIT NOT NULL DEFAULT 0,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),
    last_login DATETIME NULL
);

go
CREATE or alter PROCEDURE CreateUser
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
CREATE or alter PROCEDURE DeleteUser
    @user_id INT
AS
BEGIN

	exec DeleteFriendshipsForUser @user_id =@user_id
    DELETE FROM Users
    WHERE user_id = @user_id;
END 

go
CREATE PROCEDURE GetAllUsers
AS
BEGIN
    SET NOCOUNT ON;

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
create or alter procedure GetUserByEmail @email nvarchar(100)
as
begin
	SELECT user_id, username, email, hashed_password, developer, created_at, last_login
	from Users
	where @email = email
end

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
CREATE or alter PROCEDURE GetUserById
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

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
create or alter procedure GetUserByUsername @username char(50)
as
begin
	SELECT user_id, username, email, hashed_password, developer, created_at, last_login
	from Users
	where @username = username
end

go
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
Create or alter procedure ChangeEmailForUserId @user_id int, @newEmail char(50) as
begin
	update Users
	set email = @newEmail 
	where user_id = @user_id
end

go
create or alter procedure ChangePassword @user_id int, @newHashedPassword char(100) as
begin
		update Users set hashed_password = @newHashedPassword where user_id=@user_id
end

go
create or alter procedure ChangeUsername @user_id int, @newUsername char(50) as
begin
	update Users set username = @newUsername where user_id=@user_id
end
go
----------------------------- USER SESSIONS --------------------------------
CREATE TABLE UserSessions (
    session_id UNIQUEIDENTIFIER PRIMARY KEY,
    user_id INT NOT NULL,
    created_at DATETIME NOT NULL DEFAULT GETDATE(),  
    expires_at DATETIME NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);

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
CREATE PROCEDURE LogoutUser
    @session_id UNIQUEIDENTIFIER
AS
BEGIN
    DELETE FROM UserSessions WHERE session_id = @session_id;
END;
go
----------------------------- USER PROFILES --------------------------------
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
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);

go
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

go
CREATE or alter PROCEDURE GetUserProfileByUserId
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

go
CREATE PROCEDURE UpdateUserProfile
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


----------------------------- PASSWORD RESET CODES --------------------------------
CREATE TABLE PasswordResetCodes (
    id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    reset_code NVARCHAR(6) NOT NULL,
    expiration_time DATETIME NOT NULL,
    used BIT DEFAULT 0,
	email nvarchar(255),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);

go
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
create or alter procedure UpdateUserProfileBio @user_id int, @bio NVARCHAR(1000) as
begin
	update UserProfiles set bio = @bio where user_id = @user_id
end 
go
create or alter procedure UpdateUserProfilePicture @user_id int, @profile_picture NVARCHAR(255) as
begin
	update UserProfiles set profile_picture = @profile_picture where user_id = @user_id
end 
go

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

--SELECT @result AS VerificationResult;

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

----------------------------- WALLET --------------------------------
create TABLE Wallet (
    wallet_id INT PRIMARY KEY identity(1,1),
    user_id INT unique,
    points INT NOT NULL DEFAULT 0,
    money_for_games DECIMAL(10,2) NOT NULL DEFAULT 0.00
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);

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

----------------------------- POINTS OFFERS --------------------------------
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
create or alter procedure DeleteWallet @user_id int as
begin 
	DELETE FROM Wallet WHERE @user_Id = user_id
end 
go

----------------------------- FRIENDSHIPS --------------------------------
CREATE TABLE Friendships (
    friendship_id INT IDENTITY(1,1) PRIMARY KEY,
    user_id INT NOT NULL,
    friend_id INT NOT NULL,
    CONSTRAINT FK_Friendships_User FOREIGN KEY (user_id) REFERENCES Users(user_id),
    CONSTRAINT FK_Friendships_Friend FOREIGN KEY (friend_id) REFERENCES Users(user_id),
    CONSTRAINT UQ_Friendship UNIQUE (user_id, friend_id),
    CONSTRAINT CHK_FriendshipUsers CHECK (user_id != friend_id)
);


go
-- Add indexes for better query performance
CREATE INDEX IX_Friendships_UserId ON Friendships(user_id);
CREATE INDEX IX_Friendships_FriendId ON Friendships(friend_id);

go
CREATE OR ALTER PROCEDURE AddFriend
    @user_id INT,
    @friend_id INT
AS
BEGIN
    INSERT INTO Friendships (user_id, friend_id)
    VALUES (@user_id, @friend_id);
END
GO 
create or alter procedure DeleteFriendshipsForUser @user_id int as 
begin
	delete from Friendships where @user_id = user_id or @user_id = friend_id
end

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
CREATE OR ALTER PROCEDURE GetFriendshipCountForUser
    @user_id INT
AS
BEGIN
    SELECT COUNT(*) as friend_count
    FROM Friendships
    WHERE user_id = @user_id;
END
GO 
CREATE OR ALTER PROCEDURE RemoveFriend
    @friendship_id INT
AS
BEGIN
    DELETE FROM Friendships
    WHERE friendship_id = @friendship_id;
END
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
go
----------------------------- COLLECTIONS --------------------------------
CREATE TABLE Collections (
    collection_id INT PRIMARY KEY identity(1,1),
    user_id INT NOT NULL,
    name NVARCHAR(100) NOT NULL CHECK (LEN(name) >= 1 AND LEN(name) <= 100),
    cover_picture NVARCHAR(255) CHECK (cover_picture LIKE '%.svg' OR cover_picture LIKE '%.png' OR cover_picture LIKE '%.jpg'),
    is_public BIT DEFAULT 1,
    created_at DATE DEFAULT CAST(GETDATE() AS DATE),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
);
GO
CREATE OR ALTER PROCEDURE GetAllCollections
AS
BEGIN
    SELECT collection_id, user_id, name, cover_picture, is_public, created_at
    FROM Collections
    ORDER BY name;
END


go
CREATE OR ALTER PROCEDURE CreateCollection
	@user_id INT,
	@name NVARCHAR(100),
	@cover_picture NVARCHAR(255),
	@is_public BIT,
	@created_at DATE
AS
BEGIN
	-- Check if user exists
	IF NOT EXISTS (SELECT 1 FROM Users WHERE user_id = @user_id)
	BEGIN
		RAISERROR('User not found', 16, 1)
		RETURN
	END

	-- Insert new collection
	INSERT INTO Collections (
		user_id,
		name,
		cover_picture,
		is_public,
		created_at
	)
	VALUES (
		@user_id,
		@name,
		@cover_picture,
		@is_public,
		@created_at
	)

	-- Return the newly created collection
	SELECT 
		collection_id,
		user_id,
		name,
		cover_picture,
		is_public,
		created_at
	FROM Collections
	WHERE collection_id = SCOPE_IDENTITY()
END

SELECT * FROM Collections;
go
CREATE OR ALTER PROCEDURE DeleteCollection
    @user_id INT,
    @collection_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete associated records first (avoid foreign key constraint errors)
    DELETE FROM OwnedGames_Collection WHERE collection_id = @collection_id;

    -- Now delete the collection
    DELETE FROM Collections WHERE collection_id = @collection_id AND user_id = @user_id;
END
GO

GO 
CREATE OR ALTER PROCEDURE GetAllCollectionsForUser
    @user_id INT
AS
BEGIN
    SELECT collection_id, user_id, name, cover_picture, is_public, created_at
    FROM Collections
    WHERE user_id = @user_id
    ORDER BY created_at ASC;
END
GO
CREATE PROCEDURE GetCollectionById
    @collectionId INT,
    @user_id INT
AS
BEGIN
    SELECT 
        collection_id,
        user_id,
        name,
        cover_picture,
        is_public,
        created_at
    FROM Collections
    WHERE collection_id = @collectionId
    AND user_id = @user_id
END


go
CREATE OR ALTER PROCEDURE GetPrivateCollectionsForUser
    @user_id INT
AS
BEGIN
    SELECT collection_id, user_id, name, cover_picture, is_public, created_at
    FROM Collections
    WHERE user_id = @user_id AND is_public = 0
    ORDER BY name;
END
GO 
CREATE OR ALTER PROCEDURE GetPublicCollectionsForUser
    @user_id INT
AS
BEGIN
    SELECT collection_id, user_id, name, cover_picture, is_public, created_at
    FROM Collections
    WHERE user_id = @user_id AND is_public = 1
    ORDER BY name;
END
GO 
CREATE OR ALTER PROCEDURE MakeCollectionPrivate
    @user_id INT,
    @collection_id INT
AS
BEGIN
    UPDATE Collections
    SET is_public = 0
    WHERE collection_id = @collection_id AND user_id = @user_id;
END
GO 
CREATE OR ALTER PROCEDURE MakeCollectionPublic
    @user_id INT,
    @collection_id INT
AS
BEGIN
    UPDATE Collections
    SET is_public = 1
    WHERE collection_id = @collection_id AND user_id = @user_id;
END
GO 
CREATE OR ALTER PROCEDURE UpdateCollection
    @collection_id INT,
    @user_id INT,
    @name NVARCHAR(100),
    @cover_picture NVARCHAR(255),
    @is_public BIT,
    @created_at DATE
AS
BEGIN
    UPDATE Collections
    SET name = @name,
        cover_picture = @cover_picture,
        is_public = @is_public,
        created_at = @created_at
    WHERE collection_id = @collection_id AND user_id = @user_id;
END
GO

----------------------------- OWNED GAMES (mock table, should check OwnedGames team) --------------------------------
CREATE TABLE OwnedGames (
    game_id INT PRIMARY KEY IDENTITY(1,1),
    user_id INT NOT NULL,
    title NVARCHAR(100) NOT NULL CHECK (LEN(title) >= 1 AND LEN(title) <= 100),
    description NVARCHAR(MAX),
    cover_picture NVARCHAR(255) CHECK (cover_picture LIKE '%.svg' OR cover_picture LIKE '%.png' OR cover_picture LIKE '%.jpg'),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE
);
go
----------------------------- OWNEDGAMES_COLLECTION --------------------------------
-- OwnedGames_Collection Table
CREATE TABLE OwnedGames_Collection (
    collection_id INT NOT NULL,
    game_id INT NOT NULL,
    PRIMARY KEY (collection_id, game_id),
    FOREIGN KEY (collection_id) REFERENCES Collections(collection_id),
    FOREIGN KEY (game_id) REFERENCES OwnedGames(game_id) 
);
go
CREATE OR ALTER PROCEDURE GetAllOwnedGames
AS
BEGIN
    SELECT game_id, user_id, title, description, cover_picture
    FROM OwnedGames
    ORDER BY title;
END
GO
CREATE OR ALTER PROCEDURE AddGameToCollection
    @collection_id INT,
    @game_id INT
AS
BEGIN
    -- Check if collection exists
    IF NOT EXISTS (SELECT 1 FROM Collections WHERE collection_id = @collection_id)
    BEGIN
        RAISERROR('Collection not found', 16, 1)
        RETURN
    END

    -- Check if game exists
    IF NOT EXISTS (SELECT 1 FROM OwnedGames WHERE game_id = @game_id)
    BEGIN
        RAISERROR('Game not found', 16, 1)
        RETURN
    END

    -- Check if game is already in collection
    IF EXISTS (SELECT 1 FROM OwnedGames_Collection WHERE collection_id = @collection_id AND game_id = @game_id)
    BEGIN
        RAISERROR('Game is already in collection', 16, 1)
        RETURN
    END

    -- Add game to collection
    INSERT INTO OwnedGames_Collection (collection_id, game_id)
    VALUES (@collection_id, @game_id)
END 


go
CREATE OR ALTER PROCEDURE GetAllOwnedGames
AS
BEGIN
    SELECT game_id, user_id, title, description, cover_picture
    FROM OwnedGames
    ORDER BY title;
END
GO 
CREATE OR ALTER PROCEDURE GetGamesInCollection
    @collection_id INT
AS
BEGIN
    -- Check if collection exists
    IF NOT EXISTS (SELECT 1 FROM Collections WHERE collection_id = @collection_id)
    BEGIN
        RAISERROR('Collection not found', 16, 1)
        RETURN
    END

    -- Get the user_id who owns this collection
    DECLARE @user_id INT
    SELECT @user_id = user_id FROM Collections WHERE collection_id = @collection_id

    -- If this is the "All Owned Games" collection (collection_id = 1)
    IF @collection_id = 1
    BEGIN
        -- Return all games owned by the user (without duplicates)
        SELECT DISTINCT og.game_id, og.user_id, og.title, og.description, og.cover_picture
        FROM OwnedGames og
        WHERE og.user_id = @user_id
        ORDER BY og.title
    END
    ELSE
    BEGIN
        -- For other collections, return games from the collection that belong to the user
        SELECT og.game_id, og.user_id, og.title, og.description, og.cover_picture
        FROM OwnedGames og
        INNER JOIN OwnedGames_Collection ogc ON og.game_id = ogc.game_id
        WHERE ogc.collection_id = @collection_id
        AND og.user_id = @user_id
        ORDER BY og.title
    END
END
GO 
CREATE PROCEDURE GetGamesNotInCollection
    @collection_id INT,
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;

    -- Get all games owned by the user that are not in the specified collection
    SELECT og.game_id,
           og.user_id,
           og.title,
           og.description,
           og.cover_picture
    FROM OwnedGames og
    WHERE og.user_id = @user_id
    AND NOT EXISTS (
        SELECT 1
        FROM OwnedGames_Collection ogc
        WHERE ogc.game_id = og.game_id
        AND ogc.collection_id = @collection_id
    )
    ORDER BY og.title;
END 


go
CREATE OR ALTER PROCEDURE GetOwnedGameById
    @gameId INT
AS
BEGIN
    SELECT game_id, user_id, title, description, cover_picture
    FROM OwnedGames
    WHERE game_id = @gameId
END
GO 
CREATE OR ALTER PROCEDURE RemoveGameFromCollection
    @collection_id INT,
    @game_id INT
AS
BEGIN
    SET NOCOUNT ON;
    DELETE FROM OwnedGames_Collection WHERE collection_id = @collection_id AND game_id = @game_id;
END
GO

GO 

----------------------------- FEATURES --------------------------------
CREATE TABLE Features (
    feature_id INT PRIMARY KEY identity(1,1),
    name NVARCHAR(100) NOT NULL,
    value INT NOT NULL CHECK (value >= 0),
    description NVARCHAR(255),
    type NVARCHAR(50) CHECK (type IN ('frame', 'emoji', 'background', 'pet', 'hat')),
    source NVARCHAR(255) CHECK (source LIKE '%.svg' OR source LIKE '%.png' OR source LIKE '%.jpg')
);

----------------------------- FEATURE_USER --------------------------------
CREATE TABLE Feature_User (
    user_id INT NOT NULL,
    feature_id INT NOT NULL,
    equipped BIT DEFAULT 0,
    PRIMARY KEY (user_id, feature_id),
    FOREIGN KEY (user_id) REFERENCES Users(user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (feature_id) REFERENCES Features(feature_id) ON DELETE CASCADE ON UPDATE CASCADE
);

go
CREATE PROCEDURE GetAllFeatures
    @userId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT f.feature_id, f.name, f.value, f.description, f.type, f.source,
           CASE WHEN fu.equipped = 1 THEN 1 ELSE 0 END as equipped
    FROM Features f
    LEFT JOIN Feature_User fu ON f.feature_id = fu.feature_id 
        AND fu.user_id = @userId
    ORDER BY f.type, f.value DESC;
END

go
CREATE PROCEDURE GetFeaturesByType
    @type NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT feature_id, name, value, description, type, source
    FROM Features
    WHERE type = @type
    ORDER BY value DESC;
END


go
CREATE PROCEDURE CheckFeatureOwnership
    @userId INT,
    @featureId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(1)
    FROM Feature_User
    WHERE user_id = @userId 
    AND feature_id = @featureId;
END

go
CREATE PROCEDURE CheckFeaturePurchase
    @userId INT,
    @featureId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT COUNT(1)
    FROM Feature_User
    WHERE user_id = @userId 
    AND feature_id = @featureId;
END

go
CREATE PROCEDURE EquipFeature
    @userId INT,
    @featureId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    -- Check if the feature-user relationship exists
    IF EXISTS (SELECT 1 FROM Feature_User WHERE user_id = @userId AND feature_id = @featureId)
    BEGIN
        -- Update existing relationship
        UPDATE Feature_User
        SET equipped = 1
        WHERE user_id = @userId AND feature_id = @featureId;
    END
    ELSE
    BEGIN
        -- Create new relationship
        INSERT INTO Feature_User (user_id, feature_id, equipped)
        VALUES (@userId, @featureId, 1);
    END
    
    RETURN 1;
END

go
CREATE PROCEDURE GetAllFeaturesWithOwnership
    @userId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    SELECT 
        f.feature_id,
        f.name,
        f.value,
        f.description,
        f.type,
        f.source,
        CASE WHEN fu.feature_id IS NOT NULL THEN 1 ELSE 0 END as is_owned,
        ISNULL(fu.equipped, 0) as equipped
    FROM Features f
    LEFT JOIN Feature_User fu ON f.feature_id = fu.feature_id 
        AND fu.user_id = @userId
    ORDER BY f.type, f.value DESC;
END

go
-- Procedure to unequip a feature
CREATE PROCEDURE UnequipFeature
    @userId INT,
    @featureId INT
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE Feature_User
    SET equipped = 0
    WHERE user_id = @userId AND feature_id = @featureId;
    
    RETURN 1;
END

go
CREATE PROCEDURE UnequipFeaturesByType
    @userId INT,
    @featureType NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;
    
    UPDATE fu
    SET equipped = 0
    FROM Feature_User fu
    JOIN Features f ON fu.feature_id = f.feature_id
    WHERE fu.user_id = @userId AND f.type = @featureType;
    
    RETURN 1;
END
GO

INSERT INTO Features (name, value, description, type, source) VALUES
('Black Hat', 2000, 'An elegant hat', 'hat', 'Assets/Features/Hats/black-hat.png');

INSERT INTO Features (name, value, description, type, source) VALUES
('Pufu', 10, 'Cute doggo', 'pet', 'Assets/Features/Pets/dog.png');
INSERT INTO Features (name, value, description, type, source) VALUES
('Kitty', 8, 'Cute cat', 'pet', 'Assets/Features/Pets/cat.png');

INSERT INTO Features (name, value, description, type, source) VALUES
('Frame', 5, 'Violet frame', 'frame', 'Assets/Features/Frames/frame1.png');

INSERT INTO Features (name, value, description, type, source) VALUES
('Love Emoji', 7, 'lalal', 'emoji', 'Assets/Features/Emojis/love.png');

INSERT INTO Features (name, value, description, type, source) VALUES
('Violet Background', 7, 'Violet Background', 'background', 'Assets/Features/Backgrounds/violet.jpg');

----------------------------- ACHIEVEMENTS --------------------------------
CREATE TABLE Achievements (
    achievement_id INT PRIMARY KEY identity(1,1),
	achievement_name char(30),
	description char(100),
    achievement_type  NVARCHAR(100) NOT NULL,
    points INT NOT NULL CHECK (points >= 0),
	icon_url NVARCHAR(255) CHECK (icon_url LIKE '%.svg' OR icon_url LIKE '%.png' OR icon_url LIKE '%.jpg')
);

----------------------------- USER ACHIEVEMENTS --------------------------------
CREATE TABLE UserAchievements (
    user_id INT NOT NULL,
    achievement_id INT NOT NULL,
    unlocked_at DATETIME DEFAULT GETDATE(),
    PRIMARY KEY (user_id, achievement_id),
    FOREIGN KEY (user_id) REFERENCES Users (user_id) ON DELETE CASCADE ON UPDATE CASCADE,
    FOREIGN KEY (achievement_id) REFERENCES Achievements(achievement_id) ON DELETE CASCADE ON UPDATE CASCADE
);

go
CREATE or Alter PROCEDURE GetAchievementId
	@type NVARCHAR(50),
	@count int
AS
BEGIN
	IF @type = 'Friendships'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP4';
		ELSE IF @count = 100 SELECT achievement_id FROM Achievements WHERE achievement_name = 'FRIENDSHIP5';
	END
	ELSE IF @type = 'Owned Games'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES3';		
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'OWNEDGAMES4';
	END
	ELSE IF @type = 'Sold Games'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'SOLDGAMES4';
	END
	ELSE IF @type = 'Number of Reviews'
	BEGIN
		IF @count = 1 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW1';
		ELSE IF @count = 5 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW2';
		ELSE IF @count = 10 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW3';
		ELSE IF @count = 50 SELECT achievement_id FROM Achievements WHERE achievement_name = 'REVIEW4';
	END
END


go
CREATE PROCEDURE GetAllAchievements
AS
BEGIN
	SELECT *
    FROM Achievements
    ORDER BY points DESC;
END;

go
CREATE PROCEDURE GetNumberOfOwnedGames
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS NumberOfOwnedGames
    FROM OwnedGames
    WHERE user_id = @user_id;
END;


go
CREATE PROCEDURE GetNumberOfReviews
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS NumberOfOwnedGames
    FROM Reviews
    WHERE user_id = @user_id;
END;

go
CREATE PROCEDURE GetNumberOfSoldGames
    @user_id INT
AS
BEGIN
    SET NOCOUNT ON;
    SELECT COUNT(*) AS NumberOfSoldGames
    FROM SoldGames
    WHERE user_id = @user_id;
END;

go
CREATE PROCEDURE GetUnlockedAchievements
    @userId INT
AS
BEGIN
	SELECT a.achievement_id, a.achievement_name, a.description, a.achievement_type, a.points, a.icon_url, ua.unlocked_at
    FROM Achievements a
    INNER JOIN UserAchievements ua ON a.achievement_id = ua.achievement_id
    WHERE ua.user_id = @userId;
END

go
CREATE PROCEDURE GetUnlockedDataForAchievement
    @user_id INT,
    @achievement_id INT
AS
BEGIN
	SELECT a.achievement_name AS AchievementName, a.description AS AchievementDescription, ua.unlocked_at AS UnlockDate
    FROM UserAchievements ua
    JOIN Achievements a ON ua.achievement_id = a.achievement_id
    WHERE ua.user_id = @user_id AND ua.achievement_id = @achievement_id;
END;

go
CREATE PROCEDURE IsAchievementUnlocked
	@user_id INT,
	@achievement_id INT
AS
BEGIN
	IF EXISTS (
        SELECT 1 FROM UserAchievements 
        WHERE user_id = @user_id 
        AND achievement_id = @achievement_id
    )
        SELECT 1;
	ELSE 
		SELECT 0;
END;

go
CREATE PROCEDURE RemoveAchievement
    @userId INT,
    @achievementId INT
AS
BEGIN
	DELETE FROM UserAchievements
    WHERE user_id = @userId AND achievement_id = @achievementId;
END;

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
go

