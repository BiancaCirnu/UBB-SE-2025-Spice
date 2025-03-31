using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using SteamProfile.ViewModels;

namespace SteamProfile.Views.Controls
{
    public sealed partial class ProfileInfoControl : UserControl
    {
        public static readonly DependencyProperty ViewModelProperty =
            DependencyProperty.Register(
                nameof(ViewModel),
                typeof(ProfileViewModel),
                typeof(ProfileInfoControl),
                new PropertyMetadata(null, OnViewModelChanged));

        public ProfileViewModel ViewModel
        {
            get => (ProfileViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        private static void OnViewModelChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ProfileInfoControl control)
            {
                control.DataContext = e.NewValue;
            }
        }

        public ProfileInfoControl()
        {
            this.InitializeComponent();
        }
    }
} 