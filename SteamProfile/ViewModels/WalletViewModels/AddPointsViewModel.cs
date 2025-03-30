using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Repositories;
using SteamProfile.Services;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SteamProfile.ViewModels
{
    public class AddPointsViewModel : INotifyPropertyChanged
    {
        private readonly WalletViewModel _walletViewModel;
        private readonly PointsOffersRepository _offersRepository;
        private readonly Frame _navigationFrame;
        private int _userPoints;
        private bool _isProcessing;

        public ObservableCollection<PointsOffer> PointsOffers { get; }
        public ICommand PurchasePointsCommand { get; }

        public int UserPoints
        {
            get => _userPoints;
            private set
            {
                if (_userPoints != value)
                {
                    _userPoints = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsProcessing
        {
            get => _isProcessing;
            private set
            {
                if (_isProcessing != value)
                {
                    _isProcessing = value;
                    OnPropertyChanged();
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public AddPointsViewModel(WalletViewModel walletViewModel, PointsOffersRepository offersRepository, Frame navigationFrame)
        {
            _walletViewModel = walletViewModel ?? throw new ArgumentNullException(nameof(walletViewModel));
            _offersRepository = offersRepository ?? throw new ArgumentNullException(nameof(offersRepository));
            _navigationFrame = navigationFrame ?? throw new ArgumentNullException(nameof(navigationFrame));

            PointsOffers = new ObservableCollection<PointsOffer>(_offersRepository.Offers);
            UserPoints = _walletViewModel.Points;
            IsProcessing = false;

            PurchasePointsCommand = new RelayCommand<PointsOffer>(BuyPoints, CanBuyPoints);
        }

        private bool CanBuyPoints(PointsOffer offer)
        {
            return offer != null && !IsProcessing;
        }

        private async void BuyPoints(PointsOffer offer)
        {
            if (offer == null)
                return;

            IsProcessing = true;

            try
            {
                bool success = await _walletViewModel.AddPoints(offer);

                if (success)
                {
                    // Update the local UserPoints to reflect the wallet points
                    UserPoints = _walletViewModel.Points;

                    // Show success message
                    await ShowSuccessMessageAsync(offer);

                    // Navigate back to previous page
                    NavigateBack();
                }
                else
                {
                    // Show error message if purchase failed
                    await ShowErrorMessageAsync("Insufficient funds to purchase these points.");
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessageAsync($"An error occurred: {ex.Message}");
            }
            finally
            {
                IsProcessing = false;
            }
        }

        private async Task ShowSuccessMessageAsync(PointsOffer offer)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Points Added",
                Content = $"Successfully added {offer.Points} points to your wallet!",
                CloseButtonText = "OK",
                XamlRoot = _navigationFrame.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private async Task ShowErrorMessageAsync(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = _navigationFrame.XamlRoot
            };

            await dialog.ShowAsync();
        }

        private void NavigateBack()
        {
            if (_navigationFrame.CanGoBack)
            {
                _navigationFrame.GoBack();
            }
        }

        // Method to refresh points display when the view becomes active
        public void RefreshPoints()
        {
            UserPoints = _walletViewModel.Points;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}