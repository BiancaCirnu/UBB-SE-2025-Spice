using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Services;
using SteamProfile.Views;
using System;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public partial class RegisterViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly Frame _frame;

        [ObservableProperty]
        private string username;

        [ObservableProperty]
        private string email;

        [ObservableProperty]
        private string password;

        [ObservableProperty]
        private string confirmPassword;

        [ObservableProperty]
        private string errorMessage;

        public RegisterViewModel(Frame frame)
        {
            _userService = App.UserService;
            _frame = frame;
        }

        [RelayCommand]
        private async Task Register()
        {
            try
            {
                ErrorMessage = string.Empty;

                if (string.IsNullOrWhiteSpace(Username) || string.IsNullOrWhiteSpace(Email) || 
                    string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    ErrorMessage = "All fields are required.";
                    return;
                }

                if (Password != ConfirmPassword)
                {
                    ErrorMessage = "Passwords do not match.";
                    return;
                }

                var user = new User
                {
                    Username = Username,
                    Email = Email,
                    Password = Password
                };

                var createdUser = _userService.CreateUser(user);
                if (createdUser != null)
                {
                    // Navigate to login page on successful registration
                    _frame.Navigate(typeof(LoginPage));
                }
                else
                {
                    ErrorMessage = "Failed to create account. Please try again.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"An error occurred: {ex.Message}";
            }
        }

        [RelayCommand]
        private void NavigateToLogin()
        {
            _frame.Navigate(typeof(LoginPage));
        }
    }
} 