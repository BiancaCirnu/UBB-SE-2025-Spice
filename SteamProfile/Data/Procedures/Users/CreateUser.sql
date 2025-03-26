CREATE PROCEDURE CreateUser
    @email NVARCHAR(255),
    @username NVARCHAR(255),
    @password NVARCHAR(255),
    @profile_picture NVARCHAR(MAX) = NULL,
    @description NVARCHAR(MAX) = NULL,
    @developer BIT = 0
AS
BEGIN
    -- Check if email already exists
    IF EXISTS (SELECT 1 FROM Users WHERE email = @email)
    BEGIN
        RAISERROR('Email already exists', 16, 1)
        RETURN
    END

    -- Check if username already exists
    IF EXISTS (SELECT 1 FROM Users WHERE username = @username)
    BEGIN
        RAISERROR('Username already exists', 16, 1)
        RETURN
    END

    -- Insert new user
    INSERT INTO Users (
        email,
        username,
        password,
        profile_picture,
        description,
        developer,
        created_at
    )
    VALUES (
        @email,
        @username,
        @password,
        @profile_picture,
        @description,
        @developer,
        GETDATE()
    )

    -- Return the newly created user
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
    WHERE email = @email
END 