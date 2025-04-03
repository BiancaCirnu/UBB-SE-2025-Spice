CREATE PROCEDURE UpdateUserProfile
    @profile_id INT,
    @user_id INT,
    @profile_picture NVARCHAR(255),
    @bio NVARCHAR(1000)
 
AS
BEGIN
    SET NOCOUNT ON;

    UPDATE UserProfiles
    SET 
        profile_picture = @profile_picture,
        bio = @bio,
     
        last_modified = GETDATE()
    WHERE profile_id = @profile_id AND user_id = @user_id;

    SELECT 
        profile_id,
        user_id,
        profile_picture,
        bio,

        last_modified
    FROM UserProfiles
    WHERE profile_id = @profile_id;
END; 