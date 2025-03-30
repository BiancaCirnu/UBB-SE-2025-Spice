using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.Models;
using SteamProfile.ViewModels;
using System.Collections.Generic;

namespace SteamProfile.Views.WalletViews
{
    /// <summary>
    /// Payment page for PayPal transactions
    /// </summary>
    public sealed partial class PaypalPaymentPage : Page
    {
        public PaypalPaymentViewModel ViewModel { get; } = new PaypalPaymentViewModel();

        public PaypalPaymentPage()
        {
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            if (e.Parameter is Dictionary<string, object> parameters)
            {
                ViewModel.Initialize(parameters);
            }
        }

        private void EmailTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Validation is automatically triggered by the property setter
        }

        private void PasswordText_PasswordChanged(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidatePassword(PasswordText.Password);
        }

        private async void AddMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            if (await ViewModel.ProcessPaymentAsync())
            {
                Frame.GoBack();
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
    }
}