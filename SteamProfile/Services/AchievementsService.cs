using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public void UnlockAchievement(int userId, int achievementId)
        {
            try
            {
                if (!_achievementsRepository.IsAchievementUnlocked(userId, achievementId))
                {
                    _achievementsRepository.UnlockAchievement(userId, achievementId);
                }
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error unlocking achievement.", ex);
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


        public class ServiceException : Exception
        {
            public ServiceException(string message) : base(message) { }
            public ServiceException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }
}
