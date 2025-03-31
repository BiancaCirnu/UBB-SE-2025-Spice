using SteamProfile.Data;
using SteamProfile.Data.Exceptions;
using SteamProfile.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SteamProfile.Repositories
{
    public class UserProfilesRepository
    {
        private readonly DataLink _dataLink;

        public UserProfilesRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public UserProfile GetProfileByUserId(int userId)
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
            catch (SteamProfile.Data.Exceptions.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve profile for user {userId}.", ex);
            }
        }

        public UserProfile UpdateProfile(UserProfile profile)
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
            catch (SteamProfile.Data.Exceptions.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to update profile for user {profile.UserId}.", ex);
            }
        }

        public UserProfile CreateProfile(int userId)
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
            catch (SteamProfile.Data.Exceptions.DatabaseOperationException ex)
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
                ProfilePicture = row["profile_picture"] as string,
                Bio = row["bio"] as string,
                EquippedFrame = row["equipped_frame"] as string,
                EquippedHat = row["equipped_hat"] as string,
                EquippedPet = row["equipped_pet"] as string,
                EquippedEmoji = row["equipped_emoji"] as string,
                LastModified = Convert.ToDateTime(row["last_modified"])
            };
        }
    }
}
