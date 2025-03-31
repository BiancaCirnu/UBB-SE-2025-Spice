using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public partial class AccountSettingsViewModel : ObservableObject
    {
        [ObservableProperty]
        private bool isEmailValid;
        [ObservableProperty]
        private bool isPasswordValid;
        [ObservableProperty] 
        private bool isUsernameValid;

        [ObservableProperty]
        private bool showEmailError;
        [ObservableProperty]
        private bool showPasswordError;
        [ObservableProperty] private bool _showUsernameError;

        [ObservableProperty]
        private string emailErrorMessageStatus = "Invalid email format";
        [ObservableProperty] 
        private string usernameErrorMessageStatus = "Username cannot be empty";
        [ObservableProperty] 
        private string passwordErrorMessageStatus = "Password must be at least 8 characters with uppercase, lowercase, digit and special character";

        [ObservableProperty] 
        private string email = string.Empty;
        [ObservableProperty] 
        private string password = string.Empty;
        [ObservableProperty] 
        private string username = string.Empty;
        public Visibility EmailErrorMessageVisibility => ShowEmailError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PasswordErrorMessageVisibility => ShowPasswordError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility UsernameErrorMessageVisibility => ShowUsernameError ? Visibility.Visible : Visibility.Collapsed;

        public AccountSettingsViewModel()
        {
            ShowEmailError = false;
            ShowPasswordError = false;
            ShowUsernameError = false;
            ValidateAllFields();
        }

        partial void OnShowEmailErrorChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(nameof(EmailErrorMessageVisibility));
        }

        partial void OnShowPasswordErrorChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(nameof(PasswordErrorMessageVisibility));
        }

        partial void OnShowUsernameErrorChanged(bool oldValue, bool newValue)
        {
            OnPropertyChanged(nameof(UsernameErrorMessageVisibility));
        }

        public void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                IsUsernameValid = false;
                UsernameErrorMessageStatus = "Username cannot be empty";
            }
            else if (username.Length < 3)
            {
                IsUsernameValid = false;
                UsernameErrorMessageStatus = "Username must be at least 3 characters";
            }
            else
            {
                IsUsernameValid = true;
            }
            ShowUsernameError = !IsUsernameValid;
        }

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                IsEmailValid = false;
                EmailErrorMessageStatus = "Email cannot be empty";
            }
            else
            {
                IsEmailValid = Regex.IsMatch(email, @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$");
                if (!IsEmailValid)
                {
                    EmailErrorMessageStatus = "Invalid email format";
                }
            }
            ShowEmailError = !IsEmailValid;
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                IsPasswordValid = false;
                PasswordErrorMessageStatus = "Password cannot be empty";
            }
            else
            {
                IsPasswordValid = Regex.IsMatch(password, @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
                if (!IsPasswordValid)
                {
                    PasswordErrorMessageStatus = "Password must be at least 8 characters with uppercase, lowercase, digit and special character";
                }
            }
            ShowPasswordError = !IsPasswordValid;
        }

        public async Task UpdatePasswordAsync()
        {
            ValidatePassword(Password);
            if (IsPasswordValid)
            {
                await Task.Delay(500);
            }
        }

        public async Task UpdateUsernameAsync()
        {
            ValidateUsername(Username);
            if (IsUsernameValid)
            {
                await Task.Delay(500);
            }
        }

        public async Task UpdateEmailAsync()
        {
            ValidateEmail(Email);
            if (IsEmailValid)
            {
                await Task.Delay(500);
            }
        }

        public bool ValidateAllFields()
        {
            ValidateEmail(Email);
            ValidateUsername(Username);
            ValidatePassword(Password);

            return IsEmailValid && IsUsernameValid && IsPasswordValid;
        }

    }
}