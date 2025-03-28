CREATE PROCEDURE UserLogin
    @EmailOrUsername NVARCHAR(100),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @HashedPassword NVARCHAR(100);
    DECLARE @UserId INT;

    -- Get the hashed password for the user
    SELECT @HashedPassword = hashed_password, @UserId = user_id
    FROM Users
    WHERE username = @EmailOrUsername OR email = @EmailOrUsername;

    -- Check if the user exists and the password matches
    IF @UserId IS NOT NULL AND @HashedPassword = @Password
    BEGIN
        -- Return user data
        SELECT user_id, username, email, developer, created_at, last_login
        FROM Users
        WHERE user_id = @UserId;
    END
    ELSE
    BEGIN
        -- Return NULL if login failed
        SELECT NULL AS user_id,
               NULL AS username,
               NULL AS email,
               NULL AS developer,
               NULL AS created_at,
               NULL AS last_login;
    END
END 