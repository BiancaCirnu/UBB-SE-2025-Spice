using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SteamProfile.Models;
using SteamProfile.Views;
using SteamProfile.Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();

            // Initialize NavigationService with the ContentFrame
            NavigationService.Instance.Initialize(ContentFrame);

            // Set the initial page to UsersPage
            ContentFrame.Navigate(typeof(UsersPage));

            // Set the initial selected item in the navigation menu
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem selectedItem)
            {
                Type pageType = selectedItem.Tag?.ToString() switch
                {
                    "users" => typeof(UsersPage),
                    "profile" => typeof(ProfilePage),
                    "achievements" => typeof(AchievementsPage),
                    "collections" => typeof(CollectionsPage),
                    "features" => typeof(FeaturesPage),
                    "wallet" => typeof(WalletPage),
                    "friends" => typeof(FriendsPage),
                    "configurations" => typeof(ConfigurationsPage),
                    _ => typeof(UsersPage)
                };

                ContentFrame.Navigate(pageType);
            }
        }
    }
}
