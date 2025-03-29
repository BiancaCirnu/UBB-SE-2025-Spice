create or alter procedure AddMoney @amount int, @userId int as
begin 
	update wallet  
	set balance = balance + @amount
	where user_id = @userId
end