using Microsoft.UI.Xaml;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Storage;

namespace SteamProfile.ViewModels.ConfigurationsViewModels
{
    public class ModifyProfileViewModel : INotifyPropertyChanged
    {
        private string _description = string.Empty;
        private StorageFile _profileImage;
        private string _selectedImageName = string.Empty;

        private bool _isDescriptionValid;
        private bool _isImageValid;

        private bool _showDescriptionError;

        private string _descriptionErrorMessage = "Description is required";
        private string _imageErrorMessage = "Profile image is required";

        // Properties for data
        public string Description
        {
            get => _description;
            set
            {
                if (_description != value)
                {
                    _description = value;
                    ValidateDescription(_description);
                    OnPropertyChanged();
                }
            }
        }

        public StorageFile ProfileImage
        {
            get => _profileImage;
            set
            {
                if (_profileImage != value)
                {
                    _profileImage = value;
                    if (_profileImage != null)
                    {
                        SelectedImageName = _profileImage.Name;
                        IsImageValid = true;
                    }
                    else
                    {
                        SelectedImageName = string.Empty;
                        IsImageValid = false;
                    }
                    OnPropertyChanged();
                }
            }
        }

        public string SelectedImageName
        {
            get => _selectedImageName;
            set
            {
                if (_selectedImageName != value)
                {
                    _selectedImageName = value;
                    OnPropertyChanged();
                }
            }
        }

        // Validation properties
        public bool IsDescriptionValid
        {
            get => _isDescriptionValid;
            set
            {
                if (_isDescriptionValid != value)
                {
                    _isDescriptionValid = value;
                    OnPropertyChanged();
                    UpdateDescriptionErrorVisibility();
                }
            }
        }

        public bool IsImageValid
        {
            get => _isImageValid;
            set
            {
                if (_isImageValid != value)
                {
                    _isImageValid = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool ShowDescriptionError
        {
            get => _showDescriptionError;
            private set
            {
                if (_showDescriptionError != value)
                {
                    _showDescriptionError = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(DescriptionErrorVisibility));
                }
            }
        }

        public string DescriptionErrorMessage
        {
            get => _descriptionErrorMessage;
            set
            {
                if (_descriptionErrorMessage != value)
                {
                    _descriptionErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

        public string ImageErrorMessage
        {
            get => _imageErrorMessage;
            set
            {
                if (_imageErrorMessage != value)
                {
                    _imageErrorMessage = value;
                    OnPropertyChanged();
                }
            }
        }

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

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            // Update CanSave when any validation property changes
            if (propertyName == nameof(IsDescriptionValid) ||
                propertyName == nameof(IsImageValid))
            {
                OnPropertyChanged(nameof(CanSave));
            }
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

        // Save method
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