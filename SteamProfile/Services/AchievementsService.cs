using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Authentication.OnlineId;
using Windows.System;
using static SteamProfile.Repositories.AchievementsRepository;

namespace SteamProfile.Services
{
    public class AchievementsService
    {
        private readonly AchievementsRepository _achievementsRepository;

        public AchievementsService(AchievementsRepository achievementsRepository)
        {
            _achievementsRepository = achievementsRepository ?? throw new ArgumentNullException(nameof(achievementsRepository));
        }

        public List<Achievement> GetAchievementsForUser(int userId)
        {
            try
            {
                return _achievementsRepository.GetAllAchievements();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving achievements for user.", ex);
            }
        }

        public void UnlockAchievementForUser(int userId)
        {
            try
            {
                int numberOfSoldGames = _achievementsRepository.GetNumberOfSoldGames(userId);
                int numberOfFriends = _achievementsRepository.GetNumberOfFriends(userId);
                int numberOfOwnedGames = _achievementsRepository.GetNumberOfOwnedGames(userId);
                int numberOfReviews = _achievementsRepository.GetNumberOfReviews(userId);
                if (numberOfSoldGames == 1 || numberOfSoldGames == 5 || numberOfSoldGames == 10 || numberOfSoldGames == 50)
                {
                    int achievementId = _achievementsRepository.GetAchievementByTypeAndCount("Sold Games", numberOfSoldGames);
                    if (!_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId);
                    }
                }
                if(numberOfFriends == 1 || numberOfFriends == 5 || numberOfFriends == 10 || numberOfFriends == 50 || numberOfFriends == 100)
                {
                    int achievementId = _achievementsRepository.GetAchievementByTypeAndCount("Friendships", numberOfFriends);
                    if (!_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId);
                    }
                }
                if(numberOfOwnedGames == 1 || numberOfOwnedGames == 5 || numberOfOwnedGames == 10 || numberOfOwnedGames == 50)
                {
                    int achievementId = _achievementsRepository.GetAchievementByTypeAndCount("Owned Games", numberOfOwnedGames);
                    if (!_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId);
                    }
                }
                if(numberOfReviews == 1 || numberOfReviews == 5 || numberOfReviews == 10 || numberOfReviews == 50)
                {
                    int achievementId = _achievementsRepository.GetAchievementByTypeAndCount("Number of Reviews", numberOfReviews);
                    if (!_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId);
                    }
                }

            }
            catch (RepositoryException ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }


        public void RemoveAchievement(int userId, int achievementId)
        {
            try
            {
                _achievementsRepository.RemoveAchievement(userId, achievementId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error removing achievement.", ex);
            }
        }

        public List<Achievement> GetUnlockedAchievementsForUser(int userId)
        {
            try
            {
                return _achievementsRepository.GetUnlockedAchievementsForUser(userId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving unlocked achievements for user.", ex);
            }
        }

        public List<Achievement> GetAllAchievements()
        {
            try
            {
                return _achievementsRepository.GetAllAchievements();
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving unlocked achievements for user.", ex);
            }
        }

        public AchievementUnlockedData GetUnlockedDataForAchievement(int userId, int achievementId)
        {
            try
            {
                return _achievementsRepository.GetUnlockedDataForAchievement(userId, achievementId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving unlocked data for achievement.", ex);
            }
        }

        public List<AchievementWithStatus> GetAchievementsWithStatusForUser(int userId)
        {
            try
            {
                return _achievementsRepository.GetAchievementsWithStatusForUser(userId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving achievements with status for user.", ex);
            }
        }

        public int GetAchievementIdByTypeAndCount(string type, int count)
        {
            try
            {
                return _achievementsRepository.GetAchievementByTypeAndCount(type, count);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving achievement ID.", ex);
            }
        }

        public int GetPointsForUnlockedAchievement(int userId, int achievementId)
        {
            try
            {
                if (_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                {
                    var achievement = _achievementsRepository.GetAllAchievements()
                        .FirstOrDefault(a => a.AchievementId == achievementId);
                    if (achievement != null)
                    {
                        return achievement.Points;
                    }
                }
                throw new ServiceException("Achievement is not unlocked or does not exist.");
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving points for unlocked achievement.", ex);
            }
        }


        public class ServiceException : Exception
        {
            public ServiceException(string message) : base(message) { }
            public ServiceException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }
}
