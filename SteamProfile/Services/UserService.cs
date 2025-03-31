using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace SteamProfile.Services
{
    public class UserService
    {
        private readonly UsersRepository _usersRepository;
        private readonly UserProfilesRepository _userProfilesRepository;
        private int? _currentUserId;

        public UserService(UsersRepository usersRepository, UserProfilesRepository userProfilesRepository)
        {
            _usersRepository = usersRepository ?? throw new ArgumentNullException(nameof(usersRepository));
            _userProfilesRepository = userProfilesRepository ?? throw new ArgumentNullException(nameof(userProfilesRepository));
            InitializeCurrentUser();
        }

        private void InitializeCurrentUser()
        {
            try
            {
                Debug.WriteLine("Starting InitializeCurrentUser");
                var users = GetAllUsers();
                Debug.WriteLine($"Found {users.Count} users");

                if (users.Any())
                {
                    // For development, use AliceGamer as the default user since she's a developer
                    var defaultUser = users.FirstOrDefault(u => u.Username == "AliceGamer");
                    if (defaultUser != null)
                    {
                        Debug.WriteLine($"Found AliceGamer with ID {defaultUser.UserId}");
                        _currentUserId = defaultUser.UserId;
                    }
                    else
                    {
                        var firstUser = users.First();
                        Debug.WriteLine($"AliceGamer not found, using first user with ID {firstUser.UserId}");
                        _currentUserId = firstUser.UserId;
                    }
                }
                else
                {
                    Debug.WriteLine("No users found in database");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in InitializeCurrentUser: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
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
            var createdUser = _usersRepository.CreateUser(user);
            if (createdUser != null)
            {
                // Create a profile for the new user
                _userProfilesRepository.CreateProfile(createdUser.UserId);
            }
            return createdUser;
        }

        public User UpdateUser(User user)
        {
            return _usersRepository.UpdateUser(user);
        }

        public void DeleteUser(int userId)
        {
            _usersRepository.DeleteUser(userId);
        }

        public User GetCurrentUser()
        {
            try
            {
                Debug.WriteLine($"Getting current user. CurrentUserId: {_currentUserId}");
                if (_currentUserId == null)
                {
                    Debug.WriteLine("No current user set, initializing");
                    InitializeCurrentUser();
                    if (_currentUserId == null)
                    {
                        Debug.WriteLine("Still no current user after initialization");
                        throw new InvalidOperationException("No users found in the database.");
                    }
                }

                var user = GetUserById(_currentUserId.Value);
                Debug.WriteLine($"Retrieved current user: {user?.Username ?? "null"}");
                return user;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error in GetCurrentUser: {ex.Message}");
                if (ex.InnerException != null)
                {
                    Debug.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
                throw;
            }
        }

        public void SetCurrentUser(int userId)
        {
            _currentUserId = userId;
        }

        public UserProfile GetUserProfile(int userId)
        {
            return _userProfilesRepository.GetProfileByUserId(userId);
        }

        public UserProfile UpdateUserProfile(UserProfile profile)
        {
            return _userProfilesRepository.UpdateProfile(profile);
        }

        public UserProfile CreateUserProfile(int userId)
        {
            return _userProfilesRepository.CreateProfile(userId);
        }
    }
}
