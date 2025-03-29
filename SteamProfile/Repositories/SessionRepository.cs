using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace SteamProfile.Repositories
{
    public sealed class SessionRepository
    {
        private readonly DataLink _dataLink;

        public SessionRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public Guid CreateSession(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("CreateSession", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    throw new RepositoryException("Failed to create session.");
                }

                return Guid.Parse(dataTable.Rows[0]["session_id"].ToString());
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to create session.", ex);
            }
        }

        public void DeleteSession(Guid sessionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@session_id", sessionId)
                };

                _dataLink.ExecuteNonQuery("DeleteSession", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to delete session {sessionId}.", ex);
            }
        }

        public User? GetUserFromSession(Guid sessionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@session_id", sessionId)
                };

                var dataTable = _dataLink.ExecuteReader("GetUserFromSession", parameters);
                if (dataTable.Rows.Count > 0)
                {
                    return MapDataRowToUser(dataTable.Rows[0]);
                }
                return null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to get user from session {sessionId}.", ex);
            }
        }

        public (int UserId, DateTime CreatedAt, DateTime ExpiresAt)? GetSessionById(Guid sessionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@session_id", sessionId)
                };

                var dataTable = _dataLink.ExecuteReader("GetSessionById", parameters);
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    return (
                        UserId: Convert.ToInt32(row["user_id"]),
                        CreatedAt: Convert.ToDateTime(row["created_at"]),
                        ExpiresAt: Convert.ToDateTime(row["expires_at"])
                    );
                }
                return null; // Return null if no session found
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to get session {sessionId}.", ex);
            }
        }

        private static User MapDataRowToUser(DataRow row)
        {
            return new User
            {
                UserId = Convert.ToInt32(row["user_id"]),
                Email = row["email"].ToString() ?? string.Empty,
                Username = row["username"].ToString() ?? string.Empty,
                IsDeveloper = Convert.ToBoolean(row["developer"]),
                CreatedAt = Convert.ToDateTime(row["created_at"]),
                LastLogin = row["last_login"] as DateTime?
            };
        }
    }
} 