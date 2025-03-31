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

        public ProfilePage()
        {
            try
            {
                InitializeComponent();
                Debug.WriteLine("ProfilePage initialized.");

                // Initialize the ViewModel with the UI thread's dispatcher
                var dataLink = DataLink.Instance;
                Debug.WriteLine("DataLink instance obtained.");

                //var friendshipsRepository = new FriendshipsRepository(dataLink);
                var friendsService = App.FriendsService;
                Debug.WriteLine("FriendshipsRepository and FriendsService initialized.");

                // Add the UserProfileRepository parameter
                ProfileViewModel.Initialize(
                    App.UserService, 
                    friendsService, 
                    Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread(),
                    App.UserProfileRepository
                );
                Debug.WriteLine("ProfileViewModel initialized with services.");

                ViewModel = ProfileViewModel.Instance;

                // By default, assume we're viewing someone else's profile
              //  _isOwnProfile = true;
                Debug.WriteLine("Assuming we're viewing someone else's profile.");

                // Load the profile data
               // _ = ViewModel.LoadProfileAsync(_isOwnProfile);
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            Debug.WriteLine("Navigated to ProfilePage.");

            // If we receive a parameter, it indicates whose profile we're viewing
            if (e.Parameter != null)
            {
                // If the parameter is true, it means we're viewing our own profile
                _userId = (int)e.Parameter;
                Debug.WriteLine($"Navigating to profile of user: {_userId}");
                Debug.WriteLine($"ProfileViewModel Instance: {ProfileViewModel.Instance != null}");
                ViewModel = ProfileViewModel.Instance;
                _ = ViewModel.LoadProfileAsync(_userId);  // ??!!
            }
        }

        private async void ShowErrorDialog(string message)
        {
            try
            {
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
    }
}