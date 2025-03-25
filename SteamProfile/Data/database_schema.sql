-- Drop tables in reverse order of dependencies
DROP TABLE IF EXISTS Feature_User;
DROP TABLE IF EXISTS User_Achievement;
DROP TABLE IF EXISTS OwnedGames_Collection;
DROP TABLE IF EXISTS User_Wallet;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS Collections;
DROP TABLE IF EXISTS Features;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS Users;


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