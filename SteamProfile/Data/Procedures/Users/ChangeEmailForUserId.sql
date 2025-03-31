Create or alter procedure ChangeEmailForUserId @userId int, @newEmail char(50) as
begin
	update Users
	set email = @newEmail 
	where user_id = @userId
end