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

        public string GeneratePasswordResetCode(string email)
        {
            var user = _usersRepository.GetUserByEmail(email);
            if (user == null) return null;

            // Generate a random 6-digit code
            var resetCode = new Random().Next(100000, 999999).ToString();
            
            // Store the reset code in the database
            _usersRepository.StoreResetCode(user.UserId, resetCode);
            
            return resetCode;
        }

        public bool VerifyResetCode(string email, string resetCode)
        {
            return _usersRepository.VerifyResetCode(email, resetCode);
        }

        public bool ResetPassword(string email, string resetCode, string newPassword)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
            return _usersRepository.ResetPassword(email, resetCode, hashedPassword);
        }
    }
}
