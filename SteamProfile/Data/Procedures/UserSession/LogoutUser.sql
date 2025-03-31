CREATE PROCEDURE LogoutUser
    @session_id INT
AS
BEGIN
    DELETE FROM UserSessions WHERE session_id = @session_id;
END;
