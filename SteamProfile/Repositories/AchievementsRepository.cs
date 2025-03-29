using Microsoft.UI.Xaml.Documents;
using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
                }
                throw new RepositoryException("Error retrieving achievements with status for user.", ex);
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
