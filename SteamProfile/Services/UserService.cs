using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;
        
        public UserService(UsersRepository usersRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
        }

        public List<User> GetAllUsers()
        {
            return _usersRepository.GetAllUsers();
        }
    }
}
