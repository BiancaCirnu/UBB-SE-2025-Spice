using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SteamProfile.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            this.InitializeComponent();
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                var users = App.UserService.GetAllUsers();
                UsersGrid.ItemsSource = users;
            }
            catch (Exception ex)
            {
                // In a production app, you would want to show this error in a more user-friendly way
                System.Diagnostics.Debug.WriteLine($"Error loading users: {ex.Message}");
            }
        }
    }
}
