using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using SteamProfile.Models;
using SteamProfile.Services;
using SteamProfile.Views;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private static ProfileViewModel _instance;
        private readonly UserService _userService;
        private readonly FriendsService _friendsService;
        private readonly DispatcherQueue _dispatcherQueue;

        [ObservableProperty]
        private string _username = string.Empty;

        [ObservableProperty]
        private string _bio = string.Empty;

        [ObservableProperty]
        private int _friendCount;

        [ObservableProperty]
        private decimal _money;

        [ObservableProperty]
        private int _points;

        [ObservableProperty]
        private string _profilePicture = string.Empty;

        [ObservableProperty]
        private string _coverPhoto = string.Empty;

        [ObservableProperty]
        private ObservableCollection<string> _collections = new();

        [ObservableProperty]
        private bool _isLoading = false;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isOwner = true;

        [ObservableProperty]
        private int _userId;

        [ObservableProperty]
        private bool _hasGameplayAchievement;

        [ObservableProperty]
        private bool _hasCollectionAchievement;

        [ObservableProperty]
        private bool _hasSocialAchievement;

        [ObservableProperty]
        private bool _hasMarketAchievement;

        [ObservableProperty]
        private bool _hasCustomizationAchievement;

        [ObservableProperty]
        private bool _hasCommunityAchievement;

        [ObservableProperty]
        private bool _hasEventAchievement;

        [ObservableProperty]
        private bool _hasSpecialAchievement;

        [ObservableProperty]
        private string _equippedFrame = string.Empty;

        [ObservableProperty]
        private string _equippedHat = string.Empty;

        [ObservableProperty]
        private string _equippedPet = "ms-appx:///Assets/100_achievement.jpeg";

        [ObservableProperty]
        private string _equippedEmoji = string.Empty;

        public static ProfileViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    throw new InvalidOperationException("ProfileViewModel must be initialized with Initialize() first");
                }
                return _instance;
            }
        }

        public static void Initialize(UserService userService, FriendsService friendsService, DispatcherQueue dispatcherQueue)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("ProfileViewModel is already initialized");
            }
            _instance = new ProfileViewModel(userService, friendsService, dispatcherQueue);
        }

        private ProfileViewModel(UserService userService, FriendsService friendsService, DispatcherQueue dispatcherQueue)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
            _dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
        }

        public async Task LoadProfileAsync(bool isOwner = true)
        {
            try
            {
                await _dispatcherQueue.EnqueueAsync(() => IsLoading = true);
                await _dispatcherQueue.EnqueueAsync(() => ErrorMessage = string.Empty);

                // Load user data on a background thread
                var currentUser = await Task.Run(() => _userService.GetCurrentUser());

                await _dispatcherQueue.EnqueueAsync(() =>
                {
                    if (currentUser != null)
                    {
                        UserId = currentUser.UserId;
                        Username = currentUser.Username;

                        // Set IsOwner based on the parameter
                        IsOwner = isOwner;

                        // Load friend count
                        _ = LoadFriendCountAsync();

                        // Load user profile data
                        _ = LoadUserProfileDataAsync(currentUser.UserId);

                        // Set some test achievement values
                        HasGameplayAchievement = true;
                        HasCollectionAchievement = false;
                        HasSocialAchievement = true;
                        HasMarketAchievement = false;
                        HasCustomizationAchievement = true;
                        HasCommunityAchievement = false;
                        HasEventAchievement = true;
                        HasSpecialAchievement = false;

                        // TODO: Load these from their respective services
                        Money = 0;
                        Points = 0;
                        CoverPhoto = "default_cover.png";

                        Collections.Clear();
                        // TODO: Load collections
                    }
                    IsLoading = false;
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading profile: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                await _dispatcherQueue.EnqueueAsync(() =>
                {
                    ErrorMessage = "Failed to load profile data. Please try again later.";
                    IsLoading = false;
                });
            }
        }

        private async Task LoadUserProfileDataAsync(int userId)
        {
            try
            {
                var userProfile = await Task.Run(() => _userService.GetUserProfile(userId));
                
                if (userProfile != null)
                {
                    await _dispatcherQueue.EnqueueAsync(() =>
                    {
                        ProfilePicture = userProfile.ProfilePicture ?? string.Empty;
                        Bio = userProfile.Bio ?? string.Empty;
                        EquippedFrame = userProfile.EquippedFrame ?? string.Empty;
                        EquippedHat = userProfile.EquippedHat ?? string.Empty;
                        EquippedPet = userProfile.EquippedPet ?? "ms-appx:///Assets/100_achievement.jpeg";
                        EquippedEmoji = userProfile.EquippedEmoji ?? string.Empty;
                    });
                }
                else
                {
                    // Create a new profile if one doesn't exist
                    await Task.Run(() => _userService.CreateUserProfile(userId));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading user profile data: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        private async Task LoadFriendCountAsync()
        {
            try
            {
                var count = await _friendsService.GetFriendshipCount(UserId.ToString());
                await _dispatcherQueue.EnqueueAsync(() => FriendCount = count);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading friend count: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }

        [RelayCommand]
        private void EditProfile()
        {
            // TODO: Implement edit profile functionality
        }

        [RelayCommand]
        private void ShowMoney()
        {
            // Navigate to Wallet/Money page
            NavigationService.Instance.Navigate(typeof(Views.WalletPage));
        }

        [RelayCommand]
        private void ShowPoints()
        {
            // Navigate to Points page
            NavigationService.Instance.Navigate(typeof(Views.WalletPage));
        }

        [RelayCommand]
        private void AddCollection()
        {
            // Navigate to Collections page
            NavigationService.Instance.Navigate(typeof(Views.CollectionsPage));
        }

        [RelayCommand]
        private void ShowAllAchievements()
        {
            // Navigate to Achievements page
            NavigationService.Instance.Navigate(typeof(Views.AchievementsPage));
        }

        [RelayCommand]
        private void ShowFriends()
        {
            NavigationService.Instance.Navigate(typeof(Views.FriendsPage), UserId);
        }
    }

    public static class DispatcherQueueExtensions
    {
        public static Task EnqueueAsync(this DispatcherQueue dispatcher, Action action)
        {
            var tcs = new TaskCompletionSource();
            if (!dispatcher.TryEnqueue(() =>
            {
                try
                {
                    action();
                    tcs.SetResult();
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }
            }))
            {
                tcs.SetException(new InvalidOperationException("Failed to enqueue task to dispatcher"));
            }
            return tcs.Task;
        }
    }
}
