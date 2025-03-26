CREATE PROCEDURE GetUserById
    @userId INT
AS
BEGIN
    SELECT 
        user_id,
        email,
        username,
        profile_picture,
        description,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = @userId
END 