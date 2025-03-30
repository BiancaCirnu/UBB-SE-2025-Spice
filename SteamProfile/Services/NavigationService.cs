using Microsoft.UI.Xaml.Controls;
using System;

namespace SteamProfile.Services
{
    public class NavigationService
    {
        private Frame _frame;
        private static NavigationService _instance;

        public static NavigationService Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new NavigationService();
                }
                return _instance;
            }
        }

        public void Initialize(Frame frame)
        {
            _frame = frame;
        }

        public bool Navigate(Type pageType)
        {
            if (_frame != null)
            {
                return _frame.Navigate(pageType);
            }
            return false;
        }

        public bool Navigate(Type pageType, object parameter)
        {
            if (_frame != null)
            {
                return _frame.Navigate(pageType, parameter);
            }
            return false;
        }

        public bool GoBack()
        {
            if (_frame != null && _frame.CanGoBack)
            {
                _frame.GoBack();
                return true;
            }
            return false;
        }
    }
}