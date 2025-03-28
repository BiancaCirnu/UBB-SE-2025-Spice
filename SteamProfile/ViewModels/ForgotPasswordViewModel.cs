using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.ViewModels.Base;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Views;

namespace SteamProfile.ViewModels
{
    public class ForgotPasswordViewModel : INotifyPropertyChanged
    {
        private readonly UserService _userService;
        private string _email = string.Empty;
        private string _resetCode = string.Empty;
        private string _newPassword = string.Empty;
        private string _confirmPassword = string.Empty;
        private string _statusMessage = string.Empty;
        private Brush _statusColor;
        private bool _showEmailSection = true;
        private bool _showCodeSection = false;
        private bool _showPasswordSection = false;

        public event PropertyChangedEventHandler PropertyChanged;

        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged(nameof(Email));
            }
        }

        public string ResetCode
        {
            get => _resetCode;
            set
            {
                _resetCode = value;
                OnPropertyChanged(nameof(ResetCode));
            }
        }

        public string NewPassword
        {
            get => _newPassword;
            set
            {
                _newPassword = value;
                OnPropertyChanged(nameof(NewPassword));
            }
        }

        public string ConfirmPassword
        {
            get => _confirmPassword;
            set
            {
                _confirmPassword = value;
                OnPropertyChanged(nameof(ConfirmPassword));
            }
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged(nameof(StatusMessage));
            }
        }

        public Brush StatusColor
        {
            get => _statusColor;
            set
            {
                _statusColor = value;
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public bool ShowEmailSection
        {
            get => _showEmailSection;
            set
            {
                _showEmailSection = value;
                OnPropertyChanged(nameof(ShowEmailSection));
            }
        }

        public bool ShowCodeSection
        {
            get => _showCodeSection;
            set
            {
                _showCodeSection = value;
                OnPropertyChanged(nameof(ShowCodeSection));
            }
        }

        public bool ShowPasswordSection
        {
            get => _showPasswordSection;
            set
            {
                _showPasswordSection = value;
                OnPropertyChanged(nameof(ShowPasswordSection));
            }
        }

        public ICommand SendResetCodeCommand { get; }
        public ICommand VerifyCodeCommand { get; }
        public ICommand ResetPasswordCommand { get; }

        public ForgotPasswordViewModel(UserService userService)
        {
            _userService = userService;
            _statusColor = new SolidColorBrush(Colors.Black);
            SendResetCodeCommand = new RelayCommand(SendResetCode);
            VerifyCodeCommand = new RelayCommand(VerifyCode);
            ResetPasswordCommand = new RelayCommand(ResetPassword);
        }

        private void SendResetCode()
        {
            try
            {
                var resetCode = _userService.GeneratePasswordResetCode(Email);
                if (resetCode != null)
                {
                    string filePath = Path.Combine(Path.GetTempPath(), "reset_code.txt");
                    File.WriteAllText(filePath, $"Your password reset code is: {resetCode}");
                    System.Diagnostics.Process.Start("notepad.exe", filePath);
                    
                    ShowEmailSection = false;
                    ShowCodeSection = true;
                    StatusMessage = "Reset code has been generated and opened in a text file.";
                    StatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    StatusMessage = "Email not found in our records.";
                    StatusColor = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "An error occurred. Please try again later.";
                StatusColor = new SolidColorBrush(Colors.Red);
            }
        }

        private void VerifyCode()
        {
            try
            {
                if (_userService.VerifyResetCode(Email, ResetCode))
                {
                    ShowCodeSection = false;
                    ShowPasswordSection = true;
                    StatusMessage = "Code verified successfully. Please enter your new password.";
                    StatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    StatusMessage = "Invalid or expired code.";
                    StatusColor = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "An error occurred. Please try again later.";
                StatusColor = new SolidColorBrush(Colors.Red);
            }
        }

        private (bool isValid, string errorMessage) ValidatePassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return (false, "Password cannot be empty.");

            if (password.Length < 8)
                return (false, "Password must be at least 8 characters long.");

            if (!password.Any(char.IsUpper))
                return (false, "Password must contain at least one uppercase letter.");

            if (!password.Any(char.IsLower))
                return (false, "Password must contain at least one lowercase letter.");

            if (!password.Any(char.IsDigit))
                return (false, "Password must contain at least one number.");

            if (!password.Any(ch => !char.IsLetterOrDigit(ch)))
                return (false, "Password must contain at least one special character.");

            return (true, string.Empty);
        }

        private void ResetPassword()
        {
            try
            {
                if (NewPassword != ConfirmPassword)
                {
                    StatusMessage = "Passwords do not match.";
                    StatusColor = new SolidColorBrush(Colors.Red);
                    return;
                }

                var (isValid, errorMessage) = ValidatePassword(NewPassword);
                if (!isValid)
                {
                    StatusMessage = errorMessage;
                    StatusColor = new SolidColorBrush(Colors.Red);
                    return;
                }

                if (_userService.ResetPassword(Email, ResetCode, NewPassword))
                {
                    StatusMessage = "Password reset successful. You can now login with your new password.";
                    StatusColor = new SolidColorBrush(Colors.Green);
                }
                else
                {
                    StatusMessage = "Failed to reset password. Please try again.";
                    StatusColor = new SolidColorBrush(Colors.Red);
                }
            }
            catch (Exception ex)
            {
                StatusMessage = "An error occurred. Please try again later.";
                StatusColor = new SolidColorBrush(Colors.Red);
            }
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
