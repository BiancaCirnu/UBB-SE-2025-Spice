using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels;

namespace SteamProfile.Views
{
    public sealed partial class UsersPage : Page
    {
        public UsersViewModel ViewModel { get; }

        public UsersPage()
        {
            ViewModel = UsersViewModel.Instance;
            this.InitializeComponent();
            this.DataContext = ViewModel;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Handle user selection if needed
        }
    }
} 