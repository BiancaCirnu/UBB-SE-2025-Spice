using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public partial class AccountSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private string username = string.Empty;

        [ObservableProperty]
        private string email = string.Empty;

        [ObservableProperty]
        private string password = string.Empty;

        [ObservableProperty]
        private string currentPassword = string.Empty;

        [ObservableProperty]
        private string errorMessage = string.Empty;

        [ObservableProperty]
        private string passwordConfirmationError = string.Empty;

        [ObservableProperty]
        private Visibility emailErrorMessageVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility passwordErrorMessageVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility usernameErrorMessageVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private Visibility passwordConfirmationErrorVisibility = Visibility.Collapsed;

        [ObservableProperty]
        private bool updateEmailEnabled = false;

        [ObservableProperty]
        private bool updateUsernameEnabled = false;

        [ObservableProperty]
        private bool updatePasswordEnabled = false;

        private readonly UserService userService;

        public AccountSettingsViewModel()
        {
            userService = App.UserService;

            UpdateEmailCommand = new RelayCommand(UpdateEmail, CanUpdateEmail);
            UpdateUsernameCommand = new RelayCommand(UpdateUsername, CanUpdateUsername);
            UpdatePasswordCommand = new RelayCommand(UpdatePassword, CanUpdatePassword);
            LogoutCommand = new RelayCommand(Logout);
            DeleteAccountCommand = new RelayCommand(DeleteAccount);
            CancelCommand = new RelayCommand(Cancel);

            // Load current user data
            var currentUser = userService.GetCurrentUser();
            if (currentUser != null)
            {
                username = currentUser.Username;
                email = currentUser.Email;
            }
        }

        partial void OnPasswordChanged(string value)
        {
            ValidatePassword(value);
        }

        private void ValidatePassword(string password)
        {
            bool isValid = IsValidPassword(password);
            PasswordErrorMessageVisibility = isValid ? Visibility.Collapsed : Visibility.Visible;
            UpdatePasswordEnabled = isValid;
        }

        private bool IsValidPassword(string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\W).{8,}$");
            return regex.IsMatch(password);
        }

        partial void OnEmailChanged(string value)
        {
            ValidateEmail(value);
        }

        private void ValidateEmail(string email)
        {
            bool isValid = IsValidEmail(email);
            EmailErrorMessageVisibility = isValid ? Visibility.Collapsed : Visibility.Visible;
            UpdateEmailEnabled = isValid;
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email)) return false;
            var regex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
            return regex.IsMatch(email) && userService.GetUserByEmail(email) == null;
        }

        partial void OnUsernameChanged(string value)
        {
            ValidateUsername(value);
        }

        private void ValidateUsername(string username)
        {
            bool isValid = IsValidUsername(username);
            UsernameErrorMessageVisibility = isValid ? Visibility.Collapsed : Visibility.Visible;
            UpdateUsernameEnabled = isValid;
        }

        private bool IsValidUsername(string username)
        {
            if (string.IsNullOrEmpty(username)) return false;
            return userService.GetUserByUsername(username) == null; // Username should be unique
        }

        public IRelayCommand UpdateEmailCommand { get; }
        public IRelayCommand UpdateUsernameCommand { get; }
        public IRelayCommand UpdatePasswordCommand { get; }
        public IRelayCommand LogoutCommand { get; }
        public IRelayCommand DeleteAccountCommand { get; }
        public IRelayCommand CancelCommand { get; }

        public bool CanUpdateEmail() => !string.IsNullOrEmpty(Email) && IsValidEmail(Email);
        public bool CanUpdateUsername() => !string.IsNullOrEmpty(Username) && IsValidUsername(Username);
        public bool CanUpdatePassword() => !string.IsNullOrEmpty(Password) && IsValidPassword(Password);

        private void UpdateEmail()
        {
            // Show password confirmation dialog
            var dialog = new ContentDialog
            {
                Title = "Confirm Password",
                PrimaryButtonText = "Confirm",
                SecondaryButtonText = "Cancel",
                Content = "Please enter your current password to confirm changes"
            };

            // Handle confirmation
            // In actual implementation, you would verify the password and update the email
            if (userService.UpdateUserEmail(Email, CurrentPassword))
            {
                // Show success message
            }
            else
            {
                // Show error message
            }
        }

        private void UpdateUsername()
        {
            // Show password confirmation dialog
            // Handle confirmation
            // Update username in the user service
            if (userService.UpdateUserUsername(Username, CurrentPassword))
            {
                // Show success message
            }
            else
            {
                // Show error message
            }
        }

        private void UpdatePassword()
        {
            // Show password confirmation dialog
            // Handle confirmation
            // Update password in the user service
            if (userService.UpdateUserPassword(Password, CurrentPassword))
            {
                // Show success message
                Password = string.Empty; // Clear the password field
            }
            else
            {
                // Show error message
            }
        }

        private void Logout()
        {
            userService.Logout();
            // Navigate to login page
            // App.NavigationService.Navigate(typeof(LoginPage));
        }

        private void DeleteAccount()
        {
            // Show confirmation dialog
            var dialog = new ContentDialog
            {
                Title = "Delete Account",
                Content = "Are you sure you want to delete your account? This action cannot be undone.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"
            };

            // Handle confirmation
            // Delete account in the user service
            // Navigate to login page
        }

        private void Cancel()
        {
            // Navigate back or to home page
            // App.NavigationService.GoBack();
        }

        public bool VerifyPassword(string password)
        {
            return userService.VerifyUserPassword(password);
        }
    }
}