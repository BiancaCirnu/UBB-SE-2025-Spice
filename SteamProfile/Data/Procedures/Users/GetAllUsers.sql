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