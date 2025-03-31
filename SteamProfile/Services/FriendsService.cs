using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SteamProfile.Services
{
    public class FriendsService
    {
        private readonly FriendshipsRepository _friendshipsRepository;

        public FriendsService(FriendshipsRepository friendshipsRepository)
        {
            _friendshipsRepository = friendshipsRepository ?? throw new ArgumentNullException(nameof(friendshipsRepository));
        }

        public List<Friendship> GetAllFriendships(int userId)
        {
            try
            {
                return _friendshipsRepository.GetAllFriendships(userId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving friendships for user.", ex);
            }
        }

        public void RemoveFriend(int friendshipId)
        {
            try
            {
                _friendshipsRepository.RemoveFriendship(friendshipId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error removing friend.", ex);
            }
        }

        public int GetFriendshipCount(int userId)
        {
            try
            {
                return _friendshipsRepository.GetFriendshipCount(userId);
            }
            catch (RepositoryException ex)
            {
                throw new ServiceException("Error retrieving friendship count.", ex);
            }
        }

        public class ServiceException : Exception
        {
            public ServiceException(string message) : base(message) { }
            public ServiceException(string message, Exception innerException)
                : base(message, innerException) { }
        }
    }
}
