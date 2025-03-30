using SteamProfile.Data;
using SteamProfile.Data.Exceptions;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace SteamProfile.Repositories
{
    public class FriendshipsRepository
    {
        private readonly DataLink _dataLink;

        public FriendshipsRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public List<Friendship> GetAllFriendships()
        {
            try
            {
                var dataTable = _dataLink.ExecuteReader("GetAllFriendships");
                return MapDataTableToFriendships(dataTable);
            }
            catch (Data.DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to retrieve friendships from the database.", ex);
            }
        }

        public List<Friendship> GetFriendshipsForUser(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetFriendshipsForUser", parameters);
                return MapDataTableToFriendships(dataTable);
            }
            catch (Data.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve friendships for user {userId}.", ex);
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

                var dataTable = _dataLink.ExecuteReader("GetFriendCount", parameters);
                return Convert.ToInt32(dataTable.Rows[0]["friend_count"]);
            }
            catch (Data.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to get friendship count for user {userId}.", ex);
            }
        }

        public void AddFriend(int userId, int friendId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId),
                    new SqlParameter("@friend_id", friendId)
                };

                _dataLink.ExecuteReader("AddFriend", parameters);
            }
            catch (Data.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to add friend {friendId} for user {userId}.", ex);
            }
        }

        public void RemoveFriend(int friendshipId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@friendship_id", friendshipId)
                };

                _dataLink.ExecuteReader("RemoveFriend", parameters);
            }
            catch (Data.DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to remove friendship {friendshipId}.", ex);
            }
        }

        private List<Friendship> MapDataTableToFriendships(DataTable dataTable)
        {
            return dataTable.AsEnumerable().Select(row => new Friendship
            {
                FriendshipId = row.Field<int>("friendship_id"),
                UserId = row.Field<int>("user_id"),
                FriendId = row.Field<int>("friend_id")
            }).ToList();
        }
    }
}