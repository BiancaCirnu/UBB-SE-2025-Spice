using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels.ConfigurationsViewModels;
using System;
using Windows.Storage;
using Windows.Storage.Pickers;

namespace SteamProfile.Views.ConfigurationsView
{
    public sealed partial class ModifyProfilePage : Page
    {
        public ModifyProfileViewModel ViewModel { get; } = new ModifyProfileViewModel();

        public ModifyProfilePage()
        {
            this.InitializeComponent();
            DataContext = this;
        }

        private async void PickAPhotoButton_Click(object sender, RoutedEventArgs e)
        {
            var filePicker = new FileOpenPicker();
            filePicker.ViewMode = PickerViewMode.Thumbnail;
            filePicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            filePicker.FileTypeFilter.Add(".png");

            // WinUI 3 requires setting a window handle to the picker
            // This code assumes you have a Window property or method to get the current window
            //var window = App.MainWindow;
            //var hwnd = WinRT.Interop.WindowNative.GetWindowHandle(window);
            //WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hwnd);

            //StorageFile file = await filePicker.PickSingleFileAsync();
            //if (file != null)
            //{
            //    ViewModel.ProfileImage = file;S
            //}
            //else
            //{
            //    // User canceled the operation
            //    PickAPhotoOutputTextBlock.Text = "Operation cancelled.";
            //}
        }

        private void GoToFeaturesPage_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to features page
            Frame.Navigate(typeof(FeaturesPage));
        }

        private async void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            bool saveSuccessful = await ViewModel.SaveChanges();

            if (saveSuccessful)
            {
                ContentDialog successDialog = new ContentDialog
                {
                    Title = "Success",
                    Content = "Profile updated successfully",
                    CloseButtonText = "OK"
                };

                successDialog.XamlRoot = this.XamlRoot;
                await successDialog.ShowAsync();

                // Optionally navigate back or to another page
                if (Frame.CanGoBack)
                {
                    Frame.GoBack();
                }
            }
            else
            {
                ContentDialog errorDialog = new ContentDialog
                {
                    Title = "Save Failed",
                    Content = "Please check the validation errors and try again.",
                    CloseButtonText = "OK"
                };

                errorDialog.XamlRoot = this.XamlRoot;
                await errorDialog.ShowAsync();
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            // Go back to previous page
            if (Frame.CanGoBack)
            {
                Frame.GoBack();
            }
        }
    }
}