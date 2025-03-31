using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Views;

namespace SteamProfile.ViewModels
{
    public partial class ConfigurationsViewModel : ObservableObject
    {
        private readonly Frame _frame;

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
    }
}
