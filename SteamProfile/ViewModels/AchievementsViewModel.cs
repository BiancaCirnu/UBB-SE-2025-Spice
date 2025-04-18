﻿using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.Models;
using SteamProfile.Services;
using SteamProfile.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace SteamProfile.ViewModels
{
    public partial class AchievementsViewModel : ObservableObject
    {
        private static AchievementsViewModel _instance;
        private readonly AchievementsService _achievementsService;
        private readonly UserService _userService;

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
                    _instance = new AchievementsViewModel(App.AchievementsService, App.UserService);
                }
                return _instance;
            }
        }

        private AchievementsViewModel(AchievementsService achievementsService, UserService userService)
        {
            _achievementsService = achievementsService ?? throw new ArgumentNullException(nameof(achievementsService));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
            LoadAchievements();
        }

        [RelayCommand]
        public void LoadAchievements()
        {
            _achievementsService.UnlockAchievementForUser(_userService.GetCurrentUser().UserId);

            var allAchievements = _achievementsService.GetAchievementsWithStatusForUser(_userService.GetCurrentUser().UserId); // Example userId

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

        [RelayCommand]
        private void BackToProfile()
        {
            // Get the current user's ID from the UserService
            int currentUserId = _userService.GetCurrentUser().UserId; // Adjust this line based on your UserService implementation

            // Navigate back to the Profile page with the current user ID
            NavigationService.Instance.Navigate(typeof(ProfilePage), currentUserId);
        }


    }
}
