using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class FriendsService : IFriendsService
    {
        private readonly FriendshipsRepository _friendshipsRepository;

        public FriendsService(FriendshipsRepository friendshipsRepository)
        {
            _friendshipsRepository = friendshipsRepository ?? throw new ArgumentNullException(nameof(friendshipsRepository));
        }

        public async Task<List<Friendship>> GetFriendsListForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            return await Task.Run(() => _friendshipsRepository.GetFriendshipsForUser(parsedUserId));
        }

        public async Task AddFriendToUser(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(friendId))
                throw new ArgumentException("Friend ID cannot be null or empty.", nameof(friendId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            if (!int.TryParse(friendId, out int parsedFriendId))
                throw new ArgumentException("Invalid friend ID format.", nameof(friendId));

            await Task.Run(() => _friendshipsRepository.AddFriend(parsedUserId, parsedFriendId));
        }

        public async Task RemoveFriendFromUser(string userId, string friendId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(friendId))
                throw new ArgumentException("Friend ID cannot be null or empty.", nameof(friendId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            if (!int.TryParse(friendId, out int parsedFriendId))
                throw new ArgumentException("Invalid friend ID format.", nameof(friendId));

            // Get the friendship ID
            var friendships = await Task.Run(() => _friendshipsRepository.GetFriendshipsForUser(parsedUserId));
            var friendship = friendships.FirstOrDefault(f => f.FriendId == parsedFriendId);
            
            if (friendship == null)
                throw new InvalidOperationException($"Friendship between user {userId} and friend {friendId} not found.");

            await Task.Run(() => _friendshipsRepository.RemoveFriend(friendship.FriendshipId));
        }

        public async Task<int> GetFriendshipCount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            return await Task.Run(() => _friendshipsRepository.GetFriendshipCount(parsedUserId));
        }

        // IFriendsService implementation
        public async Task<List<Friendship>> GetFriendsList(string userId)
        {
            return await GetFriendsListForUser(userId);
        }

        public async Task AddFriend(string userId, string friendId)
        {
            await AddFriendToUser(userId, friendId);
        }

        public async Task RemoveFriend(string userId, string friendId)
        {
            await RemoveFriendFromUser(userId, friendId);
        }
    }
}
