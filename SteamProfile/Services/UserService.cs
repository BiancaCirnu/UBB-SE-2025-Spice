using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;

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

        public User GetUserByEmail(string email)
        {
            return _usersRepository.GetUserByEmail(email);
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

        internal void UpdatePassword(string newPassword)
        {
            throw new NotImplementedException();
        }

        internal void Logout()
        {
            throw new NotImplementedException();
        }

        internal void DeleteAccount()
        {
            throw new NotImplementedException();
        }

        internal void UpdateEmail(string newEmail)
        {
            throw new NotImplementedException();
        }

        internal void UpdateUsername(string newUsername)
        {
            throw new NotImplementedException();
        }
    }
}
