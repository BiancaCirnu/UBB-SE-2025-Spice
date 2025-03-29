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
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Appointments;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace SteamProfile.Views.WalletViews
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class CardPaymentPage : Page
    {
        private int  amount;
        //private User user;
        private bool validName = false;
        private bool validCardNumber = false;
        private bool validCVV = false;
        private bool validDate = false;
       
        public CardPaymentPage()
        {
            this.InitializeComponent();
            SuccessfullPayment.Visibility = Visibility.Collapsed;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            string sum  = e.Parameter as string;
            if (sum != null)
            {
                amount = Int32.Parse(sum);
            }
        }
        private void ValidName(object sender, TextChangedEventArgs e)
        {
            string name = OwnerNameTextBox.Text;
            if(name != "")
            {
                if (name.Split(' ').Length < 1)
                {
                    ErrorMessage.Visibility = Visibility.Visible;
                    return;
                }
                validName = true;
            }
            else
            {
                validName = false;
            }
                return;
        }

        // Validating Card Number TextBox (CardNumberTextBox)
        private void ValidNumber(object sender, TextChangedEventArgs e)
        {
            string cardNumber = CardNumberTextBox.Text;
            if (cardNumber == "")
            {
                validCardNumber = false;
                return;
            }

            bool isValidCardNumber = Regex.IsMatch(cardNumber, @"^\d{16}$"); // Simple check for 16-digit card number

            if (!isValidCardNumber)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                validCardNumber = false;
                return;
            }
            else
            {
                validCardNumber = true;
                ErrorMessage.Visibility = Visibility.Collapsed;
            }

        }

        // Validating CVV TextBox (CVVTextBox)
        private void ValidCVV(object sender, TextChangedEventArgs e)
        {
            string cvv = CVVTextBox.Text;
            if (cvv == "")
            {
                validCVV = false;
                return;
            }
            bool isValidCVV = Regex.IsMatch(cvv, @"^\d{3}$"); // Simple check for 3-digit CVV

            if (!isValidCVV)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                validCVV = false;
                return;
            }
            else
            {
                validCVV = true;
                ErrorMessage.Visibility = Visibility.Collapsed;
            }
        }

        // Validating Expiration Date TextBox (ExpirationDateTextBox)
        private void ValidDate(object sender, TextChangedEventArgs e)
        {
            string expirationDate = ExpirationDateTextBox.Text;
            bool isValidDateFormat = Regex.IsMatch(expirationDate, @"^(0[1-9]|1[0-2])\/\d{2}$"); // MM/YY format check

            if (!isValidDateFormat)
            {
                ErrorMessage.Visibility = Visibility.Visible;
                return;
            }
            else
            {
                string[] date = expirationDate.Split('/');
                int currentMonth = System.DateTime.Today.Month;
                int currentYear = System.DateTime.Today.Year%100;
                if (currentYear > Int32.Parse(date[1])||(currentYear == Int32.Parse(date[0]) && currentMonth > Int32.Parse(date[0]))){
                    ErrorMessage.Visibility = Visibility.Visible;
                    return;
                }
                validDate = true;
                ErrorMessage.Visibility = Visibility.Collapsed;

            }
        }

        // Button Click Event (AddMoneyToAccount)
        private void AddMoneyToAccount(object sender, RoutedEventArgs e)
        {
            
            if (IsValidPaymentDetails())
            {
                // process and go back
                SuccessfullPayment.Visibility = Visibility.Visible;
                System.Threading.Thread.Sleep(5000);
                Frame.GoBack();
            }
            else
            {
                ErrorMessage.Visibility = Visibility.Visible;
            }
        }
        private void CancelPayment(object sender, RoutedEventArgs e)
        {
            Frame.GoBack();
        }

        // Helper method to check if all payment details are valid
        private bool IsValidPaymentDetails()
        {
            return validDate && validCVV && validCardNumber && validName;
        }


    }
}
