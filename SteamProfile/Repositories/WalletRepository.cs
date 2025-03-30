using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace SteamProfile.Repositories
{
    public class WalletRepository
    {
        private readonly DataLink _dataLink;
        private readonly int _walletId;

        public WalletRepository(DataLink datalink, int walletId)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
            _walletId = walletId;
        }

        public Wallet GetWallet()
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@wallet_id", _walletId)
                };

                var dataTable = _dataLink.ExecuteReader("GetWalletById", parameters);
                return MapDataRowToUser(dataTable.Rows[0]);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve wallet with ID {_walletId} from the database.", ex);
            }
        }

        private Wallet MapDataRowToUser(DataRow dataRow)
        {
            return new Wallet
            {
                WalletId = Convert.ToInt32(dataRow["wallet_id"]),
                UserId = Convert.ToInt32(dataRow["user_id"]),
                balance = Convert.ToInt32(dataRow["balance"]),
                points = Convert.ToInt32(dataRow["points"])
            };
        }

        public void AddMoneyToWallet(decimal amount)
        {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@amount",  amount),
                new SqlParameter("@userId", _walletId)
            };
            _dataLink.ExecuteReader("AddMoney", parameters);
        }

        public void AddPointsToWallet(int amount) {
            SqlParameter[] parameters = new SqlParameter[]
            {
                new SqlParameter("@amount",  amount),
                new SqlParameter("@userId", _walletId)
            };
            _dataLink.ExecuteReader("AddPoints", parameters);
        }

        public decimal GetMoneyFromWallet() {
            return GetWallet().balance; 
        }

        public int GetPointsFromWallet()
        {
            return GetWallet().points;
        }

        public void PurchasePoints(PointsOffer offer)
        {
            SqlParameter[] parameters = new SqlParameter[]
           {
               new SqlParameter("@price", offer.Price),
               new SqlParameter("@@numberOfPoints", offer.Points),
               new SqlParameter("@userId", _walletId)
           };
            _dataLink.ExecuteReader("BuyPoints", parameters);

        }

    }
}
