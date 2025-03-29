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
using SteamProfile.Views.WalletViews;
using Microsoft.UI.Windowing;

namespace SteamProfile.Views
{
    public sealed partial class WalletPage : Page
    {
        private readonly WalletViewModel _viewModel;

        public WalletPage()
        {
            this.InitializeComponent();
            _viewModel = new WalletViewModel(App.WalletService);
            this.DataContext = _viewModel;
        }
        public void AddMoneyButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window();
            var rootPage = new Frame();

            rootPage.Navigate(typeof(AddMoneyPage));

            newWindow.Content = rootPage;
            newWindow.Activate();

        }
        public void AddPointsButton_Click(object sender, RoutedEventArgs e)
        {
            var newWindow = new Window();
            var rootPage = new Frame();

            rootPage.Navigate(typeof(AddPointsPage));

            newWindow.Content = rootPage;
            newWindow.Activate();

        }
    }    
}
