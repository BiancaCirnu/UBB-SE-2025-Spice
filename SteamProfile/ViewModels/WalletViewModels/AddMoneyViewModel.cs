using Microsoft.UI.Xaml;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using CommunityToolkit.Mvvm.Input;
using System.Windows.Input;

namespace SteamProfile.ViewModels
{
    public class AddMoneyViewModel : INotifyPropertyChanged
    {
        private readonly WalletViewModel _walletViewModel;
        private readonly List<char> _digitsAsChar = new() { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
        private bool _isInputValid;
        private bool _showErrorMessage;
        private string _amountToAdd;
        private const int MAX_AMOUNT = 500;

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand AddFundsCommand { get; }

        public AddMoneyViewModel(WalletViewModel walletViewModel)
        {
            _walletViewModel = walletViewModel ?? throw new ArgumentNullException(nameof(walletViewModel));
            IsInputValid = false;
            ShowErrorMessage = false;

            // Initialize the command
            AddFundsCommand = new RelayCommand(ProcessAddFunds, () => IsInputValid);
        }

        public string AmountToAdd
        {
            get => _amountToAdd;
            set
            {
                if (_amountToAdd != value)
                {
                    _amountToAdd = value;
                    ValidateInput(value);
                    OnPropertyChanged();
                    // Force command to reevaluate CanExecute
                    (AddFundsCommand as RelayCommand)?.NotifyCanExecuteChanged();
                }
            }
        }

        public bool IsInputValid
        {
            get => _isInputValid;
            private set
            {
                if (_isInputValid != value)
                {
                    _isInputValid = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(PaymentButtonsEnabled));
                    // Force command to reevaluate CanExecute
                    (AddFundsCommand as RelayCommand)?.NotifyCanExecuteChanged();
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

        public Visibility ErrorMessageVisibility => ShowErrorMessage ? Visibility.Visible : Visibility.Collapsed;

        public bool PaymentButtonsEnabled => IsInputValid;

        public void ValidateInput(string input)
        {
            ShowErrorMessage = false;
            IsInputValid = false;

            if (string.IsNullOrEmpty(input))
            {
                return;
            }

            // Check if input is too long
            if (input.Length > 3)
            {
                ShowErrorMessage = true;
                return;
            }

            // Check if input contains only digits
            if (input.Any(c => !_digitsAsChar.Contains(c)))
            {
                ShowErrorMessage = true;
                return;
            }

            // Check if amount is within limits
            if (int.TryParse(input, out int amount))
            {
                if (amount > MAX_AMOUNT || amount <= 0)
                {
                    ShowErrorMessage = true;
                    return;
                }

                IsInputValid = true;
            }
            else
            {
                ShowErrorMessage = true;
            }
        }

        private void ProcessAddFunds()
        {
            if (!IsInputValid || string.IsNullOrEmpty(AmountToAdd))
                return;

            if (int.TryParse(AmountToAdd, out int amount))
            {
                _walletViewModel.AddFunds(amount);
                AmountToAdd = string.Empty;
                IsInputValid = false;
            }
        }

        public Dictionary<string, object> CreateNavigationParameters()
        {
            return new Dictionary<string, object>
            {
                { "sum", int.Parse(AmountToAdd) },
                { "viewModel", _walletViewModel }
            };
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}