using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class GameLibraryService : IGameLibraryService
    {
        private readonly OwnedGamesRepository _ownedGamesRepository;

        public GameLibraryService(OwnedGamesRepository ownedGamesRepository)
        {
            _ownedGamesRepository = ownedGamesRepository ?? throw new ArgumentNullException(nameof(ownedGamesRepository));
        }

        public async Task<List<OwnedGame>> GetGameLibraryForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            return await Task.Run(() => _ownedGamesRepository.GetAllOwnedGames(parsedUserId));
        }

        public async Task AddGameToLibrary(string userId, string gameId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(gameId))
                throw new ArgumentException("Game ID cannot be null or empty.", nameof(gameId));

            if (!int.TryParse(userId, out int parsedUserId))
                throw new ArgumentException("Invalid user ID format.", nameof(userId));

            if (!int.TryParse(gameId, out int parsedGameId))
                throw new ArgumentException("Invalid game ID format.", nameof(gameId));

            // Validate that the game exists
            var game = await Task.Run(() => _ownedGamesRepository.GetOwnedGameById(parsedGameId));
            if (game == null)
                throw new InvalidOperationException($"Game with ID {gameId} not found.");

            // In the future, this will be replaced with actual database operations
            // For now, we just validate that the game exists in our database
            await Task.CompletedTask;
        }

        // IGameLibraryService implementation
        public async Task<List<OwnedGame>> GetGameLibrary(string userId)
        {
            return await GetGameLibraryForUser(userId);
        }

        public async Task AddGame(string userId, int gameId)
        {
            await AddGameToLibrary(userId, gameId.ToString());
        }
    }
} 