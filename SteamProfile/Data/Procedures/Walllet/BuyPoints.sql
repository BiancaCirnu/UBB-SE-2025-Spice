create or alter procedure BuyPoints @offerId int, @userId int 
as
begin
	update Wallet 
	set points = points + (select numberOfPoints from PointsOffers where offerId = @offerId)
	where user_id = @userId;

	update Wallet
	set balance = balance - (select value from PointsOffers where offerId = @offerId) 
	where user_id = @userId
end