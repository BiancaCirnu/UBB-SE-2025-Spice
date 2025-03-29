using SteamProfile.Models;
using SteamProfile.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SteamProfile.Services
{
    public class CollectionsService
    {
        private readonly CollectionsRepository _collectionsRepository;

        public CollectionsService(CollectionsRepository collectionsRepository)
        {
            _collectionsRepository = collectionsRepository ?? throw new ArgumentNullException(nameof(collectionsRepository));
        }

        public async Task<List<Collection>> GetPublicCollectionsForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await Task.Run(() => _collectionsRepository.FetchPublicCollectionsForUser(userId));
        }

        public async Task<List<Collection>> GetPrivateCollectionsForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await Task.Run(() => _collectionsRepository.FetchPrivateCollectionsForUser(userId));
        }

        public async Task<List<Collection>> GetAllCollectionsForUser(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            return await Task.Run(() => _collectionsRepository.FetchAllCollectionsForUser(userId));
        }

        public async Task SaveCollection(string userId, Collection collection)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            // Validate the collection
            collection.Validate();

            // Ensure the collection belongs to the user
            if (collection.CollectionId != 0 && collection.UserId.ToString() != userId)
                throw new InvalidOperationException("Cannot save a collection that belongs to another user.");

            await Task.Run(() => _collectionsRepository.SaveCollection(userId, collection));
        }

        public async Task RemoveCollectionForUser(string userId, string collectionId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentException("Collection ID cannot be null or empty.", nameof(collectionId));

            await Task.Run(() => _collectionsRepository.RemoveCollectionForUser(userId, collectionId));
        }

        public async Task MakeCollectionPrivateForUser(string userId, string collectionId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentException("Collection ID cannot be null or empty.", nameof(collectionId));

            await Task.Run(() => _collectionsRepository.MakeCollectionPrivateForUser(userId, collectionId));
        }

        public async Task MakeCollectionPublicForUser(string userId, string collectionId)
        {
            if (string.IsNullOrEmpty(userId))
                throw new ArgumentException("User ID cannot be null or empty.", nameof(userId));

            if (string.IsNullOrEmpty(collectionId))
                throw new ArgumentException("Collection ID cannot be null or empty.", nameof(collectionId));

            await Task.Run(() => _collectionsRepository.MakeCollectionPublicForUser(userId, collectionId));
        }
    }
}
