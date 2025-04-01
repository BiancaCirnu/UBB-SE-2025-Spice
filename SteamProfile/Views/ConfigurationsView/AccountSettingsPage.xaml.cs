using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels.ConfigurationsViewModels;
using System;
using System.Threading.Tasks;
namespace SteamProfile.Views.ConfigurationsView
{
    public sealed partial class AccountSettingsPage : Page
    {
        public AccountSettingsViewModel ViewModel { get; private set; }
        public AccountSettingsPage()
        {
            this.InitializeComponent();
            ViewModel = new AccountSettingsViewModel(this.Frame);
            ViewModel.RequestPasswordConfirmation += ViewModel_RequestPasswordConfirmation;
            DataContext = ViewModel;
        }

        private async void ViewModel_RequestPasswordConfirmation(object sender, EventArgs e)
        {
            await ShowPasswordConfirmationDialog();
        }

        private void PasswordConfirmationDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string password = ConfirmPasswordBox.Password;
            if (string.IsNullOrEmpty(password))
            {
                args.Cancel = true;
                ViewModel.PasswordConfirmationError = "Password cannot be empty";
                ViewModel.PasswordConfirmationErrorVisibility = Visibility.Visible;
                return;
            }

            if (!ViewModel.VerifyPassword(password))
            {
                args.Cancel = true;
                ViewModel.PasswordConfirmationError = "Incorrect password";
                ViewModel.PasswordConfirmationErrorVisibility = Visibility.Visible;
                return;
            }

            ViewModel.CurrentPassword = password;
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;

            ViewModel.ExecutePendingAction();
        }

        private void PasswordConfirmationDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;

            ConfirmPasswordBox.Password = string.Empty;

            ViewModel.CancelPendingAction();
        }

        public async Task<ContentDialogResult> ShowPasswordConfirmationDialog()
        {
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;
            ConfirmPasswordBox.Password = string.Empty;

            return await PasswordConfirmationDialog.ShowAsync();
        }
    }
}