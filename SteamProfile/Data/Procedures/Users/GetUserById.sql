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