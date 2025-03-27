CREATE PROCEDURE UserLogin
    @Username NVARCHAR(50),
    @Password NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @UserId INT;
    DECLARE @SessionId UNIQUEIDENTIFIER;

    -- Check if user exists and password matches
    SELECT @UserId = user_id
    FROM Users
    WHERE username = @Username AND password = @Password;

    IF @UserId IS NOT NULL
    BEGIN
        -- Create new session
        SET @SessionId = NEWID();
        
        INSERT INTO UserSessions (session_id, user_id, created_at, expires_at)
        VALUES (@SessionId, @UserId, GETDATE(), DATEADD(HOUR, 24, GETDATE()));

        -- Return user data and session ID
        SELECT 
            u.user_id,
            u.username,
            u.email,
            u.developer,
            u.created_at,
            u.last_login,
            @SessionId AS session_id
        FROM Users u
        WHERE u.user_id = @UserId;

        -- Update last login
        UPDATE Users
        SET last_login = GETDATE()
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
               NULL AS last_login,
               NULL AS session_id;
    END
END 