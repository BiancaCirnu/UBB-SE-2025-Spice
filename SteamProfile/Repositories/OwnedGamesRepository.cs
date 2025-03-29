using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Repositories
{
    public class OwnedGamesRepository
    {
        private readonly DataLink _dataLink;

        public OwnedGamesRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public List<OwnedGame> GetAllOwnedGames()
        {
            try
            {
                var dataTable = _dataLink.ExecuteReader("GetAllOwnedGames");
                return MapDataTableToOwnedGames(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException("Failed to retrieve owned games from the database.", ex);
            }
        }

        public OwnedGame? GetOwnedGameById(int gameId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@gameId", gameId)
                };

                var dataTable = _dataLink.ExecuteReader("GetOwnedGameById", parameters);
                return dataTable.Rows.Count > 0 ? MapDataRowToOwnedGame(dataTable.Rows[0]) : null;
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve owned game with ID {gameId} from the database.", ex);
            }
        }

        public List<OwnedGame> GetGamesInCollection(int collectionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@collection_id", collectionId)
                };

                var dataTable = _dataLink.ExecuteReader("GetGamesInCollection", parameters);
                return MapDataTableToOwnedGames(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve games in collection {collectionId}.", ex);
            }
        }

        public void AddGameToCollection(int collectionId, int gameId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@collection_id", collectionId),
                    new SqlParameter("@game_id", gameId)
                };

                _dataLink.ExecuteReader("AddGameToCollection", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to add game {gameId} to collection {collectionId}.", ex);
            }
        }

        public void RemoveGameFromCollection(int collectionId, int gameId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@collection_id", collectionId),
                    new SqlParameter("@game_id", gameId)
                };

                _dataLink.ExecuteReader("RemoveGameFromCollection", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to remove game {gameId} from collection {collectionId}.", ex);
            }
        }

        private static List<OwnedGame> MapDataTableToOwnedGames(DataTable dataTable)
        {
            return dataTable.AsEnumerable()
                .Select(MapDataRowToOwnedGame)
                .ToList();
        }

        private static OwnedGame MapDataRowToOwnedGame(DataRow row)
        {
            return new OwnedGame
            {
                GameId = Convert.ToInt32(row["game_id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                Title = row["title"].ToString() ?? string.Empty,
                Description = row["description"]?.ToString() ?? string.Empty,
                CoverPicture = row["cover_picture"]?.ToString()
            };
        }
    }
} 