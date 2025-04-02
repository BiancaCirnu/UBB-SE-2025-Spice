using Microsoft.UI.Xaml.Documents;
using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography.Certificates;

namespace SteamProfile.Repositories
{
    public class AchievementsRepository
    {
        private readonly DataLink _dataLink;

        public AchievementsRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public List<Achievement> GetAllAchievements()
        {
            try
            {
                var dataTable = _dataLink.ExecuteReader("GetAllAchievements");
                return MapDataTableToAchievements(dataTable);
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving achievements.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving achievements.", ex);
            }
        }

        public List<Achievement> GetUnlockedAchievementsForUser(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", userId)
                };
                var dataTable = _dataLink.ExecuteReader("GetUnlockedAchievements", parameters);
                return MapDataTableToAchievements(dataTable);
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving unlocked achievements.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving unlocked achievements.", ex);
            }
        }

        public void UnlockAchievement(int userId, int achievementId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", userId),
                    new SqlParameter("@achievementId", achievementId)
                };
                _dataLink.ExecuteNonQuery("UnlockAchievement", parameters);
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while unlocking achievement.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while unlocking achievement.", ex);
            }
        }

        public void RemoveAchievement(int userId, int achievementId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", userId),
                    new SqlParameter("@achievementId", achievementId)
                };
                _dataLink.ExecuteNonQuery("RemoveAchievement", parameters);
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while removing achievement.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while removing achievement.", ex);
            }
        }

        public AchievementUnlockedData GetUnlockedDataForAchievement(int userId, int achievementId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId),
                    new SqlParameter("@achievement_id", achievementId)
                };
                var dataTable = _dataLink.ExecuteReader("GetUnlockedDataForAchievement", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToAchievementUnlockedData(dataTable.Rows[0]) : null;
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving achievement data.", ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Exception: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new RepositoryException("An unexpected error occurred while retrieving achievement data.", ex);
            }
        }
       
        public bool IsAchievementUnlocked(int userId, int achievementId)
        {
            try
            {
                var parameters = new SqlParameter[]
            {
                new SqlParameter("@user_id", userId),
                new SqlParameter("@achievement_id", achievementId)
            };

                var result = _dataLink.ExecuteScalar<int>("IsAchievementUnlocked", parameters);
                return result > 0;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error during ExecuteScalar operation: {ex.Message}");
                throw new RepositoryException("Error checking if achievement is unlocked.", ex);
            }
        }

        public List<AchievementWithStatus> GetAchievementsWithStatusForUser(int userId)
        {
            try
            {
                var allAchievements = GetAllAchievements();
                var achievementsWithStatus = new List<AchievementWithStatus>();

                foreach (var achievement in allAchievements)
                {
                    var isUnlocked = IsAchievementUnlocked(userId, achievement.AchievementId);
                    var unlockedData = GetUnlockedDataForAchievement(userId, achievement.AchievementId);
                    achievementsWithStatus.Add(new AchievementWithStatus
                    {
                        Achievement = achievement,
                        IsUnlocked = isUnlocked,
                        UnlockedDate = unlockedData?.UnlockDate
                    });
                }

                return achievementsWithStatus;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new RepositoryException("Error retrieving achievements with status for user.", ex);
            }
        }

        public int GetNumberOfSoldGames(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                new SqlParameter("@user_id", userId)
                };

                var result = _dataLink.ExecuteScalar<int>("GetNumberOfSoldGames", parameters);
                return result;
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving number of sold games.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving number of sold games.", ex);
            }
        }

        public int GetFriendshipCount(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };
                return _dataLink.ExecuteScalar<int>("GetFriendshipCountForUser", parameters);
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Error: {ex.Message}");
                throw new RepositoryException("Database error while retrieving friendship count.", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
                throw new RepositoryException("An unexpected error occurred while retrieving friendship count.", ex);
            }
        }

        public int GetNumberOfOwnedGames(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                new SqlParameter("@user_id", userId)
                };

                var result = _dataLink.ExecuteScalar<int>("GetNumberOfOwnedGames", parameters);
                return result;
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving number of owned games.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving number of owned games.", ex);
            }
        }

        public int GetNumberOfReviews(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                new SqlParameter("@user_id", userId)
                };

                var result = _dataLink.ExecuteScalar<int>("GetNumberOfReviews", parameters);
                return result;
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving number of reviews.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving number of reviews.", ex);
            }
        }

        public int? GetAchievementIdByName(string achievementName)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
            new SqlParameter("@achievementName", achievementName)
                };

                var result = _dataLink.ExecuteScalar<int?>("GetAchievementIdByName", parameters);
                System.Diagnostics.Debug.WriteLine($"Achievement ID for name {achievementName}: {result}");
                return result;
            }
            catch (SqlException ex)
            {
                System.Diagnostics.Debug.WriteLine($"SQL Error while retrieving achievement ID: {ex.Message}");
                throw new RepositoryException("Database error while retrieving achievement ID.", ex);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Unexpected error while retrieving achievement ID: {ex.Message}");
                throw new RepositoryException("An unexpected error occurred while retrieving achievement ID.", ex);
            }
        }

        private static List<Achievement> MapDataTableToAchievements(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(MapDataRowToAchievement).ToList();
        }

        private static Achievement MapDataRowToAchievement(DataRow row)
        {
            return new Achievement
            {
                AchievementId = Convert.ToInt32(row["achievement_id"]),
                AchievementName = row["achievement_name"].ToString() ?? string.Empty,
                Description = row["description"].ToString() ?? string.Empty,
                AchievementType = row["achievement_type"].ToString() ?? string.Empty,
                Points = Convert.ToInt32(row["points"]),
                Icon = row["icon_url"].ToString()
            };
        }

        private static AchievementUnlockedData MapDataRowToAchievementUnlockedData(DataRow row)
        {
            return new AchievementUnlockedData
            {
                AchievementName = row["AchievementName"].ToString() ?? string.Empty,
                AchievementDescription = row["AchievementDescription"].ToString() ?? string.Empty,
                UnlockDate = row["UnlockDate"] != DBNull.Value ? Convert.ToDateTime(row["UnlockDate"]) : (DateTime?)null
            };
        }
    }
}
