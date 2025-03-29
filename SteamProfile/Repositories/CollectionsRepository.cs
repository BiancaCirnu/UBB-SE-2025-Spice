using SteamProfile.Data;
using SteamProfile.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SteamProfile.Repositories
{
    public class CollectionsRepository
    {
        private readonly DataLink _dataLink;

        public CollectionsRepository(DataLink datalink)
        {
            _dataLink = datalink ?? throw new ArgumentNullException(nameof(datalink));
        }

        public List<Collection> FetchPublicCollectionsForUser(string userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetPublicCollectionsForUser", parameters);
                return MapDataTableToCollections(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve public collections for user {userId}.", ex);
            }
        }

        public List<Collection> FetchPrivateCollectionsForUser(string userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetPrivateCollectionsForUser", parameters);
                return MapDataTableToCollections(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve private collections for user {userId}.", ex);
            }
        }

        public List<Collection> FetchAllCollectionsForUser(string userId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId)
                };

                var dataTable = _dataLink.ExecuteReader("GetAllCollectionsForUser", parameters);
                return MapDataTableToCollections(dataTable);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to retrieve all collections for user {userId}.", ex);
            }
        }

        public void MakeCollectionPrivateForUser(string userId, string collectionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId),
                    new SqlParameter("@collection_id", collectionId)
                };

                _dataLink.ExecuteReader("MakeCollectionPrivate", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to make collection {collectionId} private for user {userId}.", ex);
            }
        }

        public void MakeCollectionPublicForUser(string userId, string collectionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId),
                    new SqlParameter("@collection_id", collectionId)
                };

                _dataLink.ExecuteReader("MakeCollectionPublic", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to make collection {collectionId} public for user {userId}.", ex);
            }
        }

        public void RemoveCollectionForUser(string userId, string collectionId)
        {
            try
            {
                var parameters = new SqlParameter[]
                {
                    new SqlParameter("@user_id", userId),
                    new SqlParameter("@collection_id", collectionId)
                };

                _dataLink.ExecuteReader("DeleteCollection", parameters);
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to remove collection {collectionId} for user {userId}.", ex);
            }
        }

        public void SaveCollection(string userId, Collection collection)
        {
            try
            {
                if (collection.CollectionId == 0)
                {
                    // Create new collection
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@user_id", userId),
                        new SqlParameter("@name", collection.Name),
                        new SqlParameter("@cover_picture", collection.CoverPicture),
                        new SqlParameter("@is_public", collection.IsPublic),
                        new SqlParameter("@created_at", collection.CreatedAt.ToDateTime(TimeOnly.MinValue))
                    };

                    _dataLink.ExecuteReader("CreateCollection", parameters);
                }
                else
                {
                    // Update existing collection
                    var parameters = new SqlParameter[]
                    {
                        new SqlParameter("@collection_id", collection.CollectionId),
                        new SqlParameter("@user_id", userId),
                        new SqlParameter("@name", collection.Name),
                        new SqlParameter("@cover_picture", collection.CoverPicture),
                        new SqlParameter("@is_public", collection.IsPublic),
                        new SqlParameter("@created_at", collection.CreatedAt.ToDateTime(TimeOnly.MinValue))
                    };

                    _dataLink.ExecuteReader("UpdateCollection", parameters);
                }
            }
            catch (DatabaseOperationException ex)
            {
                throw new RepositoryException($"Failed to save collection for user {userId}.", ex);
            }
        }

        private static List<Collection> MapDataTableToCollections(DataTable dataTable)
        {
            return dataTable.AsEnumerable()
                .Select(MapDataRowToCollection)
                .ToList();
        }

        private static Collection MapDataRowToCollection(DataRow row)
        {
            return new Collection
            {
                CollectionId = Convert.ToInt32(row["collection_id"]),
                UserId = Convert.ToInt32(row["user_id"]),
                Name = row["name"].ToString() ?? string.Empty,
                CoverPicture = row["cover_picture"]?.ToString(),
                IsPublic = Convert.ToBoolean(row["is_public"]),
                CreatedAt = DateOnly.FromDateTime(Convert.ToDateTime(row["created_at"]))
            };
        }
    }
}
