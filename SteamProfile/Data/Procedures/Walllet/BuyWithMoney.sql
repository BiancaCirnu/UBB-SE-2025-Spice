create or alter procedure WinPoints @amount int, @userId int 
as 
begin
	update  Wallet 
	set balance = balance - @amount
	where user_id = @userId
end