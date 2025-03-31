create or alter procedure WinPoints @amount decimal, @userId int 
as 
begin
	update  Wallet 
	set balance = balance - @amount
	where user_id = @userId
end