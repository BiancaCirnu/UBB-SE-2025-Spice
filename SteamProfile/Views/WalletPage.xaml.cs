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
    }
}
