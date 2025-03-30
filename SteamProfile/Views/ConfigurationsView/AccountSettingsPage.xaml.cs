using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.Services;
using SteamProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SteamProfile.ViewModels.ConfigurationsViewModels;
// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views.ConfigurationsView
{
    public sealed partial class AccountSettingsPage : Page
    {
        private AccountSettingsViewModel ViewModel { get; set; }

        public AccountSettingsPage()
        {
            InitializeComponent();
            ViewModel = new AccountSettingsViewModel(); // Ensure ViewModel is not null
            DataContext = ViewModel;
        }

        private void ValidEmail(object sender, TextChangedEventArgs e)
        {
            ViewModel.ValidateEmail(EmailText.Text);
        }
        private void ValidUsername(object sender, TextChangedEventArgs e)
        {
            ViewModel.ValidateUsername(UsernameText.Text);
        }
        private void ValidPassword(object sender, RoutedEventArgs e)
        {
            ViewModel.ValidatePassword(PasswordText.Password);
        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }
        private async void UpdatePassword_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UpdatePassword();
        }
        private async void UpdateUsername_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UpdateUsername();
        }
        private async void UpdateEmail_Click(object sender, RoutedEventArgs e)
        {
            await ViewModel.UpdateEmail();
        }
    }
}
