using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Repositories;
using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class WalletViewModel : INotifyPropertyChanged
    {
        private readonly WalletService _walletService;
        private readonly PointsOffersRepository _pointsOffersRepository;

        private decimal _balance;
        private int _points;
        private int _walletId;

        public List<PointsOffer> PointsOffers { get; set; }
        public event PropertyChangedEventHandler PropertyChanged;

        public decimal Balance
        {
            get => _balance;
            private set
            {
                if (_balance != value)
                {
                    _balance = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(BalanceText));
                }
            }
        }

        public int Points
        {
            get => _points;
            private set
            {
                if (_points != value)
                {
                    _points = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PointsText));
                }
            }
        }

        public string BalanceText => $"${Balance:F2}";

        public string PointsText => $"{Points} pts";

        public WalletViewModel(WalletService walletService)
        {
            _walletService = walletService ?? throw new ArgumentNullException(nameof(walletService));
            _pointsOffersRepository = new PointsOffersRepository();

            PointsOffers = _pointsOffersRepository.Offers;

            RefreshWalletData();
        }

        public void RefreshWalletData()
        {
            Balance = _walletService.GetBalance();
            Points = _walletService.GetPoints();
        }

        public void AddFunds(decimal amount)
        {
            if (amount <= 0)
                return;

            _walletService.AddMoney(amount);
            RefreshWalletData();
        }

        public async Task<bool> AddPoints(PointsOffer offer)
        {
            if (offer == null)
                return false;

            // Check if user has enough balance to purchase the points
            if (Balance >= offer.Price)
            {
                try
                {
                    // Use the service to handle the purchase transaction
                    _walletService.PurchasePoints(offer);

                    // Refresh wallet data after purchase
                    RefreshWalletData();
                    return true;
                }
                catch (Exception)
                {
                    // Return false if any exception occurs during the purchase
                    return false;
                }
            }

            return false;
        }

        public void AddPoints(int points)
        {
            if (points <= 0)
                return;

            _walletService.AddPoints(points);
            RefreshWalletData();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}