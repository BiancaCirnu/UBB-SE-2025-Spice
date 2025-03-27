CREATE PROCEDURE LoginUser
    @email_or_username NVARCHAR(100)
AS
BEGIN
    SET NOCOUNT ON;

    -- Get user data including password hash
    SELECT user_id, email, username, hashed_password, developer
    FROM Users
    WHERE email = @email_or_username OR username = @email_or_username;
END;
