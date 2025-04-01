using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;

namespace SteamProfile.Repositories
{
    public class UserProfilesRepository
    {
        private readonly DataLink _dataLink;

        public UserProfilesRepository(DataLink dataLink)
        {
            _dataLink = dataLink ?? throw new ArgumentNullException(nameof(dataLink));
        }

        public UserProfile? GetUserProfileByUserId(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetUserProfileByUserId", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToUserProfile(dataTable.Rows[0]) : null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve user profile with ID {userId} from the database.", ex);
            }
        }

        public UserProfile? UpdateProfile(UserProfile profile)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@profile_id", profile.ProfileId),
                    new SqlParameter("@user_id", profile.UserId),
                    new SqlParameter("@profile_picture", (object?)profile.ProfilePicture ?? DBNull.Value),
                    new SqlParameter("@bio", (object?)profile.Bio ?? DBNull.Value),
                    new SqlParameter("@equipped_frame", (object?)profile.EquippedFrame ?? DBNull.Value),
                    new SqlParameter("@equipped_hat", (object?)profile.EquippedHat ?? DBNull.Value),
                    new SqlParameter("@equipped_pet", (object?)profile.EquippedPet ?? DBNull.Value),
                    new SqlParameter("@equipped_emoji", (object?)profile.EquippedEmoji ?? DBNull.Value)
                };

                var dataTable = _dataLink.ExecuteReader("UpdateUserProfile", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToUserProfile(dataTable.Rows[0]) : null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to update profile for user {profile.UserId}.", ex);
            }
        }

        public UserProfile? CreateProfile(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("CreateUserProfile", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToUserProfile(dataTable.Rows[0]) : null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to create profile for user {userId}.", ex);
            }
        }

        private static UserProfile MapDataRowToUserProfile(DataRow row)
        {
            return new UserProfile
            {
                ProfileId = Convert.ToInt32(row["profile_id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                ProfilePicture = row["profile_picture"] != DBNull.Value ? row["profile_picture"] as string : null,
                Bio = row["bio"] != DBNull.Value ? row["bio"] as string : null,
                EquippedFrame = row["equipped_frame"] != DBNull.Value ? row["equipped_frame"] as string : null,
                EquippedHat = row["equipped_hat"] != DBNull.Value ? row["equipped_hat"] as string : null,
                EquippedPet = row["equipped_pet"] != DBNull.Value ? row["equipped_pet"] as string : null,
                EquippedEmoji = row["equipped_emoji"] != DBNull.Value ? row["equipped_emoji"] as string : null,
                LastModified = row["last_modified"] != DBNull.Value ? Convert.ToDateTime(row["last_modified"]) : DateTime.MinValue
            };
        }
    }
}