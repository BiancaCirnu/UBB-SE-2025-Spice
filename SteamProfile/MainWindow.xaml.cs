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
            Title = "SteamProfile";
            
            // Set the initial page to UsersPage
            ContentFrame.Navigate(typeof(UsersPage));
            
            // Set the initial selected item in the navigation menu
            NavView.SelectedItem = NavView.MenuItems[0];
        }

        private void NavView_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItemContainer != null)
            {
                string tag = args.SelectedItemContainer.Tag.ToString();
                switch (tag)
                {
                    case "users":
                        ContentFrame.Navigate(typeof(UsersPage));
                        break;
                    case "profile":
                        ContentFrame.Navigate(typeof(ProfilePage));
                        break;
                    case "achievements":
                        ContentFrame.Navigate(typeof(AchievementsPage));
                        break;
                    case "collections":
                        ContentFrame.Navigate(typeof(CollectionsPage));
                        break;
                    case "features":
                        ContentFrame.Navigate(typeof(FeaturesPage));
                        break;
                    case "friends":
                        ContentFrame.Navigate(typeof(FriendsPage));
                        break;
                    case "configurations":
                        ContentFrame.Navigate(typeof(ConfigurationsPage));
                        break;
                }
            }
        }
    }
}
