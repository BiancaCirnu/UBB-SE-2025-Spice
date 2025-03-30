using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using SteamProfile.Models;
using SteamProfile.Services;
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

        public static void Initialize(UserService userService, DispatcherQueue dispatcherQueue)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("ProfileViewModel is already initialized");
            }
            _instance = new ProfileViewModel(userService, dispatcherQueue);
        }

        private ProfileViewModel(UserService userService, DispatcherQueue dispatcherQueue)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
        }

        public async Task LoadProfileAsync()
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
                        Username = currentUser.Username;
                        Bio = currentUser.Description ?? string.Empty;
                        ProfilePicture = currentUser.ProfilePicture ?? string.Empty;

                        // Set IsOwner based on whether the viewed profile is the current user's profile
                        IsOwner = true; // TODO: Compare with logged-in user ID when implementing profile viewing

                        // TODO: Load these from their respective services
                        FriendCount = 0;
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
            // Navigate to Friends page
            NavigationService.Instance.Navigate(typeof(Views.FriendsPage));
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
