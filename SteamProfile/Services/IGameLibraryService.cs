using SteamProfile.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public interface IGameLibraryService
    {
        Task<List<OwnedGame>> GetGameLibrary(string userId);
        Task AddGame(string userId, int gameId);
    }
} 