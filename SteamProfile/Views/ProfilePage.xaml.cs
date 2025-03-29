using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels;

namespace SteamProfile.Views
{
    public sealed partial class ProfilePage : Page
    {
        public ProfilePage()
        {
            this.InitializeComponent();
            this.DataContext = new ProfileViewModel(App.UserService);
        }
    }
}