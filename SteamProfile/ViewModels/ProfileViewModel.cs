using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SteamProfile.Views;
using Microsoft.UI.Xaml;

namespace SteamProfile.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        private readonly Frame _frame;

        [ObservableProperty]
        private User currentUser;

        public ProfileViewModel(UserService userService, Frame frame)
        {
            _userService = userService;
            _frame = frame;
            LoadUserProfile();
        }

        private void LoadUserProfile()
        {
            CurrentUser = _userService.GetCurrentUser(); // Load the current user
        }

        [RelayCommand]
        private void Logout()
        {
            _userService.Logout();
            _frame.Navigate(typeof(LoginPage)); // Navigate back to the login page
        }
    }
}
