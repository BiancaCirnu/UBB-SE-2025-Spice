CREATE PROCEDURE CreateUser
    @username NVARCHAR(50),
    @email NVARCHAR(100),
    @password_hash NVARCHAR(255),
    @developer BIT
AS
BEGIN
    INSERT INTO Users (username, email, password_hash, developer)
    VALUES (@username, @email, @password_hash, @developer);

    SELECT 
        user_id,
        username,
        email,
        password_hash,
        developer,
        created_at,
        last_login
    FROM Users
    WHERE user_id = SCOPE_IDENTITY();
END;
