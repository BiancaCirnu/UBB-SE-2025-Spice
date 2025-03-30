using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.Data;
using SteamProfile.Repositories;
using SteamProfile.Services;
using SteamProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamProfile.Views
{
    public sealed partial class FriendsPage : Page
    {
        private readonly FriendsViewModel _viewModel;
        private int _userId;

        public FriendsViewModel ViewModel => _viewModel;

        public FriendsPage()
        {
            this.InitializeComponent();
            var dataLink = DataLink.Instance;
            var friendshipsRepository = new FriendshipsRepository(dataLink);
            var friendsService = new FriendsService(friendshipsRepository);
            _viewModel = new FriendsViewModel(friendsService);
            this.DataContext = _viewModel;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            if (e.Parameter is int userId)
            {
                _userId = userId;
                await _viewModel.LoadFriends(_userId.ToString());
            }
        }
    }
}
