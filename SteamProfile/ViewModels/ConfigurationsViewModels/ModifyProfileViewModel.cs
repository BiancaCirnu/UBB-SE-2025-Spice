using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.Storage;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public partial class ModifyProfileViewModel : ObservableObject
    {
        // Observable properties with source generation
        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
        private string _description = string.Empty;

        [ObservableProperty]
        private StorageFile _profileImage;

        [ObservableProperty]
        private string _selectedImageName = string.Empty;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        [NotifyPropertyChangedFor(nameof(DescriptionErrorVisibility))]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
        private bool _isDescriptionValid;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(CanSave))]
        [NotifyCanExecuteChangedFor(nameof(SaveChangesCommand))]
        private bool _isImageValid;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(DescriptionErrorVisibility))]
        private bool _showDescriptionError;

        [ObservableProperty]
        private string _descriptionErrorMessage = "Description is required";

        [ObservableProperty]
        private string _imageErrorMessage = "Profile image is required";

        // UI helper properties
        public Visibility DescriptionErrorVisibility => ShowDescriptionError ? Visibility.Visible : Visibility.Collapsed;

        // Save enabled state
        public bool CanSave => IsDescriptionValid && IsImageValid;

        // Constructor
        public ModifyProfileViewModel()
        {
            // Initialize validation states
            ValidateDescription(_description);
            IsImageValid = false;
            ShowDescriptionError = false;
        }

        // Property change handlers
        partial void OnDescriptionChanged(string value)
        {
            ValidateDescription(value);
        }

        partial void OnProfileImageChanged(StorageFile value)
        {
            if (value != null)
            {
                SelectedImageName = value.Name;
                IsImageValid = true;
            }
            else
            {
                SelectedImageName = string.Empty;
                IsImageValid = false;
            }
        }

        partial void OnIsDescriptionValidChanged(bool value)
        {
            UpdateDescriptionErrorVisibility();
        }

        // Validation methods
        private void UpdateDescriptionErrorVisibility()
        {
            ShowDescriptionError = !IsDescriptionValid;
        }

        public void ValidateDescription(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
            {
                IsDescriptionValid = false;
                DescriptionErrorMessage = "Description is required";
            }
            else if (description.Length < 10)
            {
                IsDescriptionValid = false;
                DescriptionErrorMessage = "Description must be at least 10 characters";
            }
            else if (description.Length > 500)
            {
                IsDescriptionValid = false;
                DescriptionErrorMessage = "Description cannot exceed 500 characters";
            }
            else
            {
                IsDescriptionValid = true;
            }
        }

        // Save method with RelayCommand
        [RelayCommand(CanExecute = nameof(CanSave))]
        public async Task<bool> SaveChanges()
        {
            // Validate all fields before saving
            ValidateDescription(Description);

            if (!CanSave)
            {
                return false;
            }

            try
            {
                // Implement your actual save logic here
                // For example:
                // - Upload image to server
                // - Save description to database
                await Task.Delay(500); // Simulating save operation

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}