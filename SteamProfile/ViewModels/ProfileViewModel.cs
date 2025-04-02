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
using SteamProfile.Views;
using Microsoft.UI.Xaml;
using SteamProfile.Repositories;
using System.Linq;

namespace SteamProfile.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private static ProfileViewModel _instance;
        private readonly UserService _userService;
        private readonly FriendsService _friendsService;
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly UserProfilesRepository _userProfileRepository;
        private readonly AchievementsService _achievementsService;

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
        private ObservableCollection<Collection> _collections = new();

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
        private string _equippedPet = "ms-appx:///Assets/100_achievement.jpeg"; // Using the dog image for now

        [ObservableProperty]
        private string _equippedEmoji = string.Empty;
        private static CollectionsRepository _collectionsRepository;

        [ObservableProperty]
        private AchievementWithStatus _friendshipsAchievement;

        [ObservableProperty]
        private AchievementWithStatus _ownedGamesAchievement;

        [ObservableProperty]
        private AchievementWithStatus _soldGamesAchievement;

        [ObservableProperty]
        private AchievementWithStatus _numberOfReviewsAchievement;

        [ObservableProperty]
        private bool _isDeveloper;

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

        public static void Initialize(
            UserService userService,
            FriendsService friendsService,
            DispatcherQueue dispatcherQueue,
            UserProfilesRepository userProfileRepository,
            CollectionsRepository collectionsRepository,
            AchievementsService achievementsService)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("ProfileViewModel is already initialized");
            }
            _instance = new ProfileViewModel(userService, friendsService, dispatcherQueue, userProfileRepository, collectionsRepository, achievementsService);
        }

        public ProfileViewModel(
            UserService userService,
            FriendsService friendsService,
            DispatcherQueue dispatcherQueue,
            UserProfilesRepository userProfileRepository,
            CollectionsRepository collectionsRepository,
            AchievementsService achievementsService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
            _dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
            _userProfileRepository = userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));
            _collectionsRepository = collectionsRepository ?? throw new ArgumentNullException(nameof(collectionsRepository));
            _achievementsService = achievementsService ?? throw new ArgumentNullException(nameof(achievementsService));

        }

        public async Task LoadProfileAsync(int user_id)
        {
            try
            {
                await _dispatcherQueue.EnqueueAsync(() => IsLoading = true);
                await _dispatcherQueue.EnqueueAsync(() => ErrorMessage = string.Empty);

                // Load both user and profile data on a background thread
                var currentUser = await Task.Run(() => _userService.GetUserById(user_id));
                var userProfile = await Task.Run(() =>
                    _userProfileRepository.GetUserProfileByUserId(currentUser.UserId));

                await _dispatcherQueue.EnqueueAsync(() =>
                {
                    if (currentUser != null)
                    {
                        if (user_id == _userService.GetCurrentUser().UserId)
                            IsOwner = true;
                        else IsOwner = false;

                        // Basic user info from Users table
                        UserId = currentUser.UserId;
                        Username = currentUser.Username;
                        IsDeveloper = currentUser.IsDeveloper;

                        Debug.WriteLine($"Current user {Username} ; isOwner = {IsOwner} ; isDeveloper = {IsDeveloper}");

                        // Profile info from UserProfiles table
                        if (userProfile != null)
                        {
                            Bio = userProfile.Bio ?? string.Empty;
                            ProfilePicture = userProfile.ProfilePicture ?? string.Empty;
                        }

                        // Set IsOwner based on the parameter
                        //IsOwner = isOwner;

                        // Load friend count
                        FriendCount = _friendsService.GetFriendshipCount(currentUser.UserId);

                        // First unlock any achievements the user has earned
                        _achievementsService.UnlockAchievementForUser(currentUser.UserId);

                        // Then load achievements
                        FriendshipsAchievement = GetTopAchievement(currentUser.UserId, "Friendships");
                        OwnedGamesAchievement = GetTopAchievement(currentUser.UserId, "Owned Games");
                        SoldGamesAchievement = GetTopAchievement(currentUser.UserId, "Sold Games");
                        NumberOfReviewsAchievement = GetTopAchievement(currentUser.UserId, "Number of Reviews");

                        Debug.WriteLine($"Loaded achievements for user {currentUser.UserId}:");
                        Debug.WriteLine($"Friendships: {FriendshipsAchievement?.Achievement?.AchievementName}, Unlocked: {FriendshipsAchievement?.IsUnlocked}");
                        Debug.WriteLine($"Owned Games: {OwnedGamesAchievement?.Achievement?.AchievementName}, Unlocked: {OwnedGamesAchievement?.IsUnlocked}");
                        Debug.WriteLine($"Sold Games: {SoldGamesAchievement?.Achievement?.AchievementName}, Unlocked: {SoldGamesAchievement?.IsUnlocked}");
                        Debug.WriteLine($"Reviews: {NumberOfReviewsAchievement?.Achievement?.AchievementName}, Unlocked: {NumberOfReviewsAchievement?.IsUnlocked}");

                        // TODO: Load these from their respective services
                        Money = 0;
                        Points = 0;
                        CoverPhoto = "default_cover.png";


                        // Get the last three collections
                        var lastThreeCollections = _collectionsRepository.GetLastThreeCollectionsForUser(user_id);
                        Collections.Clear();



                        foreach (var collection in lastThreeCollections)
                        {
                            Collections.Add(collection);
                        }

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

        private AchievementWithStatus GetTopAchievement(int userId, string category)
        {
            try
            {
                // Get all achievements for this category
                var achievements = _achievementsService.GetAchievementsWithStatusForUser(userId)
                    .Where(a => a.Achievement.AchievementType == category)
                    .ToList();

                // First try to get the highest-points unlocked achievement
                var topUnlockedAchievement = achievements
                    .Where(a => a.IsUnlocked)
                    .OrderByDescending(a => a.Achievement.Points)
                    .FirstOrDefault();

                // If we found an unlocked achievement, return it
                if (topUnlockedAchievement != null)
                {
                    Debug.WriteLine($"Found top unlocked {category} achievement: {topUnlockedAchievement.Achievement.AchievementName}");
                    return topUnlockedAchievement;
                }

                // If no unlocked achievements, get the lowest-points locked achievement
                var lowestLockedAchievement = achievements
                    .Where(a => !a.IsUnlocked)
                    .OrderBy(a => a.Achievement.Points)
                    .FirstOrDefault();

                // If we found a locked achievement, return it
                if (lowestLockedAchievement != null)
                {
                    Debug.WriteLine($"Found lowest locked {category} achievement: {lowestLockedAchievement.Achievement.AchievementName}");
                    return lowestLockedAchievement;
                }

                // If no achievements found at all, return an empty achievement
                Debug.WriteLine($"No achievements found for {category}, returning empty achievement");
                return new AchievementWithStatus
                {
                    Achievement = new Achievement
                    {
                        AchievementName = $"No {category} Achievement",
                        Description = "Complete tasks to unlock this achievement",
                        AchievementType = category,
                        Points = 0,
                        Icon = "ms-appx:///Assets/empty_achievement.png" // Use a grayscale or empty icon
                    },
                    IsUnlocked = false
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error getting top achievement for {category}: {ex.Message}");
                return new AchievementWithStatus
                {
                    Achievement = new Achievement
                    {
                        AchievementName = $"No {category} Achievement",
                        Description = "Complete tasks to unlock this achievement",
                        AchievementType = category,
                        Points = 0,
                        Icon = "ms-appx:///Assets/empty_achievement.png" // Use a grayscale or empty icon
                    },
                    IsUnlocked = false
                };
            }
        }

        [RelayCommand]
        private void Configuration()
        {
            NavigationService.Instance.Navigate(typeof(Views.ConfigurationsPage));
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

        [RelayCommand]
        private void BackToProfile()
        {
            // Get the current user's ID from the UserService
            int currentUserId = _userService.GetCurrentUser().UserId; // Adjust this line based on your UserService implementation

            // Navigate back to the Profile page with the current user ID
            NavigationService.Instance.Navigate(typeof(ProfilePage), currentUserId);
        }
    }

    public static partial class DispatcherQueueExtensions
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