create or alter procedure GetUserByEmail @email char(50)
as
begin
	select User_id from Users
	where @email = email
end
