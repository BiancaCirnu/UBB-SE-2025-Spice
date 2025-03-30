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
    public class CardPaymentViewModel : INotifyPropertyChanged
    {
        // Properties

        private int _amount;
        private WalletViewModel _walletViewModel;
        private User _user;

        private string _amountText;
        private bool _isNameValid;
        private bool _isCardNumberValid;
        private bool _isCVVValid;
        private bool _isDateValid;
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

        public bool IsNameValid
        {
            get => _isNameValid;
            private set
            {
                if (_isNameValid != value)
                {
                    _isNameValid = value;
                    OnPropertyChanged();
                    UpdateErrorMessageVisibility();
                }
            }
        }

        public bool IsCardNumberValid
        {
            get => _isCardNumberValid;
            private set
            {
                if (_isCardNumberValid != value)
                {
                    _isCardNumberValid = value;
                    OnPropertyChanged();
                    UpdateErrorMessageVisibility();
                }
            }
        }

        public bool IsCVVValid
        {
            get => _isCVVValid;
            private set
            {
                if (_isCVVValid != value)
                {
                    _isCVVValid = value;
                    OnPropertyChanged();
                    UpdateErrorMessageVisibility();
                }
            }
        }

        public bool IsDateValid
        {
            get => _isDateValid;
            private set
            {
                if (_isDateValid != value)
                {
                    _isDateValid = value;
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

        public bool AreAllFieldsValid => IsNameValid && IsCardNumberValid && IsCVVValid && IsDateValid;

        // Initialization

        public CardPaymentViewModel()
        {
            StatusMessageVisibility = Visibility.Collapsed;
            ShowErrorMessage = false;
        }

        public void Initialize(Dictionary<string, object> parameters)
        {
            // Extract parameters
            _amount = parameters.ContainsKey("sum") ? (int)parameters["sum"] : 0;
            _walletViewModel = parameters.ContainsKey("viewModel") ? (WalletViewModel)parameters["viewModel"] : null;
            _user = parameters.ContainsKey("user") ? (User)parameters["user"] : null;

            // Update UI bindings
            AmountText = "Sum: " + _amount.ToString();
        }

        // Validation Methods

        public void ValidateName(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                IsNameValid = false;
                return;
            }
            IsNameValid = name.Split(' ').Length > 1;
        }

        public void ValidateCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                IsCardNumberValid = false;
                return;
            }

            // Simple check for 16-digit card number
            IsCardNumberValid = Regex.IsMatch(cardNumber, @"^\d{16}$");
        }

        public void ValidateCVV(string cvv)
        {
            if (string.IsNullOrEmpty(cvv))
            {
                IsCVVValid = false;
                return;
            }

            // Simple check for 3-digit CVV
            IsCVVValid = Regex.IsMatch(cvv, @"^\d{3}$");
        }

        public void ValidateExpirationDate(string expirationDate)
        {
            if (string.IsNullOrEmpty(expirationDate))
            {
                IsDateValid = false;
                return;
            }

            // Check MM/YY format
            bool isValidDateFormat = Regex.IsMatch(expirationDate, @"^(0[1-9]|1[0-2])\/\d{2}$");

            if (!isValidDateFormat)
            {
                IsDateValid = false;
                return;
            }

            // Check if date is in the future
            string[] date = expirationDate.Split('/');
            int month = int.Parse(date[0]);
            int year = int.Parse(date[1]);
            int currentMonth = DateTime.Today.Month;
            int currentYear = DateTime.Today.Year % 100;

            IsDateValid = (year > currentYear) || (year == currentYear && month >= currentMonth);
        }

        private void UpdateErrorMessageVisibility()
        {
            ShowErrorMessage = !IsNameValid || !IsCardNumberValid || !IsCVVValid || !IsDateValid;
        }


        // Payment Processing

        public async Task<bool> ProcessPaymentAsync()
        {
            if (!AreAllFieldsValid)
            {
                ShowErrorMessage = true;
                return false;
            }

            // Update UI to show processing
            StatusMessageVisibility = Visibility.Visible;
            StatusMessage = "Processing...";

            await Task.Delay(1000);

            // Update wallet balance via the WalletViewModel
            if (_walletViewModel != null)
            {
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