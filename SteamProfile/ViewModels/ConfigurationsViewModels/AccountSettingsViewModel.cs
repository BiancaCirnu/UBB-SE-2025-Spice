using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Services;
using SteamProfile.Views;
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

        // Event to request the View to show the password confirmation dialog
        public event EventHandler RequestPasswordConfirmation;

        private readonly UserService userService;
        private Action pendingAction;
        private readonly Frame _frame;
        public AccountSettingsViewModel(Frame frame)
        {
            userService = App.UserService;
            _frame = frame;
            UpdateEmailCommand = new RelayCommand(UpdateEmail, CanUpdateEmail);
            UpdateUsernameCommand = new RelayCommand(UpdateUsername, CanUpdateUsername);
            UpdatePasswordCommand = new RelayCommand(UpdatePassword, CanUpdatePassword);
            LogoutCommand = new RelayCommand(Logout);
            DeleteAccountCommand = new RelayCommand(DeleteAccount);
            CancelCommand = new RelayCommand(Cancel);

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
            pendingAction = () =>
            {
                if (userService.UpdateUserEmail(Email, CurrentPassword))
                {
                    ErrorMessage = "Email updated successfully!";
                }
                else
                {
                    ErrorMessage = "Failed to update email. Please try again.";
                }
            };

            RequestPasswordConfirmation?.Invoke(this, EventArgs.Empty);
        }

        private void UpdateUsername()
        {
            pendingAction = () =>
            {
                if (userService.UpdateUserUsername(Username, CurrentPassword))
                {
                    ErrorMessage = "Username updated successfully!";
                }
                else
                {
                    ErrorMessage = "Failed to update username. Please try again.";
                }
            };

            RequestPasswordConfirmation?.Invoke(this, EventArgs.Empty);
        }

        private void UpdatePassword()
        {
            pendingAction = () =>
            {
                if (userService.UpdateUserPassword(Password, CurrentPassword))
                {
                    ErrorMessage = "Password updated successfully!";
                    Password = string.Empty; // Clear the password field
                }
                else
                {
                    ErrorMessage = "Failed to update password. Please try again.";
                }
            };

            RequestPasswordConfirmation?.Invoke(this, EventArgs.Empty);
        }

        private void Logout()
        {
            userService.Logout();
            _frame.Navigate(typeof(LoginPage));
        }

        private async void DeleteAccount()
        {
            var dialog = new ContentDialog
            {
                Title = "Delete Account",
                Content = "Are you sure you want to delete your account? This action cannot be undone.",
                PrimaryButtonText = "Delete",
                CloseButtonText = "Cancel"

            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                pendingAction = () =>
                {
                    userService.DeleteUser(userService.GetCurrentUser().UserId);
                    _frame.Navigate(typeof(LoginPage));
                };

                RequestPasswordConfirmation?.Invoke(this, EventArgs.Empty);
            }
        }

        private void Cancel()
        {
            _frame.Navigate(typeof(ProfilePage));
        }

        public bool VerifyPassword(string password)
        {
            return userService.VerifyUserPassword(password);
        }

        public void ExecutePendingAction()
        {
            pendingAction?.Invoke();
            pendingAction = null;
            CurrentPassword = string.Empty;
        }

        public void CancelPendingAction()
        {
            pendingAction = null;
            CurrentPassword = string.Empty;
        }
    }
}