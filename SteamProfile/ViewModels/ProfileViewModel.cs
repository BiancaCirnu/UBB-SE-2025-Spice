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

namespace SteamProfile.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private readonly UserService _userService;
        // .. add all other needed services

        [ObservableProperty]
        private User currentUser; // Property to hold the current user

        public ProfileViewModel(UserService userService)
        {
            _userService = userService;
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
            // Logic to navigate back to the login page can be handled in the view or through a navigation service
        }
    }
}
