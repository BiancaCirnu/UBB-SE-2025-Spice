using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Repositories
{
    public class UsersRepository
    {
        private readonly DataLink _dataLink;

        public UsersRepository(DataLink datalink)
        {
            _dataLink = datalink;
        }

        public List<User> GetAllUsers()
        {
            var dataTable = _dataLink.ExecuteReader("GetAllUsers");
            return MapDataTableToUsers(dataTable);
        }

        private List<User> MapDataTableToUsers(DataTable dataTable)
        {
            List<User> users = new();

            foreach (DataRow row in dataTable.Rows)
            {
                users.Add(new User
                {
                    UserId = Convert.ToInt32(row["user_id"]),
                    Email = row["email"].ToString() ?? string.Empty,
                    Username = row["username"].ToString() ?? string.Empty,
                    ProfilePicture = row["profile_picture"] as string,
                    Description = row["description"] as string,
                    IsDeveloper = Convert.ToBoolean(row["developer"]),
                    CreatedAt = Convert.ToDateTime(row["created_at"]),
                    LastLogin = row["last_login"] as DateTime?
                });
            }

            return users;
        }
    }
}
