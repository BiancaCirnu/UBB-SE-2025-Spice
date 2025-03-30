create or alter procedure GetWalletById @wallet_id int as
begin
	select wallet_id, user_id, balance, points from Wallet where @wallet_id = wallet_id
end