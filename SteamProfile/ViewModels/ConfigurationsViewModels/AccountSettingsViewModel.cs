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
    public class AccountSettingsViewModel : INotifyPropertyChanged
    {
        private bool _isEmailValid;
        private bool _isPasswordValid;
        private bool _isUsernameValid;

        private bool _showEmailError;
        private bool _showPasswordError;
        private bool _showUsernameError;

        private string _emailErrorMessageStatus = "Invalid email format";
        private string _usernameErrorMessageStatus = "Username cannot be empty";
        private string _passwordErrorMessageStatus = "Password must be at least 8 characters with uppercase, lowercase, digit and special character";

        private string _email = string.Empty;
        private string _password = string.Empty;
        private string _username = string.Empty;

        public string Email
        {
            get => _email;
            set
            {
                if (_email != value)
                {
                    _email = value;
                    ValidateEmail(_email);
                    OnPropertyChanged();
                }
            }
        }

        public string Username
        {
            get => _username;
            set
            {
                if (_username != value)
                {
                    _username = value;
                    ValidateUsername(_username);
                    OnPropertyChanged();
                }
            }
        }

        public string Password
        {
            get => _password;
            set
            {
                if (_password != value)
                {
                    _password = value;
                    ValidatePassword(_password);
                    OnPropertyChanged();
                }
            }
        }

        public bool IsEmailValid
        {
            get => _isEmailValid;
            set
            {
                if (_isEmailValid != value)
                {
                    _isEmailValid = value;
                    OnPropertyChanged();
                    UpdateEmailErrorVisibility();
                }
            }
        }

        private void UpdateEmailErrorVisibility()
        {
            ShowEmailError = !IsEmailValid;
        }

        public bool IsUsernameValid
        {
            get => _isUsernameValid;
            set
            {
                if (_isUsernameValid != value)
                {
                    _isUsernameValid = value;
                    OnPropertyChanged();
                    UpdateUsernameErrorVisibility();
                }
            }
        }

        private void UpdateUsernameErrorVisibility()
        {
            ShowUsernameError = !IsUsernameValid;
        }

        public bool IsPasswordValid
        {
            get => _isPasswordValid;
            private set
            {
                if (_isPasswordValid != value)
                {
                    _isPasswordValid = value;
                    OnPropertyChanged();
                    UpdatePasswordErrorVisibility();
                }
            }
        }

        private void UpdatePasswordErrorVisibility()
        {
            ShowPasswordError = !IsPasswordValid;
        }

        public bool ShowEmailError
        {
            get => _showEmailError;
            private set
            {
                if (_showEmailError != value)
                {
                    _showEmailError = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(EmailErrorMessageVisibility));
                }
            }
        }

        public bool ShowUsernameError
        {
            get => _showUsernameError;
            private set
            {
                if (_showUsernameError != value)
                {
                    _showUsernameError = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(UsernameErrorMessageVisibility));
                }
            }
        }

        public bool ShowPasswordError
        {
            get => _showPasswordError;
            private set
            {
                if (_showPasswordError != value)
                {
                    _showPasswordError = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PasswordErrorMessageVisibility));
                }
            }
        }

        public string EmailMessageErrorStatus
        {
            get => _emailErrorMessageStatus;
            set
            {
                if (_emailErrorMessageStatus != value)
                {
                    _emailErrorMessageStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string UsernameMessageErrorStatus
        {
            get => _usernameErrorMessageStatus;
            set
            {
                if (_usernameErrorMessageStatus != value)
                {
                    _usernameErrorMessageStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public string PasswordMessageErrorStatus
        {
            get => _passwordErrorMessageStatus;
            set
            {
                if (_passwordErrorMessageStatus != value)
                {
                    _passwordErrorMessageStatus = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility EmailErrorMessageVisibility => ShowEmailError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility PasswordErrorMessageVisibility => ShowPasswordError ? Visibility.Visible : Visibility.Collapsed;
        public Visibility UsernameErrorMessageVisibility => ShowUsernameError ? Visibility.Visible : Visibility.Collapsed;

        // INotifyPropertyChanged Implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public AccountSettingsViewModel()
        {
            // Initialize with validation state
            ShowEmailError = false;
            ShowPasswordError = false;
            ShowUsernameError = false;

            // Validate initial values
            ValidateEmail(_email);
            ValidateUsername(_username);
            ValidatePassword(_password);
        }

        // Validations
        public void ValidateUsername(string username)
        {
            if (string.IsNullOrEmpty(username))
            {
                IsUsernameValid = false;
                UsernameMessageErrorStatus = "Username cannot be empty";
            }
            else if (username.Length < 3)
            {
                IsUsernameValid = false;
                UsernameMessageErrorStatus = "Username must be at least 3 characters";
            }
            else
            {
                IsUsernameValid = true;
            }
        }

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                IsEmailValid = false;
                EmailMessageErrorStatus = "Email cannot be empty";
                return;
            }

            string emailRegexString = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Regex emailRegex = new Regex(emailRegexString);

            IsEmailValid = emailRegex.IsMatch(email);
            if (!IsEmailValid)
            {
                EmailMessageErrorStatus = "Invalid email format";
            }
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                IsPasswordValid = false;
                PasswordMessageErrorStatus = "Password cannot be empty";
                return;
            }

            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex passwordRegex = new Regex(pattern);

            IsPasswordValid = passwordRegex.IsMatch(password);
            if (!IsPasswordValid)
            {
                PasswordMessageErrorStatus = "Password must be at least 8 characters with uppercase, lowercase, digit and special character";
            }
        }

        // Methods to update account settings
        internal async Task UpdatePassword()
        {
            ValidatePassword(Password);
            if (IsPasswordValid)
            {
                // Implement actual password update logic here
                await Task.Delay(500); // Simulating API call
            }
        }

        internal async Task UpdateUsername()
        {
            ValidateUsername(Username);
            if (IsUsernameValid)
            {
                // Implement actual username update logic here
                await Task.Delay(500); // Simulating API call
            }
        }

        internal async Task UpdateEmail()
        {
            ValidateEmail(Email);
            if (IsEmailValid)
            {
                // Implement actual email update logic here
                await Task.Delay(500); // Simulating API call
            }
        }

        // Helper method to validate all fields at once
        public bool ValidateAllFields()
        {
            ValidateEmail(Email);
            ValidateUsername(Username);
            ValidatePassword(Password);

            return IsEmailValid && IsUsernameValid && IsPasswordValid;
        }
    }
}