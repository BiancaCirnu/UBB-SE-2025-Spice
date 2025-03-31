go
CREATE PROCEDURE UpdateUser
    @user_id INT,
    @email NVARCHAR(255) = NULL,
    @username NVARCHAR(255) = NULL,
    @profile_picture NVARCHAR(MAX) = NULL,
    @description NVARCHAR(MAX) = NULL,
    @developer BIT = NULL
AS
BEGIN
    -- Check if user exists
    IF NOT EXISTS (SELECT 1 FROM Users WHERE user_id = @user_id)
    BEGIN
        RAISERROR('User not found', 16, 1)
        RETURN
    END

    -- Check if new email already exists (if email is being changed)
    IF @email IS NOT NULL 
        AND EXISTS (SELECT 1 FROM Users WHERE email = @email AND user_id != @user_id)
    BEGIN
        RAISERROR('Email already exists', 16, 1)
        RETURN
    END

    -- Check if new username already exists (if username is being changed)
    IF @username IS NOT NULL 
        AND EXISTS (SELECT 1 FROM Users WHERE username = @username AND user_id != @user_id)
    BEGIN
        RAISERROR('Username already exists', 16, 1)
        RETURN
    END

    -- Update user information
    UPDATE Users
    SET 
        email = ISNULL(@email, email),
        username = ISNULL(@username, username),
        profile_picture = ISNULL(@profile_picture, profile_picture),
        description = ISNULL(@description, description),
        developer = ISNULL(@developer, developer)
    WHERE user_id = @user_id

    -- Return the updated user
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
    WHERE user_id = @user_id
END 