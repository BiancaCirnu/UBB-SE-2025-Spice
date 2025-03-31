using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Views;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public partial class ModifyProfileViewModel : ObservableObject
    {
        private readonly Frame _frame;

        public ModifyProfileViewModel(Frame frame)
        {
            _frame = frame;
        }

        [RelayCommand]
        private void NavigateToFeatures()
        {
            _frame.Navigate(typeof(FeaturesPage));
        }

        [RelayCommand]
        private void BackToConfig()
        {
            _frame.GoBack();
        }

        [RelayCommand]
        private void ChooseNewPhoto()
        {

        }
        [ObservableProperty]
        private string selectedImageName = string.Empty ;
        [ObservableProperty]
        private string description = string.Empty;
        [ObservableProperty]
        private string descriptionErrorMessage = string.Empty;
        [ObservableProperty]
        private Visibility descriptionErrorVisibility = Visibility.Collapsed;
        [ObservableProperty]
        public bool canSave;

    }
}