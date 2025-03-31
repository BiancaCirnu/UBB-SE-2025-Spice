using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SteamProfile.Models;
using SteamProfile.Services;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;

namespace SteamProfile.ViewModels
{
    public class CreateCollectionParams
    {
        public string Name { get; set; }
        public string CoverPicture { get; set; }
        public bool IsPublic { get; set; }
        public DateOnly CreatedAt { get; set; }
    }

    public class UpdateCollectionParams
    {
        public int CollectionId { get; set; }
        public string Name { get; set; }
        public string CoverPicture { get; set; }
        public bool IsPublic { get; set; }
    }

    public partial class CollectionsViewModel : ObservableObject
    {
        private readonly ICollectionService _collectionService;
        private readonly INavigationService _navigationService;
        private readonly int _userId;

        [ObservableProperty]
        private ObservableCollection<Collection> _collections;

        [ObservableProperty]
        private Collection _selectedCollection;

        [ObservableProperty]
        private string _errorMessage = string.Empty;

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private bool _isAllOwnedGamesCollection;

        public CollectionsViewModel(ICollectionService collectionService, INavigationService navigationService, int userId)
        {
            _collectionService = collectionService;
            _navigationService = navigationService;
            _userId = userId;
            Collections = new ObservableCollection<Collection>();
        }

        [RelayCommand]
        private async Task LoadCollectionsAsync()
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                var collections = await _collectionService.GetCollectionsAsync(_userId);
                Collections.Clear();
                foreach (var collection in collections)
                {
                    Collections.Add(collection);
                }

                // Set IsAllOwnedGamesCollection based on the current collection
                IsAllOwnedGamesCollection = false;
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Failed to load collections: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
            }
        }

        [RelayCommand]
        private void NavigateToCollectionGames(Collection collection)
        {
            _navigationService.NavigateToCollectionGames(collection.Id);
        }

        [RelayCommand]
        private void NavigateToCreateCollection()
        {
            _navigationService.NavigateToCreateCollection(_userId);
        }

        [RelayCommand]
        private void NavigateToEditCollection(Collection collection)
        {
            _navigationService.NavigateToEditCollection(collection.Id);
        }

        [RelayCommand]
        private async Task DeleteCollectionAsync(Collection collection)
        {
            try
            {
                IsLoading = true;
                ErrorMessage = string.Empty;

                await _collectionService.DeleteCollectionAsync(collection.Id);
                Collections.Remove(collection);
            }
            catch (System.Exception ex)
            {
                ErrorMessage = $"Failed to delete collection: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
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
                _collectionService.AddGameToCollection(SelectedCollection.CollectionId, gameId, _userId);
                LoadCollectionsAsync(); // Reload collections to update the UI
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
                _collectionService.RemoveGameFromCollection(SelectedCollection.CollectionId, gameId);
                LoadCollectionsAsync(); // Reload collections to update the UI
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
                _collectionService.CreateCollection(
                    _userId,
                    parameters.Name,
                    parameters.CoverPicture,
                    parameters.IsPublic,
                    parameters.CreatedAt
                );
                LoadCollectionsAsync(); // Reload collections after creation
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error creating collection: {ex.Message}");
                ErrorMessage = "Error creating collection. Please try again.";
            }
        }

        [RelayCommand]
        private void UpdateCollection(UpdateCollectionParams parameters)
        {
            try
            {
                Debug.WriteLine($"Updating collection: {parameters.Name}");
                _collectionService.UpdateCollection(
                    parameters.CollectionId,
                    _userId,
                    parameters.Name,
                    parameters.CoverPicture,
                    parameters.IsPublic
                );
                LoadCollectionsAsync(); // Reload collections after update
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating collection: {ex.Message}");
                ErrorMessage = "Error updating collection. Please try again.";
            }
        }
    }
}
