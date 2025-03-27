CREATE PROCEDURE CheckUserExists
    @email NVARCHAR(100),
    @username NVARCHAR(50)
AS
BEGIN
    SET NOCOUNT ON;

    -- Check for existing email and username
    SELECT 
        CASE 
            WHEN EXISTS (SELECT 1 FROM Users WHERE Email = @email) THEN 'Email already exists'
            WHEN EXISTS (SELECT 1 FROM Users WHERE Username = @username) THEN 'Username already exists'
            ELSE NULL
        END AS ErrorMessage;
END;
