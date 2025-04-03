USE [issEduarda]
GO

-- First drop all stored procedures
DROP PROCEDURE IF EXISTS CreateUserProfile;
DROP PROCEDURE IF EXISTS GetUserProfileByUserId;
DROP PROCEDURE IF EXISTS UpdateUserProfile;
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

-- Drop indexes
DROP INDEX IF EXISTS IX_Friendships_UserId ON Friendships;
DROP INDEX IF EXISTS IX_Friendships_FriendId ON Friendships;

-- Drop tables in correct order (child tables first)
DROP TABLE IF EXISTS Feature_User;
DROP TABLE IF EXISTS User_Achievement;
DROP TABLE IF EXISTS OwnedGames_Collection;
DROP TABLE IF EXISTS User_Wallet;
DROP TABLE IF EXISTS Friendships;
DROP TABLE IF EXISTS UserProfiles;
DROP TABLE IF EXISTS Wallet;
DROP TABLE IF EXISTS Collections;
DROP TABLE IF EXISTS Features;
DROP TABLE IF EXISTS Achievements;
DROP TABLE IF EXISTS Users; 