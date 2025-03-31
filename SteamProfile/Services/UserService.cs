using SteamProfile.Models;
using SteamProfile.Repositories;
using SteamProfile.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BCrypt.Net;
using System.Diagnostics;

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
            try
            {
                Debug.WriteLine("Getting all users");
                var users = _usersRepository.GetAllUsers();
                Debug.WriteLine($"Successfully retrieved {users.Count} users");
                return users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetAllUsers: {ex.Message}");
                throw;
            }
        }

        public User GetUserById(int userId)
        {
            try
            {
                Debug.WriteLine($"Getting user by ID: {userId}");
                var user = _usersRepository.GetUserById(userId);
                if (user != null)
                {
                    Debug.WriteLine($"Found user: {user.Username}");
                }
                else
                {
                    Debug.WriteLine($"No user found with ID: {userId}");
                }
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetUserById: {ex.Message}");
                throw;
            }
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

 
      
   
        public User GetUserByEmail(string email)
        {
            return _usersRepository.GetUserByEmail(email);
        }

        public void ValidateUserAndEmail(string email, string username)
        {
            // Check if user already exists
            var errorType = _usersRepository.CheckUserExists(email, username);

            if (!string.IsNullOrEmpty(errorType))
            {
                switch (errorType)
                {
                    case "EMAIL_EXISTS":
                        throw new EmailAlreadyExistsException(email);
                    case "USERNAME_EXISTS":
                        throw new UsernameAlreadyTakenException(username);
                    default:
                        throw new UserValidationException($"Unknown validation error: {errorType}");
                }
            }
        }


        public User? Login(string emailOrUsername, string password)
        {
            var user = _usersRepository.VerifyCredentials(emailOrUsername);
            if (user != null)
            {
                if (PasswordHasher.VerifyPassword(password, user.Password)) // Check the password against the hashed password
                { 
                    _sessionService.CreateNewSession(user);

                    // update last login time for user
                    _usersRepository.UpdateLastLogin(user.UserId);
                }
                else
                    return null;
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
