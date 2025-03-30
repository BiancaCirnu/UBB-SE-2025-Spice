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
using Microsoft.UI.Xaml.Shapes;
using SteamProfile.Data;
using SteamProfile.Repositories;
using SteamProfile.Services;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace SteamProfile
{
    public partial class App : Application
    {
        public static readonly AchievementsService AchievementsService;
        public static readonly FeaturesService FeaturesService;
        public static readonly CollectionsService CollectionsService;
        public static readonly WalletService WalletService;
        public static readonly AuthenticationService AuthenticationService;
        public static readonly UserService UserService;

        static App()
        {
            var dataLink = DataLink.Instance;

            var achievementsRepository = new AchievementsRepository(dataLink);
            var featuresRepository = new FeaturesRepository(dataLink);
            var usersRepository = new UsersRepository(dataLink);
            var collectionsRepository = new CollectionsRepository(dataLink);
            var walletRepository = new WalletRepository(dataLink, 1);

            AchievementsService = new AchievementsService(achievementsRepository);
            FeaturesService = new FeaturesService(featuresRepository);
            CollectionsService = new CollectionsService(collectionsRepository);
            WalletService = new WalletService(walletRepository, 1);
            AuthenticationService = new AuthenticationService(usersRepository);
            UserService = new UserService(usersRepository);
        }

        public App()
        {
            this.InitializeComponent();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window? m_window;
    }
}
