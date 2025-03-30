using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public partial class FriendsViewModel : ObservableObject
    {
        private readonly IFriendsService _friendsService;
        private string _userId;

        [ObservableProperty]
        private ObservableCollection<FriendViewModel> _friends = new();

        [ObservableProperty]
        private int _friendCount;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        public FriendsViewModel(IFriendsService friendsService)
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
                ErrorMessage = $"Error removing friend: {ex.Message}";
                System.Diagnostics.Debug.WriteLine($"Error removing friend: {ex.Message}");
            }
        }

        [RelayCommand]
        private void ViewProfile(string friendId)
        {
            if (string.IsNullOrEmpty(friendId)) return;
            // TODO: Navigate to friend's profile
            // NavigationService.Instance.Navigate(typeof(ProfilePage), friendId);
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
                        Username = friendship.FriendId.ToString(), // This will be replaced with actual username when we get the Community team's service
                        Status = "Online", // TODO: Get actual status from the service
                        ProfilePicture = "default_profile.png" // TODO: Get actual profile picture from the service
                    });
                }

                // Get friend count
                FriendCount = await _friendsService.GetFriendshipCount(userId);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error loading friends: {ex.Message}";
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

        [ObservableProperty]
        private string _status = "Offline";

        [ObservableProperty]
        private string _profilePicture = "default_profile.png";
    }
}
