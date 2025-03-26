using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels;

namespace SteamProfile.Views
{
    public sealed partial class UsersPage : Page
    {
        private readonly UsersViewModel _viewModel;

        public UsersPage()
        {
            this.InitializeComponent();
            _viewModel = UsersViewModel.Instance;
            this.DataContext = _viewModel;
        }
    }
} 