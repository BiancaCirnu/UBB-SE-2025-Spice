using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.Diagnostics;

namespace SteamProfile.Views
{
    public sealed partial class FriendsPage : Page
    {
        private readonly FriendsViewModel _viewModel;

        public FriendsViewModel ViewModel => _viewModel;

        public FriendsPage()
        {
            InitializeComponent();
            _viewModel = new FriendsViewModel(App.FriendsService);
            DataContext = _viewModel;
            _viewModel.LoadFriends(); // Load friends immediately when page is created
        }

        private void RemoveFriend_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int friendshipId)
            {
                try
                {
                    _viewModel.RemoveFriend(friendshipId);
                }
                catch (Exception ex)
                {
                    // Handle any errors
                    Debug.WriteLine($"Error removing friend: {ex.Message}");
                }
            }
        }

        private void ViewProfile_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.CommandParameter is int friendId)
            {
                Debug.WriteLine($"Navigating to profile page for friend ID: {friendId}");
                // TODO: When profile pages are implemented, navigate to the profile page
                // Frame.Navigate(typeof(ProfilePage), friendId);
            }
        }
    }
}
