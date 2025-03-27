CREATE PROCEDURE CreateUser
    @username NVARCHAR(50),
    @email NVARCHAR(100),
    @hashed_password NVARCHAR(255),
    @developer BIT,
    @created_at DATETIME
AS
BEGIN
    INSERT INTO Users (username, email, hashed_password, developer, created_at, last_login)
    VALUES (@username, @email, @hashed_password, @developer, @created_at, NULL);

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
