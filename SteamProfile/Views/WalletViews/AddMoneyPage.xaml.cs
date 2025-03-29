using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using SteamProfile.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views.WalletViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddMoneyPage : Page
    {
        public AddMoneyPage()
        {
            this.InitializeComponent();
            InvalidInput_ErrorMessage.Visibility = Visibility.Collapsed;
            UseCardButton.IsEnabled = false;
            UsePaypalButton.IsEnabled = false;

        }

        public void sumToBeAddedTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string input = sumToBeAddedTextBox.Text;
            InvalidInput_ErrorMessage.Visibility = Visibility.Collapsed;

            if (input != null&& input!="")
            {
                if (input.Length > 3)
                {
                    InvalidInput_ErrorMessage.Visibility = Visibility.Visible;
                    return;
                }
                for (int i = 0; i < input.Length; i++)
                {
                    if (!digistAsChar.Contains(input[i]))
                    {
                        InvalidInput_ErrorMessage.Visibility = Visibility.Visible;
                        return;
                    }
                }
                int sum = Int32.Parse(sumToBeAddedTextBox.Text);
                if (sum > 500)
                {
                    InvalidInput_ErrorMessage.Visibility = Visibility.Visible;
                    return;
                }
                else
                {
                    InvalidInput_ErrorMessage.Visibility = Visibility.Collapsed;
                    UseCardButton.IsEnabled = true;
                    UsePaypalButton.IsEnabled = true;
                    return;
                }
            }
                return;
        }

        private void UseCardForPayment(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WalletViews.CardPaymentPage),
                //user,
                sumToBeAddedTextBox.Text);
            
        }
        private void UsePaypalForPayment(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(WalletViews.PaypalPaymentPage),
                //user,
                sumToBeAddedTextBox.Text);

        }

        private List<char> digistAsChar = ['0', '1', '2','3','4','5','6','7','8','9']; 
    }
}
