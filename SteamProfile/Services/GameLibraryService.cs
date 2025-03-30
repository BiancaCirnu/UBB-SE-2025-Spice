//using SteamProfile.Models;
//using SteamProfile.Repositories;
//using System;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Linq;

//namespace SteamProfile.Services
//{
//    public class GameLibraryService : IGameLibraryService
//    {
//        private readonly OwnedGamesRepository _ownedGamesRepository;

//        public GameLibraryService(OwnedGamesRepository ownedGamesRepository)
//        {
//            _ownedGamesRepository = ownedGamesRepository ?? throw new ArgumentNullException(nameof(ownedGamesRepository));
//        }

//        public async Task<List<OwnedGame>> GetAllOwnedGames(int userId)
//        {
//            try
//            {
//                return await _ownedGamesRepository.GetAllOwnedGames(userId);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error getting all owned games: {ex.Message}");
//            }
//        }

//        public async Task<OwnedGame> GetOwnedGameById(int gameId, int userId)
//        {
//            try
//            {
//                return await _ownedGamesRepository.GetOwnedGameById(gameId, userId);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error getting owned game by ID: {ex.Message}");
//            }
//        }

//        public async Task AddOwnedGame(OwnedGame game)
//        {
//            try
//            {
//                await _ownedGamesRepository.AddOwnedGame(game);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error adding owned game: {ex.Message}");
//            }
//        }

//        public async Task RemoveOwnedGame(int gameId, int userId)
//        {
//            try
//            {
//                await _ownedGamesRepository.RemoveOwnedGame(gameId, userId);
//            }
//            catch (Exception ex)
//            {
//                throw new Exception($"Error removing owned game: {ex.Message}");
//            }
//        }

//        public async Task<List<OwnedGame>> GetGameLibraryForUser(string userId)
//        {
//            if (string.IsNullOrEmpty(userId))
//                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

//            if (!int.TryParse(userId, out int parsedUserId))
//                throw new ArgumentException("Invalid user ID format.", nameof(userId));

//            return await Task.Run(() => _ownedGamesRepository.GetAllOwnedGames(parsedUserId));
//        }

//        public async Task AddGameToLibrary(string userId, string gameId)
//        {
//            if (string.IsNullOrEmpty(userId))
//                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

//            if (string.IsNullOrEmpty(gameId))
//                throw new ArgumentException("Game ID cannot be null or empty.", nameof(gameId));

//            if (!int.TryParse(userId, out int parsedUserId))
//                throw new ArgumentException("Invalid user ID format.", nameof(userId));

//            if (!int.TryParse(gameId, out int parsedGameId))
//                throw new ArgumentException("Invalid game ID format.", nameof(gameId));

//            // Validate that the game exists
//            var game = await _ownedGamesRepository.GetOwnedGameById(parsedGameId, parsedUserId);
//            if (game == null)
//                throw new InvalidOperationException($"Game with ID {gameId} not found.");

//            // In the future, this will be replaced with actual database operations
//            // For now, we just validate that the game exists in our database
//            await Task.CompletedTask;
//        }

//        // IGameLibraryService implementation
//        public async Task<List<OwnedGame>> GetGameLibrary(string userId)
//        {
//            return await GetGameLibraryForUser(userId);
//        }

//        public async Task AddGame(string userId, int gameId)
//        {
//            await AddGameToLibrary(userId, gameId.ToString());
//        }
//    }
//} 