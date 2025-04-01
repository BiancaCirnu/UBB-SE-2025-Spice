using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Services;
using SteamProfile.Views;
using System;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public partial class AccountSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string _username;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _password;

        [ObservableProperty]
        private string _currentPassword;

        [ObservableProperty]
        private string _errorMessage;

        private readonly UserService _userService;

        public AccountSettingsViewModel(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [RelayCommand]
        private async Task UpdateUsernameAsync()
        {
            if (ValidateCurrentPassword())
            {
                // Logic to update username
                await ShowSuccessMessage("Username updated successfully.");
            }
        }

        [RelayCommand]
        private async Task UpdateEmailAsync()
        {
            if (ValidateCurrentPassword())
            {
                // Logic to update email
                await ShowSuccessMessage("Email updated successfully.");
            }
        }

        [RelayCommand]
        private async Task UpdatePasswordAsync()
        {
            if (ValidateCurrentPassword())
            {
                // Logic to update password
                await ShowSuccessMessage("Password updated successfully.");
            }
        }

        [RelayCommand]
        private async Task DeleteAccountAsync()
        {
            if (ValidateCurrentPassword())
            {
                // Logic to delete account
                await ShowSuccessMessage("Account deleted.");
            }
        }


        [RelayCommand]
        private void Logout()
        {
            _userService.Logout();
            NavigationService.Instance.Navigate(typeof(LoginPage));
        }

        [RelayCommand]
        private void GoBack()
        {
            // Navigation logic to go back
        }

        private bool ValidateCurrentPassword()
        {
            if (string.IsNullOrWhiteSpace(CurrentPassword))
            {
                ErrorMessage = "Current password is required.";
                return false;
            }
            return true; // Add actual password validation logic
        }

        private async Task ShowSuccessMessage(string message)
        {
            ContentDialog dialog = new()
            {
                Title = "Success",
                Content = message,
                CloseButtonText = "OK"
            };
            await dialog.ShowAsync();
        }
    }
}
