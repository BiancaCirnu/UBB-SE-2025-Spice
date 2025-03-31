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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; }
        private bool _isOwnProfile;

        public ProfilePage()
        {
            try
            {
                InitializeComponent();

                // Initialize the ViewModel with the UI thread's dispatcher
                var dataLink = DataLink.Instance;
                var friendshipsRepository = new FriendshipsRepository(dataLink);
                var friendsService = new FriendsService(friendshipsRepository);
                ProfileViewModel.Initialize(App.UserService, friendsService, Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                ViewModel = ProfileViewModel.Instance;

                // By default, assume we're viewing someone else's profile
                _isOwnProfile = true;

                // Load the profile data
                _ = ViewModel.LoadProfileAsync(_isOwnProfile);
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

            // If we receive a parameter, it indicates whose profile we're viewing
            if (e.Parameter != null)
            {
                // If the parameter is true, it means we're viewing our own profile
                _isOwnProfile = (bool)e.Parameter;
                _ = ViewModel.LoadProfileAsync(_isOwnProfile);
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
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error showing dialog: {ex.Message}");
            }
        }
    }
}
