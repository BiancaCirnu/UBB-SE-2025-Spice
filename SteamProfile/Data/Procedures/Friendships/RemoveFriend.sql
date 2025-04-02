CREATE OR ALTER PROCEDURE RemoveFriend
    @friendship_id INT
AS
BEGIN
    DELETE FROM Friendships
    WHERE friendship_id = @friendship_id;
END