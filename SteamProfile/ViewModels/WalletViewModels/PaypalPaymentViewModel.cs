using Microsoft.UI.Xaml;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class PaypalPaymentViewModel : INotifyPropertyChanged
    {
        // Properties

        private int _amount;
        private WalletViewModel _walletViewModel;
        private User _user;

        private string _amountText;
        private string _email = string.Empty;
        private bool _isEmailValid;
        private bool _isPasswordValid;
        private bool _showErrorMessage;
        private string _statusMessage;
        private Visibility _statusMessageVisibility;

        public string AmountText
        {
            get => _amountText;
            private set
            {
                if (_amountText != value)
                {
                    _amountText = value;
                    OnPropertyChanged();
                }
            }
        }

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

        public bool IsEmailValid
        {
            get => _isEmailValid;
            private set
            {
                if (_isEmailValid != value)
                {
                    _isEmailValid = value;
                    OnPropertyChanged();
                    UpdateErrorMessageVisibility();
                }
            }
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
                    UpdateErrorMessageVisibility();
                }
            }
        }

        public bool ShowErrorMessage
        {
            get => _showErrorMessage;
            private set
            {
                if (_showErrorMessage != value)
                {
                    _showErrorMessage = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(ErrorMessageVisibility));
                }
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            private set
            {
                if (_statusMessage != value)
                {
                    _statusMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility StatusMessageVisibility
        {
            get => _statusMessageVisibility;
            private set
            {
                if (_statusMessageVisibility != value)
                {
                    _statusMessageVisibility = value;
                    OnPropertyChanged();
                }
            }
        }

        public Visibility ErrorMessageVisibility => ShowErrorMessage ? Visibility.Visible : Visibility.Collapsed;

        public bool AreAllFieldsValid => IsEmailValid && IsPasswordValid;


        // Initialization

        public PaypalPaymentViewModel()
        {
            StatusMessageVisibility = Visibility.Collapsed;
            ShowErrorMessage = false;
        }

        public void Initialize(int amount, User user, WalletViewModel walletViewModel)
        {
            _amount = amount;
            _user = user;
            _walletViewModel = walletViewModel;

            // Update UI bindings
            AmountText = "Sum: " + _amount.ToString();
        }

        // Overload to match the pattern used in CardPaymentViewModel
        public void Initialize(Dictionary<string, object> parameters)
        {
            _amount = parameters.ContainsKey("sum") ? (int)parameters["sum"] : 0;
            _walletViewModel = parameters.ContainsKey("viewModel") ? (WalletViewModel)parameters["viewModel"] : null;
            _user = parameters.ContainsKey("user") ? (User)parameters["user"] : null;

            // Update UI bindings
            AmountText = "Sum: " + _amount.ToString();
        }


        // Validation Methods

        public void ValidateEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                IsEmailValid = false;
                return;
            }

            string emailRegexString = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,}$";
            Regex emailRegex = new Regex(emailRegexString);

            IsEmailValid = emailRegex.IsMatch(Email);
        }

        public void ValidatePassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                _isPasswordValid = false;
                return;
            }

            string pattern = @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$";
            Regex passwordRegex = new Regex(pattern);

            _isPasswordValid = passwordRegex.IsMatch(password);
        }

        private void UpdateErrorMessageVisibility()
        {
            ShowErrorMessage = !IsEmailValid || !IsPasswordValid;
        }


        // Payment Processing

        public async Task<bool> ProcessPaymentAsync()
        {
            if (!AreAllFieldsValid)
            {
                ShowErrorMessage = true;
                return false;
            }
            // Hide error message
            ShowErrorMessage = false;

            // Update UI to show processing
            StatusMessageVisibility = Visibility.Visible;
            StatusMessage = "Processing...";

            await Task.Delay(1000);

            // Update wallet balance
            if (_walletViewModel != null)
            {
                // Match the method used in the CardPaymentViewModel
                _walletViewModel.AddFunds(_amount);
            }

            StatusMessage = "Payment was performed successfully";
            await Task.Delay(5000);

            return true;
        }


        // INotifyPropertyChanged Implementation

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}