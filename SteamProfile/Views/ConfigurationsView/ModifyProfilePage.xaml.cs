using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels.ConfigurationsViewModels;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SteamProfile.Views.ConfigurationsView
{
    public sealed partial class ModifyProfilePage : Page
    {
        public ModifyProfileViewModel ViewModel { get; private set; }

        public ModifyProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += ModifyProfilePage_Loaded;

        }

        private void ModifyProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new ModifyProfileViewModel(this.Frame);
            DataContext = ViewModel;

        }

    }
}