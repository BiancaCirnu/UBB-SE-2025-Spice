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
    public sealed class UsersRepository
    {
        private readonly DataLink _dataLink;

        public UsersRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public List<User> GetAllUsers()
        {
            try
            {
                var dataTable = _dataLink.ExecuteReader("GetAllUsers");
                return MapDataTableToUsers(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                // Log the error or handle it appropriately
                throw new RepositoryException("Failed to retrieve users from the database.", ex);
            }
        }

        public User? GetUserById(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@userId", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetUserById", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToUser(dataTable.Rows[0]) : null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve user with ID {userId} from the database.", ex);
            }
        }

        public User UpdateUser(User user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", user.UserId),
                    new SqlParameter("@email", user.Email),
                    new SqlParameter("@username", user.Username),
                    new SqlParameter("@developer", user.IsDeveloper)
                };

                var dataTable = _dataLink.ExecuteReader("UpdateUser", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    throw new RepositoryException($"User with ID {user.UserId} not found.");
                }

                return MapDataRowToUser(dataTable.Rows[0]);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to update user with ID {user.UserId}.", ex);
            }
        }

        public User CreateUser(User user)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@username", user.Username),
                    new SqlParameter("@email", user.Email),
                    new SqlParameter("@hashed_password", user.Password),
                    new SqlParameter("@developer", user.IsDeveloper),
                    new SqlParameter("@created_at", user.CreatedAt)
                };

                var dataTable = _dataLink.ExecuteReader("CreateUser", parameters);
                if (dataTable.Rows.Count == 0)
                {
                    throw new RepositoryException("Failed to create user.");
                }

                return MapDataRowToUser(dataTable.Rows[0]);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to create user.", ex);
            }
        }

        public void DeleteUser(int userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_Id", userId)
                };

                _dataLink.ExecuteNonQuery("DeleteUser", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to delete user with ID {userId}.", ex);
            }
        }

        public User? Login(string username, string password)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@Username", username),
                    new SqlParameter("@Password", password)
                };

                var dataTable = _dataLink.ExecuteReader("UserLogin", parameters);
                if (dataTable.Rows.Count > 0)
                {
                    var row = dataTable.Rows[0];
                    var user = MapDataRowToUser(row);
                    
                    // Get session ID from the result
                    var sessionId = row["session_id"] as Guid?;
                    if (sessionId.HasValue)
                    {
                        // Update the UserSession singleton with the new session
                        UserSession.Instance.UpdateSession(sessionId.Value, user);
                    }

                    return user;
                }
                return null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to login user.", ex);
            }
        }

        private static List<User> MapDataTableToUsers(DataTable dataTable)
        {
            return dataTable.AsEnumerable()
                .Select(MapDataRowToUser)
                .ToList();
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

    public class RepositoryException : Exception
    {
        public RepositoryException(string message) : base(message) { }
        public RepositoryException(string message, Exception innerException) 
            : base(message, innerException) { }
    }
}