CREATE PROCEDURE CreateUserSession
    @user_id INT,
    @session_id INT OUTPUT
AS
BEGIN
    SET NOCOUNT ON;

    SET @session_id = NEWID(); -- Generate a unique session ID

    INSERT INTO UserSessions (user_id, expires_at)
    VALUES (@user_id, DATEADD(HOUR, 2, GETDATE())); -- Session expires in 2 hours

    SELECT @session_id AS session_id;
END;
