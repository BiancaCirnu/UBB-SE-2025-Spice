using SteamProfile.Models;
using SteamProfile.Repositories;
using SteamProfile.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamProfile.Services
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;
        private readonly SessionService _sessionService;

        public UserService(UsersRepository usersRepository, SessionService sessionService)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _sessionService = sessionService ?? throw new ArgumentNullException(nameof(sessionService));
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
            // Hash the password before passing it to the repository
            user.Password = PasswordHasher.HashPassword(user.Password);

            // Check if user already exists
            var errorType = _usersRepository.CheckUserExists(user.Email, user.Username);

            if (!string.IsNullOrEmpty(errorType))
            {
                switch (errorType)
                {
                    case "EMAIL_EXISTS":
                        throw new EmailAlreadyExistsException(user.Email);
                    case "USERNAME_EXISTS":
                        throw new UsernameAlreadyTakenException(user.Username);
                    default:
                        throw new UserValidationException($"Unknown validation error: {errorType}");
                }
            }

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

        public User? Login(string emailOrUsername, string password)
        {
            // Hash the password before passing it to the repository
            var hashedPassword = PasswordHasher.HashPassword(password);
            var user = _usersRepository.VerifyCredentials(emailOrUsername, hashedPassword);
            if (user != null)
            {
                _sessionService.CreateNewSession(user);
            }
            return user;
        }

        public void Logout()
        {
            _sessionService.EndSession();
        }

        public User? GetCurrentUser()
        {
            return _sessionService.GetCurrentUser();
        }

        public bool IsUserLoggedIn()
        {
            return _sessionService.IsUserLoggedIn();
        }
    }
}
