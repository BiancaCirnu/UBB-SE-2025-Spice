using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels.ConfigurationsViewModels;
using System;

namespace SteamProfile.Views.ConfigurationsView
{
    public sealed partial class AccountSettingsPage : Page
    {
        public AccountSettingsViewModel ViewModel { get; private set; }

        public AccountSettingsPage()
        {
            InitializeComponent();
            ViewModel = new AccountSettingsViewModel();
            DataContext = ViewModel;
        }

        private void PasswordConfirmationDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            string password = ConfirmPasswordBox.Password;

            if (string.IsNullOrEmpty(password))
            {
                // Prevent dialog from closing
                args.Cancel = true;
                ViewModel.PasswordConfirmationError = "Password cannot be empty";
                ViewModel.PasswordConfirmationErrorVisibility = Visibility.Visible;
                return;
            }

            // Verify password
            if (!ViewModel.VerifyPassword(password))
            {
                // Prevent dialog from closing
                args.Cancel = true;
                ViewModel.PasswordConfirmationError = "Incorrect password";
                ViewModel.PasswordConfirmationErrorVisibility = Visibility.Visible;
                return;
            }

            // Password is correct, set it to the ViewModel
            ViewModel.CurrentPassword = password;
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;
        }

        private void PasswordConfirmationDialog_SecondaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
            // Clear any error message
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;
            // Clear the password box
            ConfirmPasswordBox.Password = string.Empty;
        }

        // Method to show the password confirmation dialog
        public async void ShowPasswordConfirmationDialog()
        {
            // Reset error state
            ViewModel.PasswordConfirmationErrorVisibility = Visibility.Collapsed;
            ConfirmPasswordBox.Password = string.Empty;

            // Show dialog
            await PasswordConfirmationDialog.ShowAsync();
        }
    }
}