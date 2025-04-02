CREATE OR ALTER PROCEDURE AddFriend
    @user_id INT,
    @friend_id INT
AS
BEGIN
    INSERT INTO Friendships (user_id, friend_id)
    VALUES (@user_id, @friend_id);
END
