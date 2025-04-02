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
using SteamProfile.Data;
using SteamProfile.Repositories;
using SteamProfile.Services;
using System.Threading.Tasks;
using SteamProfile.Views;
using SteamProfile.Services;
using Microsoft.UI;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; private set; }
        //private bool _isOwnProfile;
        private int _userId;
        private bool _isNavigatingAway = false;

        public ProfilePage()
        {
            try
            {
                InitializeComponent();
                Debug.WriteLine("ProfilePage initialized.");

                // Only initialize the ViewModel if it hasn't been initialized yet
                if (ProfileViewModel.IsInitialized)
                {
                    Debug.WriteLine("Using existing ProfileViewModel instance.");
                    ViewModel = ProfileViewModel.Instance;
                }
                else
                {
                    // Initialize the ViewModel with the UI thread's dispatcher
                    var dataLink = DataLink.Instance;
                    Debug.WriteLine("DataLink instance obtained.");

                    var friendsService = App.FriendsService;
                    Debug.WriteLine("FriendshipsRepository and FriendsService initialized.");

                    // Add the UserProfileRepository parameter
                    ProfileViewModel.Initialize(
                        App.UserService, 
                        friendsService, 
                        Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread(),
                        App.UserProfileRepository,
                        App.CollectionsRepository,
                        App.FeaturesService
                    );
                    Debug.WriteLine("ProfileViewModel initialized with services.");
                    
                    ViewModel = ProfileViewModel.Instance;
                }
                
                DataContext = ViewModel; // Ensure this is set correctly
                Debug.WriteLine("Profile data loading initiated.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error initializing ProfilePage: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }

                // Show error dialog to user
                ShowErrorDialog("Failed to initialize profile. Please try again later.");
            }
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // Unregister from property changed events to prevent memory leaks
            if (ViewModel != null)
            {
                ViewModel.PropertyChanged -= ViewModel_PropertyChanged;
            }
            
            base.OnNavigatedFrom(e);
            _isNavigatingAway = true;
            Debug.WriteLine("Navigated away from ProfilePage");
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("Navigated to ProfilePage.");

            // Make sure we have a ViewModel instance
            ViewModel = ProfileViewModel.Instance;
            DataContext = ViewModel;

            // Register for property changed events
            ViewModel.PropertyChanged += ViewModel_PropertyChanged;

            // If we receive a parameter, it indicates whose profile we're viewing
            if (e.Parameter != null)
            {
                // Use the parameter as the user ID
                _userId = (int)e.Parameter;
                Debug.WriteLine($"Using user ID from navigation parameter: {_userId}");
                
                // Load the profile data
                _ = ViewModel.LoadProfileAsync(_userId);
            }
            // If no parameter but we're returning to the page and ViewModel has a user ID
            else if (ViewModel.UserId > 0)
            {
                // Use the user ID stored in the ViewModel
                _userId = ViewModel.UserId;
                Debug.WriteLine($"Using stored user ID from ViewModel: {_userId}");
                
                // Load the profile data
                _ = ViewModel.LoadProfileAsync(_userId);
            }
            else
            {
                Debug.WriteLine("No user ID available - cannot load profile");
            }
        }

        private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Update the AdaptiveProfileControl when relevant properties change
            if (e.PropertyName == nameof(ViewModel.Username) ||
                e.PropertyName == nameof(ViewModel.Bio) ||
                e.PropertyName == nameof(ViewModel.ProfilePicture) ||
                e.PropertyName == nameof(ViewModel.EquippedHatSource) ||
                e.PropertyName == nameof(ViewModel.EquippedPetSource) ||
                e.PropertyName == nameof(ViewModel.EquippedEmojiSource) ||
                e.PropertyName == nameof(ViewModel.EquippedFrameSource) ||
                e.PropertyName == nameof(ViewModel.EquippedBackgroundSource) ||
                e.PropertyName == nameof(ViewModel.HasEquippedHat) ||
                e.PropertyName == nameof(ViewModel.HasEquippedPet) ||
                e.PropertyName == nameof(ViewModel.HasEquippedEmoji) ||
                e.PropertyName == nameof(ViewModel.HasEquippedFrame) ||
                e.PropertyName == nameof(ViewModel.HasEquippedBackground))
            {
                UpdateProfileControl();
            }
        }

        private async void ShowErrorDialog(string message)
        {
            try
            {
                // Create a new dialog instance each time
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Error",
                    Content = message,
                    CloseButtonText = "OK",
                    XamlRoot = this.XamlRoot
                };

                await errorDialog.ShowAsync();
                Debug.WriteLine("Error dialog shown.");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing dialog: {ex.Message}");
            }
        }

        private void ViewCollections_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(CollectionsPage));
        }
        private void AchievementsButton_Click (object sender, RoutedEventArgs e)
        {
            this.Frame.Navigate(typeof(AchievementsPage));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            // If we have a valid user ID, refresh the profile
            if (_userId > 0)
            {
                _ = ViewModel.LoadProfileAsync(_userId);
            }
            
            // Initial update of the profile control
            UpdateProfileControl();
        }
        
        private void UpdateProfileControl()
        {
            // Only update if the control exists
            if (ProfileControl == null) return;
            
            // Get profile picture path with correct prefix
            string profilePicture = ViewModel.ProfilePicture;
            
            // Get paths for all equipped items, but only if they are equipped (check visibility flags)
            string hatPath = ViewModel.HasEquippedHat ? ViewModel.EquippedHatSource : null;
            string petPath = ViewModel.HasEquippedPet ? ViewModel.EquippedPetSource : null;
            string emojiPath = ViewModel.HasEquippedEmoji ? ViewModel.EquippedEmojiSource : null;
            string framePath = ViewModel.HasEquippedFrame ? ViewModel.EquippedFrameSource : null;
            string backgroundPath = ViewModel.HasEquippedBackground ? ViewModel.EquippedBackgroundSource : null;
            
            // Update the AdaptiveProfileControl
            ProfileControl.UpdateProfile(
                ViewModel.Username,
                "", // We're displaying bio separately in the ProfilePage
                profilePicture,
                hatPath,
                petPath,
                emojiPath,
                framePath,
                backgroundPath
            );
        }
    }

    public class StringToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string frameString && !string.IsNullOrEmpty(frameString))
            {
                // You can implement logic to convert frame identifiers to colors
                // For example: "rare" -> Gold, "epic" -> Purple, etc.
                switch (frameString.ToLowerInvariant())
                {
                    case "common": return new SolidColorBrush(Colors.Gray);
                    case "rare": return new SolidColorBrush(Colors.CornflowerBlue);
                    case "epic": return new SolidColorBrush(Colors.Purple);
                    case "legendary": return new SolidColorBrush(Colors.Gold);
                    default: return new SolidColorBrush(Colors.White);
                }
            }
            return new SolidColorBrush(Colors.Transparent);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }

    public class BoolToOpacityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                return boolValue ? 1.0 : 0.3;
            }
            return 0.3;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}