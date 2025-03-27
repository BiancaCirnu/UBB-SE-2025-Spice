using Microsoft.UI.Xaml.Controls;
using System;

namespace SteamProfile.Services
{
    public sealed class NavigationService
    {
        private static NavigationService? _instance;
        private static readonly object _lock = new object();
        private Frame? _frame;

        private NavigationService() { }

        public static NavigationService Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new NavigationService();
                    }
                    return _instance;
                }
            }
        }

        public void InitializeFrame(Frame frame)
        {
            lock (_lock)
            {
                _frame = frame;
            }
        }

        public void NavigateTo(Type pageType)
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("NavigationService has not been initialized with a Frame.");
            }
            _frame.Navigate(pageType);
        }

        public void NavigateBack()
        {
            if (_frame?.CanGoBack == true)
            {
                _frame.GoBack();
            }
        }

        public bool CanGoBack => _frame?.CanGoBack ?? false;
    }
} 