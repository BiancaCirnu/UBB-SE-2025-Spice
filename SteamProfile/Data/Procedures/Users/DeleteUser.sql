CREATE or alter PROCEDURE DeleteUser
    @user_id INT
AS
BEGIN

	exec DeleteFriendshipsForUser @user_id =@user_id
    DELETE FROM Users
    WHERE user_id = @user_id;
END 

