using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;

namespace SteamProfile.Repositories
{
    public class FriendshipsRepository
    {
        private readonly DataLink _dataLink;

        public FriendshipsRepository(DataLink dataLink)
        {
            _dataLink = dataLink ?? throw new ArgumentNullException(nameof(dataLink));
        }

        public List<Friendship> GetAllFriendships(int userId)
        {
            try
            {
                Debug.WriteLine($"Getting friends for user {userId}");
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                Debug.WriteLine("Executing GetFriendsForUser stored procedure");
                var dataTable = _dataLink.ExecuteReader("GetFriendsForUser", parameters);
                Debug.WriteLine($"Got {dataTable.Rows.Count} rows from database");

                var friendships = MapDataTableToFriendships(dataTable);
                Debug.WriteLine($"Mapped {friendships.Count} friendships");
                return friendships;
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Error: {ex.Message}");
                Debug.WriteLine($"Error Number: {ex.Number}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new RepositoryException("Database error while retrieving friendships.", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw new RepositoryException("An unexpected error occurred while retrieving friendships.", ex);
            }
        }

        public Friendship GetFriendshipById(int friendshipId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@friendshipId", friendshipId)
                };
                var dataTable = _dataLink.ExecuteReader("GetFriendshipById", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToFriendship(dataTable.Rows[0]) : null;
            }
            catch (SqlException ex)
            {
                throw new RepositoryException("Database error while retrieving friendship by ID.", ex);
            }
            catch (Exception ex)
            {
                throw new RepositoryException("An unexpected error occurred while retrieving friendship by ID.", ex);
            }
        }

        public void RemoveFriendship(int friendshipId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@friendship_id", friendshipId)
                };
                _dataLink.ExecuteNonQuery("RemoveFriend", parameters);
            }
            catch (SqlException ex)
            {
                Debug.WriteLine($"SQL Error: {ex.Message}");
                throw new RepositoryException("Database error while removing friendship.", ex);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Unexpected Error: {ex.Message}");
                throw new RepositoryException("An unexpected error occurred while removing friendship.", ex);
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
                return _dataLink.ExecuteScalar<int>("GetFriendshipCount", parameters);
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

        private static List<Friendship> MapDataTableToFriendships(DataTable dataTable)
        {
            try
            {
                Debug.WriteLine("Starting to map DataTable to Friendships");
                var friendships = dataTable.AsEnumerable().Select(row => new Friendship
                {
                    FriendshipId = Convert.ToInt32(row["friendship_id"]),
                    UserId = Convert.ToInt32(row["user_id"]),
                    FriendId = Convert.ToInt32(row["friend_id"]),
                    FriendUsername = row["friend_username"].ToString(),
                    FriendProfilePicture = row["friend_profile_picture"].ToString()
                }).ToList();
                Debug.WriteLine($"Successfully mapped {friendships.Count} friendships");
                return friendships;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error mapping DataTable: {ex.Message}");
                Debug.WriteLine($"Stack Trace: {ex.StackTrace}");
                throw;
            }
        }

        private static Friendship MapDataRowToFriendship(DataRow row)
        {
            return new Friendship
            {
                FriendshipId = Convert.ToInt32(row["friendship_id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                FriendId = Convert.ToInt32(row["friend_id"]),
                FriendUsername = row["friend_username"].ToString(),
                FriendProfilePicture = row["friend_profile_picture"].ToString()
            };
        }
    }

    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception innerException)
            : base(message, innerException) { }
    }
} 