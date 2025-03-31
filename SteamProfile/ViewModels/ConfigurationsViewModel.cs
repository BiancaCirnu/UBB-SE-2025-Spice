using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Services;
using SteamProfile.Views;
using SteamProfile.Views.ConfigurationsView;

namespace SteamProfile.ViewModels
{
    public partial class ConfigurationsViewModel : ObservableObject
    {
        private readonly Frame _frame;
        private readonly UserService _userService;

        public ConfigurationsViewModel(Frame frame)
        {
            _frame = frame;
        }

        [RelayCommand]
        private void NavigateToFeatures()
        {
            _frame.Navigate(typeof(FeaturesPage));
        }

        [RelayCommand]
        private void NavigateToProfile()
        {
            _frame.Navigate(typeof(ProfilePage));
        }
        [RelayCommand]
        private void NavigateToProfileSettings()
        {
            _frame.Navigate(typeof(ModifyProfilePage));
        }
        [RelayCommand]

        private void NavigateToAccountSettings()
        {
            _frame.Navigate(typeof(AccountSettingsPage));
        }
        [RelayCommand]
        private void NavigateToWallet()
        {
            _frame.Navigate(typeof(WalletPage));
        }
    }
}
