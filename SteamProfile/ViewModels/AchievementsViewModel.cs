using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SteamProfile.ViewModels
{
    public partial class AchievementsViewModel : ObservableObject
    {
        private static AchievementsViewModel _instance;
        private readonly AchievementsService _achievementsService;

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _allAchievements = new ObservableCollection<AchievementWithStatus>();

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _friendshipsAchievements = new ObservableCollection<AchievementWithStatus>();

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _ownedGamesAchievements = new ObservableCollection<AchievementWithStatus>();

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _soldGamesAchievements = new ObservableCollection<AchievementWithStatus>();

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _yearsOfActivityAchievements = new ObservableCollection<AchievementWithStatus>();

        [ObservableProperty]
        private ObservableCollection<AchievementWithStatus> _numberOfPostsAchievements = new ObservableCollection<AchievementWithStatus>();

        public static AchievementsViewModel Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new AchievementsViewModel(App.AchievementsService);
                }
                return _instance;
            }
        }

        private AchievementsViewModel(AchievementsService achievementsService)
        {
            _achievementsService = achievementsService ?? throw new ArgumentNullException(nameof(achievementsService));
            LoadAchievements();
        }

        [RelayCommand]
        public void LoadAchievements()
        {
            _achievementsService.UnlockAchievementForUser(1);

            var allAchievements = _achievementsService.GetAchievementsWithStatusForUser(1); // Example userId

            AllAchievements.Clear();

            foreach (var achievement in allAchievements)
            {
                AllAchievements.Add(achievement);
            }

            LoadCategoryAchievements(FriendshipsAchievements, "Friendships");
            LoadCategoryAchievements(OwnedGamesAchievements, "Owned Games");
            LoadCategoryAchievements(SoldGamesAchievements, "Sold Games");
            LoadCategoryAchievements(NumberOfPostsAchievements, "Number of Reviews");
        }

        private void LoadCategoryAchievements(ObservableCollection<AchievementWithStatus> collection, string category)
        {
            var achievements = AllAchievements.Where(a => a.Achievement.AchievementType == category).ToList();
            collection.Clear();
            foreach (var achievement in achievements)
            {
                collection.Add(achievement);
            }
        }

    }
}
