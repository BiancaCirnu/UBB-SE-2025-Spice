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
using System.Collections.Generic;

namespace SteamProfile.ViewModels
{
    public partial class ProfileViewModel : ObservableObject
    {
        private static ProfileViewModel _instance;
        private readonly UserService _userService;
        private readonly FriendsService _friendsService;
        private readonly DispatcherQueue _dispatcherQueue;
        private readonly UserProfilesRepository _userProfileRepository;
        private readonly FeaturesService _featuresService;

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
        private string _equippedFrameSource = string.Empty;

        [ObservableProperty]
        private string _equippedHatSource = string.Empty;

        [ObservableProperty]
        private string _equippedPetSource = string.Empty;

        [ObservableProperty]
        private string _equippedEmojiSource = string.Empty;

        [ObservableProperty]
        private string _equippedBackgroundSource = string.Empty;

        [ObservableProperty]
        private bool _hasEquippedFrame;

        [ObservableProperty]
        private bool _hasEquippedHat;

        [ObservableProperty]
        private bool _hasEquippedPet;

        [ObservableProperty]
        private bool _hasEquippedEmoji;

        [ObservableProperty]
        private bool _hasEquippedBackground;

        private static CollectionsRepository _collectionsRepository;

        public static bool IsInitialized => _instance != null;

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
            FeaturesService featuresService)
        {
            if (_instance != null)
            {
                throw new InvalidOperationException("ProfileViewModel is already initialized");
            }
            _instance = new ProfileViewModel(userService, friendsService, dispatcherQueue, userProfileRepository, collectionsRepository, featuresService);
        }

        public ProfileViewModel(
            UserService userService, 
            FriendsService friendsService, 
            DispatcherQueue dispatcherQueue,
            UserProfilesRepository userProfileRepository,
            CollectionsRepository collectionsRepository,
            FeaturesService featuresService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            _friendsService = friendsService ?? throw new ArgumentNullException(nameof(friendsService));
            _dispatcherQueue = dispatcherQueue ?? throw new ArgumentNullException(nameof(dispatcherQueue));
            _userProfileRepository = userProfileRepository ?? throw new ArgumentNullException(nameof(userProfileRepository));
            _collectionsRepository = collectionsRepository ?? throw new ArgumentNullException(nameof(collectionsRepository));
            _featuresService = featuresService ?? throw new ArgumentNullException(nameof(featuresService));
            
            // Register for feature equipped/unequipped events
            FeaturesViewModel.FeatureEquipStatusChanged += async (sender, userId) => 
            {
                // Only refresh if it's the current user's profile being displayed
                if (userId == UserId)
                {
                    await RefreshEquippedFeaturesAsync();
                }
            };
        }

        public async Task LoadProfileAsync(int user_id)
        {
            try
            {
                await _dispatcherQueue.EnqueueAsync(() => IsLoading = true);
                await _dispatcherQueue.EnqueueAsync(() => ErrorMessage = string.Empty);

                Debug.WriteLine($"Loading profile for user {user_id}");

                // Added safety check for invalid user ID
                if (user_id <= 0)
                {
                    Debug.WriteLine($"Invalid user ID: {user_id}");
                    await _dispatcherQueue.EnqueueAsync(() => 
                    {
                        ErrorMessage = "Invalid user ID provided.";
                        IsLoading = false;
                    });
                    return;
                }
                
                // Load user first, with careful error handling
                User currentUser = null;
                try
                {
                    // Instead of using Task.Run, try direct call to reduce complexity
                    currentUser = _userService.GetUserById(user_id);
                    
                    if (currentUser == null)
                    {
                        Debug.WriteLine($"User with ID {user_id} not found");
                        await _dispatcherQueue.EnqueueAsync(() => 
                        {
                            ErrorMessage = "User not found.";
                            IsLoading = false;
                        });
                        return;
                    }
                    
                    Debug.WriteLine($"Retrieved user: {currentUser.Username}");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error getting user: {ex.Message}");
                    if (ex.InnerException != null)
                    {
                        Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                    }
                    Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                await _dispatcherQueue.EnqueueAsync(() =>
                    {
                        ErrorMessage = "Failed to load user data.";
                        IsLoading = false;
                    });
                    return;
                }

                // Continue with rest of the method only if we successfully got a user
                try
                {
                    // Get user profile (optional - can proceed without)
                    UserProfile userProfile = null;
                    try
                    {
                        userProfile = _userProfileRepository.GetUserProfileByUserId(currentUser.UserId);
                        Debug.WriteLine($"Retrieved profile ID: {userProfile?.ProfileId.ToString() ?? "null"}");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error getting user profile: {ex.Message}");
                        // Continue without profile info
                    }

                    // Get equipped features (safer direct call instead of Task.Run)
                    List<Feature> equippedFeatures = new List<Feature>();
                    try
                    {
                        equippedFeatures = _featuresService.GetUserEquippedFeatures(currentUser.UserId);
                        Debug.WriteLine($"Retrieved {equippedFeatures.Count} equipped features");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error getting equipped features: {ex.Message}");
                        // Continue with empty features list
                    }

                    // Update UI with retrieved data
                    await _dispatcherQueue.EnqueueAsync(() =>
                    {
                        if (currentUser != null)
                        {
                            IsOwner = user_id == _userService.GetCurrentUser().UserId;
                        UserId = currentUser.UserId;
                            Username = currentUser.Username ?? string.Empty;
                            Debug.WriteLine($"Current user {Username}; isOwner = {IsOwner}");

                        // Profile info from UserProfiles table
                        if (userProfile != null)
                        {
                            Bio = userProfile.Bio ?? string.Empty;
                            // Add ms-appx:/// prefix if it's not already there
                            ProfilePicture = userProfile.ProfilePicture != null 
                                ? (userProfile.ProfilePicture.StartsWith("ms-appx:///") 
                                    ? userProfile.ProfilePicture 
                                    : $"ms-appx:///{userProfile.ProfilePicture}")
                                : "ms-appx:///Assets/default-profile.png";
                        }

                            // Process equipped features
                            ProcessEquippedFeatures(equippedFeatures);

                        // Load friend count
                            try
                            {
                        FriendCount = _friendsService.GetFriendshipCount(currentUser.UserId);
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error getting friend count: {ex.Message}");
                                FriendCount = 0;
                            }

                            // Set achievement values
                        HasGameplayAchievement = true;
                        HasCollectionAchievement = false;
                        HasSocialAchievement = true;
                        HasMarketAchievement = false;
                        HasCustomizationAchievement = true;
                        HasCommunityAchievement = false;
                        HasEventAchievement = true;
                        HasSpecialAchievement = false;

                            // Default values
                        Money = 0;
                        Points = 0;
                        CoverPhoto = "default_cover.png";

                            // Load collections
                            try
                            {
                        var lastThreeCollections = _collectionsRepository.GetLastThreeCollectionsForUser(user_id);
                        Collections.Clear();
                        foreach (var collection in lastThreeCollections)
                        {
                            Collections.Add(collection);
                        }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error loading collections: {ex.Message}");
                            }
                    }
                    IsLoading = false;
                });
            }
            catch (Exception ex)
            {
                    Debug.WriteLine($"Error in profile loading process: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                    Debug.WriteLine($"Stack trace: {ex.StackTrace}");

                await _dispatcherQueue.EnqueueAsync(() =>
                {
                    ErrorMessage = "Failed to load profile data. Please try again later.";
                    IsLoading = false;
                });
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Critical error in LoadProfileAsync: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            }
        }

        private void ProcessEquippedFeatures(List<Feature> equippedFeatures)
        {
            try
            {
                // Use a known-good image path that definitely exists in the project
                const string DEFAULT_IMAGE = "ms-appx:///Assets/default-profile.png";
                
                // Reset all equipped features to a valid empty image
                EquippedFrameSource = DEFAULT_IMAGE;
                EquippedHatSource = DEFAULT_IMAGE;
                EquippedPetSource = DEFAULT_IMAGE;
                EquippedEmojiSource = DEFAULT_IMAGE;
                EquippedBackgroundSource = DEFAULT_IMAGE;
                
                // Reset visibility flags
                HasEquippedFrame = false;
                HasEquippedHat = false;
                HasEquippedPet = false;
                HasEquippedEmoji = false;
                HasEquippedBackground = false;

                Debug.WriteLine($"Processing {equippedFeatures?.Count ?? 0} equipped features");

                // Process each equipped feature with better error handling
                if (equippedFeatures != null)
                {
                    foreach (var feature in equippedFeatures)
                    {
                        if (feature == null)
                        {
                            Debug.WriteLine("Skipping null feature");
                            continue;
                        }
                            
                        Debug.WriteLine($"Processing feature: ID={feature.FeatureId}, Type={feature.Type}, Source={feature.Source}, Equipped={feature.Equipped}");
                        
                        if (feature.Equipped)
                        {
                            try
                            {
                                // Use ms-appx path format for images
                                string source = feature.Source;
                                if (string.IsNullOrEmpty(source))
                                {
                                    Debug.WriteLine($"Skipping feature {feature.FeatureId} with empty source");
                                    continue;
                                }
                                    
                                if (!source.StartsWith("ms-appx:///"))
                                {
                                    source = $"ms-appx:///{source}";
                                }

                                if (string.IsNullOrEmpty(feature.Type))
                                {
                                    Debug.WriteLine($"Skipping feature {feature.FeatureId} with empty type");
                                    continue;
                                }

                                switch (feature.Type.ToLower())
                                {
                                    case "frame":
                                        EquippedFrameSource = source;
                                        HasEquippedFrame = true;
                                        Debug.WriteLine($"Set frame: {source}");
                                        break;
                                    case "hat":
                                        EquippedHatSource = source;
                                        HasEquippedHat = true;
                                        Debug.WriteLine($"Set hat: {source}");
                                        break;
                                    case "pet":
                                        EquippedPetSource = source;
                                        HasEquippedPet = true;
                                        Debug.WriteLine($"Set pet: {source}");
                                        break;
                                    case "emoji":
                                        EquippedEmojiSource = source;
                                        HasEquippedEmoji = true;
                                        Debug.WriteLine($"Set emoji: {source}");
                                        break;
                                    case "background":
                                        EquippedBackgroundSource = source;
                                        HasEquippedBackground = true;
                                        Debug.WriteLine($"Set background: {source}");
                                        break;
                                    default:
                                        Debug.WriteLine($"Unknown feature type: {feature.Type}");
                                        break;
                                }
                            }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Error processing feature {feature.FeatureId}: {ex.Message}");
                            }
                        }
                    }
                }
                
                Debug.WriteLine($"Feature visibility - Frame: {HasEquippedFrame}, Hat: {HasEquippedHat}, Pet: {HasEquippedPet}, Emoji: {HasEquippedEmoji}, Background: {HasEquippedBackground}");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in ProcessEquippedFeatures: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                
                // In case of error, ensure we have valid image sources
                const string DEFAULT_IMAGE = "ms-appx:///Assets/default-profile.png";
                EquippedFrameSource = DEFAULT_IMAGE;
                EquippedHatSource = DEFAULT_IMAGE;
                EquippedPetSource = DEFAULT_IMAGE;
                EquippedEmojiSource = DEFAULT_IMAGE;
                EquippedBackgroundSource = DEFAULT_IMAGE;
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

        public async Task RefreshEquippedFeaturesAsync()
        {
            try
            {
                Debug.WriteLine($"Refreshing equipped features for user {UserId}");
                
                // Get the updated equipped features
                var equippedFeatures = _featuresService.GetUserEquippedFeatures(UserId);
                
                // Process and update the UI
                await _dispatcherQueue.EnqueueAsync(() =>
                {
                    ProcessEquippedFeatures(equippedFeatures);
                });
                
                Debug.WriteLine("Equipped features refreshed successfully");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error refreshing equipped features: {ex.Message}");
            }
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
