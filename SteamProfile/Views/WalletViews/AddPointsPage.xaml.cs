using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.Repositories;
using SteamProfile.ViewModels;
using System;

namespace SteamProfile.Views.WalletViews
{
    public sealed partial class AddPointsPage : Page
    {
        private AddPointsViewModel _viewModel;

        public AddPointsPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            // Check if navigation parameters contain the WalletViewModel
            if (e.Parameter is WalletViewModel walletViewModel)
            {
                // Create repository
                var repository = new PointsOffersRepository();

                // Create the ViewModel with navigation frame
                _viewModel = new AddPointsViewModel(walletViewModel, repository, this.Frame);

                // Set the DataContext
                this.DataContext = _viewModel;
            }
            else
            {
                // Handle error - no wallet view model provided
                ShowErrorAndGoBack("Navigation error: Wallet data not found.");
            }
        }

        private async void ShowErrorAndGoBack(string message)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Error",
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await dialog.ShowAsync();

            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }

        private void CancelAddPoints(object sender, RoutedEventArgs e)
        {
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}