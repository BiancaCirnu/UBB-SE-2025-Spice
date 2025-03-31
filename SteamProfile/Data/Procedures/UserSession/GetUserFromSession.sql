CREATE PROCEDURE GetUserFromSession
    @session_id UNIQUEIDENTIFIER
AS
BEGIN
    SET NOCOUNT ON;

    -- Check if session exists and is not expired
    IF EXISTS (
        SELECT 1 
        FROM UserSessions 
        WHERE session_id = @session_id 
        AND expires_at > GETDATE()
    )
    BEGIN
        -- Return user details
        SELECT 
            u.user_id,
            u.username,
            u.email,
            u.developer,
            u.created_at,
            u.last_login
        FROM UserSessions us
        JOIN Users u ON us.user_id = u.user_id
        WHERE us.session_id = @session_id;
    END
    ELSE
    BEGIN
        -- If session is expired or doesn't exist, delete it
        DELETE FROM UserSessions WHERE session_id = @session_id;
    END
END; 