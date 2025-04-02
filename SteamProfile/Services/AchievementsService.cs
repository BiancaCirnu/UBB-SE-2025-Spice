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
                int numberOfSoldGames = 0;
                int numberOfFriends = 0;
                int numberOfOwnedGames = 0;
                int numberOfReviewsGiven = 0;
                int numberOfReviewsReceived = 0;
                int numberOfPosts = 0;
                try
                {
                    numberOfFriends = _achievementsRepository.GetFriendshipCount(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving friendship count: {ex.Message}");
                }
                if (numberOfFriends == 1 || numberOfFriends == 5 || numberOfFriends == 10 || numberOfFriends == 50 || numberOfFriends == 100)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Friendships", numberOfFriends);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
                    }
                }
                try
                {
                    numberOfOwnedGames = _achievementsRepository.GetNumberOfOwnedGames(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving owned games count: {ex.Message}");
                }
                if(numberOfOwnedGames == 1 || numberOfOwnedGames == 5 || numberOfOwnedGames == 10 || numberOfOwnedGames == 50)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Owned Games", numberOfOwnedGames);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
                    }
                }
                try
                {
                    numberOfSoldGames = _achievementsRepository.GetNumberOfSoldGames(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving sold games count: {ex.Message}");
                }
                if(numberOfSoldGames == 1 || numberOfSoldGames == 5 || numberOfSoldGames == 10 || numberOfSoldGames == 50)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Sold Games", numberOfSoldGames);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
                    }
                }
                try
                {
                    numberOfReviewsGiven = _achievementsRepository.GetNumberOfReviewsGiven(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving reviews given count: {ex.Message}");
                }
                if(numberOfReviewsGiven == 1 || numberOfReviewsGiven == 5 || numberOfReviewsGiven == 10 || numberOfReviewsGiven == 50)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Number of Reviews Given", numberOfReviewsGiven);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
                    }
                }
                try
                {
                    numberOfReviewsReceived = _achievementsRepository.GetNumberOfReviewsReceived(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving reviews received count: {ex.Message}");
                }
                if (numberOfReviewsReceived == 1 || numberOfReviewsReceived == 5 || numberOfReviewsReceived == 10 || numberOfReviewsReceived == 50)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Number of Reviews Received", numberOfReviewsReceived);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
                    }
                }
                try
                {
                    numberOfPosts = _achievementsRepository.GetNumberOfPosts(userId);
                }
                catch (RepositoryException ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error retrieving posts count: {ex.Message}");
                }
                if(numberOfPosts == 1 || numberOfPosts == 5 || numberOfPosts == 10 || numberOfPosts == 50)
                {
                    int? achievementId = GetAchievementIdByTypeAndCount("Number of Posts", numberOfPosts);
                    if (achievementId.HasValue && !_achievementsRepository.IsAchievementUnlocked(userId, achievementId.Value))
                    {
                        _achievementsRepository.UnlockAchievement(userId, achievementId.Value);
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

        public int? GetAchievementIdByTypeAndCount(string type, int count)
        {
            if (type == "Friendships")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("FRIENDSHIP1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("FRIENDSHIP2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("FRIENDSHIP3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("FRIENDSHIP4");
                else if (count == 100)
                    return _achievementsRepository.GetAchievementIdByName("FRIENDSHIP5");
            }
            else if (type == "Owned Games")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("OWNEDGAMES1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("OWNEDGAMES2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("OWNEDGAMES3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("OWNEDGAMES4");
            }
            else if (type == "Sold Games")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("SOLDGAMES1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("SOLDGAMES2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("SOLDGAMES3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("SOLDGAMES4");
            }
            else if (type == "Number of Reviews Given")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("REVIEW1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("REVIEW2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("REVIEW3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("REVIEW4");
            }
            else if (type == "Number of Reviews Received")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("REVIEWR1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("REVIEWR2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("REVIEWR3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("REVIEWR4");
            }
            else if (type == "Number of Posts")
            {
                if (count == 1)
                    return _achievementsRepository.GetAchievementIdByName("POSTS1");
                else if (count == 5)
                    return _achievementsRepository.GetAchievementIdByName("POSTS2");
                else if (count == 10)
                    return _achievementsRepository.GetAchievementIdByName("POSTS3");
                else if (count == 50)
                    return _achievementsRepository.GetAchievementIdByName("POSTS4");
            }

            return null;
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
