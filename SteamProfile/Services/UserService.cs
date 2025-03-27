using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

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

        public User GetUserById(int userId)
        {
            return _usersRepository.GetUserById(userId);
        }

        public User CreateUser(User user)
        {
            return _usersRepository.CreateUser(user);
        }

        public User UpdateUser(User user)
        {
            return _usersRepository.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            _usersRepository.DeleteUser(userId);
        }

        public User? Login(string username, string password)
        {
            return _usersRepository.Login(username, password);
        }
    }
}
