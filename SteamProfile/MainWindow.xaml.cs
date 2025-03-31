using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Views;
using Microsoft.UI.Windowing;
using Microsoft.UI;
using System;

namespace SteamProfile
{
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

        private void LoginButton_Click(object sender, RoutedEventArgs e)
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