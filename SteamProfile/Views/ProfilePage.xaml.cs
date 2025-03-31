using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using SteamProfile.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SteamProfile.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ProfilePage : Page
    {
        public ProfileViewModel ViewModel { get; private set; }

        public ProfilePage()
        {
            this.InitializeComponent();
            this.Loaded += ProfilePage_Loaded;
        }

        private void ProfilePage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModel = new ProfileViewModel(App.UserService, this.Frame);
            this.DataContext = ViewModel;
        }

        // Add this new method for Configurations button
        private void ConfigurationsButton_Click(object sender, RoutedEventArgs e)
        {
            // Direct navigation to ConfigurationsPage
            this.Frame.Navigate(typeof(ConfigurationsPage));
        }
    }
}