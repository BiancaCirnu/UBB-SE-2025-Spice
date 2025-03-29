using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace SteamProfile.ViewModels
{
    public partial class FriendListViewModel : ObservableObject
    {
        private readonly IFriendsService _friendsService;
        private string _userId;

        [ObservableProperty]
        private ObservableCollection<FriendViewModel> _friends = new();

        [ObservableProperty]
        private int _friendCount;

        [ObservableProperty]
        private bool _isLoading;

        public FriendListViewModel(IFriendsService friendsService)
        {
            _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
        }

        [RelayCommand]
        private async Task RemoveFriend(FriendViewModel friend)
        {
            if (friend == null) return;

            try
            {
                await _friendsService.RemoveFriend(_userId, friend.FriendId);
                Friends.Remove(friend);
                FriendCount--;
            }
            catch (Exception ex)
            {
                // Handle error (you might want to show a message to the user)
                System.Diagnostics.Debug.WriteLine($"Error removing friend: {ex.Message}");
            }
        }

        [RelayCommand]
        private void GoToProfile(FriendViewModel friend)
        {
            if (friend == null) return;
            // This will be implemented to navigate to the friend's profile
            // You'll need to coordinate with your teammate who's implementing the profile pages
        }

        public async Task LoadFriends(string userId)
        {
            try
            {
                IsLoading = true;
                _userId = userId;

                // Get friends list
                var friendships = await _friendsService.GetFriendsList(userId);
                Friends.Clear();
                foreach (var friendship in friendships)
                {
                    Friends.Add(new FriendViewModel
                    {
                        FriendshipId = friendship.FriendshipId,
                        FriendId = friendship.FriendId.ToString(),
                        Username = friendship.FriendId.ToString() // This will be replaced with actual username when we get the Community team's service
                    });
                }

                // Get friend count
                FriendCount = await _friendsService.GetFriendshipCount(userId);
            }
            catch (Exception ex)
            {
                // Handle error (you might want to show a message to the user)
                System.Diagnostics.Debug.WriteLine($"Error loading friends: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
    }

    public partial class FriendViewModel : ObservableObject
    {
        [ObservableProperty]
        private int _friendshipId;

        [ObservableProperty]
        private string _friendId;

        [ObservableProperty]
        private string _username;
    }
} 