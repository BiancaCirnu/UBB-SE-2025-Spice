using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;

namespace SteamProfile.ViewModels
{
    public class CreateCollectionParams
    {
        public string Name { get; set; }
        public string CoverPicture { get; set; }
        public bool IsPublic { get; set; }
        public DateOnly CreatedAt { get; set; }
    }

    public partial class CollectionsViewModel : ObservableObject
    {
        private readonly CollectionsService _collectionsService;
        private readonly int _userId = 1; // TODO: Get actual user ID

        [ObservableProperty]
        private ObservableCollection<Collection> _collections = new();

        [ObservableProperty]
        private Collection _selectedCollection;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        public CollectionsViewModel(CollectionsService collectionsService)
        {
            _collectionsService = collectionsService ?? throw new ArgumentNullException(nameof(collectionsService));
        }

        [RelayCommand]
        private void LoadCollections()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;
                Debug.WriteLine("Loading collections...");

                var collections = _collectionsService.GetAllCollections(_userId);
                Debug.WriteLine($"Retrieved {collections?.Count ?? 0} collections from service");
                
                Collections.Clear();
                if (collections != null)
                {
                    foreach (var collection in collections)
                    {
                        Collections.Add(collection);
                    }
                    Debug.WriteLine($"Added {Collections.Count} collections to ObservableCollection");
                }
                else
                {
                    Debug.WriteLine("Collections list is null");
                    ErrorMessage = "No collections found.";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error loading collections: {ex.Message}");
                Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                ErrorMessage = "Error loading collections. Please try again.";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void DeleteCollection(int collectionId)
        {
            try
            {
                Debug.WriteLine($"Deleting collection {collectionId}");
                _collectionsService.DeleteCollection(collectionId, _userId);
                LoadCollections(); // Reload collections after deletion
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error deleting collection: {ex.Message}");
                ErrorMessage = "Error deleting collection. Please try again.";
            }
        }

        [RelayCommand]
        private void ViewCollection(Collection collection)
        {
            try
            {
                if (collection == null)
                {
                    Debug.WriteLine("No collection selected");
                    return;
                }

                Debug.WriteLine($"Viewing collection: {collection.Name}");
                SelectedCollection = collection;
                // TODO: Navigate to collection details page
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error viewing collection: {ex.Message}");
                ErrorMessage = "Error viewing collection. Please try again.";
            }
        }

        [RelayCommand]
        private void AddGameToCollection(int gameId)
        {
            try
            {
                if (SelectedCollection == null)
                {
                    Debug.WriteLine("No collection selected");
                    return;
                }

                Debug.WriteLine($"Adding game {gameId} to collection {SelectedCollection.Name}");
                _collectionsService.AddGameToCollection(SelectedCollection.CollectionId, gameId, _userId);
                LoadCollections(); // Reload collections to update the UI
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error adding game to collection: {ex.Message}");
                ErrorMessage = "Error adding game to collection. Please try again.";
            }
        }

        [RelayCommand]
        private void RemoveGameFromCollection(int gameId)
        {
            try
            {
                if (SelectedCollection == null)
                {
                    Debug.WriteLine("No collection selected");
                    return;
                }

                Debug.WriteLine($"Removing game {gameId} from collection {SelectedCollection.Name}");
                _collectionsService.RemoveGameFromCollection(SelectedCollection.CollectionId, gameId);
                LoadCollections(); // Reload collections to update the UI
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error removing game from collection: {ex.Message}");
                ErrorMessage = "Error removing game from collection. Please try again.";
            }
        }

        [RelayCommand]
        private void CreateCollection(CreateCollectionParams parameters)
        {
            try
            {
                Debug.WriteLine($"Creating collection: {parameters.Name}");
                _collectionsService.CreateCollection(
                    _userId,
                    parameters.Name,
                    parameters.CoverPicture,
                    parameters.IsPublic,
                    parameters.CreatedAt
                );
                LoadCollections(); // Reload collections after creation
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating collection: {ex.Message}");
                ErrorMessage = "Error creating collection. Please try again.";
            }
        }
    }
}
