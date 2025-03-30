using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class WalletService
    {
        private readonly WalletRepository _walletRepository;
        private readonly int _userId;

        public WalletService(WalletRepository walletRepository, int userId)
        {
            _walletRepository = walletRepository ?? throw new ArgumentNullException(nameof(walletRepository));
            _userId = userId;
        }

        internal void AddMoney(decimal amount)
        {
            _walletRepository.AddMoneyToWallet(amount);
        }

        internal void AddPoints(int points)
        {
            _walletRepository.AddPointsToWallet(points);
        }

        internal decimal GetBalance()
        {
            return _walletRepository.GetMoneyFromWallet();
        }

        internal int GetPoints()
        {
           return (_walletRepository.GetPointsFromWallet());
        }


        public void PurchasePoints(PointsOffer offer)
        {
            if (offer == null)
                throw new ArgumentNullException(nameof(offer));

            // Check if user has enough balance
            if (GetBalance() < offer.Price)
                throw new InvalidOperationException("Insufficient funds");
            _walletRepository.PurchasePoints(offer);
        }
    }
}
