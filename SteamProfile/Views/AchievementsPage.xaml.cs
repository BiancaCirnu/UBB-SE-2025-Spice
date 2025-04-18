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

namespace SteamProfile.Views
{
    public sealed partial class AchievementsPage : Page
    {
        private readonly AchievementsViewModel _viewModel;
        private readonly UserService _userService;
        public AchievementsViewModel ViewModel => _viewModel;

        public AchievementsPage()
        {
            this.InitializeComponent();
            _viewModel = AchievementsViewModel.Instance;
            this.DataContext = _viewModel;
        }

       
    }
}
