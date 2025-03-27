using Microsoft.UI.Xaml.Controls;
using System;
namespace SteamProfile.Services
{
    public interface INavigationService
    {
        void NavigateTo(Type pageType);
        void NavigateBack();
        bool CanGoBack { get; }
    }
} 