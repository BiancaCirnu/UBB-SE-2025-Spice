using Microsoft.UI.Xaml.Controls;
using System;

namespace SteamProfile.Services
{
    public class NavigationService
    {
        private static Frame _frame;

        public static void Initialize(Frame frame)
        {
            _frame = frame;
        }

        public static bool Navigate(Type pageType)
        {
            if (_frame == null)
                return false;

            return _frame.Navigate(pageType);
        }

        public static bool Navigate(Type pageType, object parameter)
        {
            if (_frame == null)
                return false;

            return _frame.Navigate(pageType, parameter);
        }

        public static bool GoBack()
        {
            if (_frame == null || !_frame.CanGoBack)
                return false;

            _frame.GoBack();
            return true;
        }
    }
} 