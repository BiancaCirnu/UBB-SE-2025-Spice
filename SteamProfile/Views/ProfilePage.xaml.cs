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

namespace SteamProfile.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; }

        public ProfilePage()
        {
            try
            {
                InitializeComponent();

                // Initialize the ViewModel with the UI thread's dispatcher
                ProfileViewModel.Initialize(App.UserService, Microsoft.UI.Dispatching.DispatcherQueue.GetForCurrentThread());
                ViewModel = ProfileViewModel.Instance;

                // Load the profile data
                _ = ViewModel.LoadProfileAsync();
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
