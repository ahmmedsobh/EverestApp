using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class ContactViewModel:BaseViewModel
    {
        public Command<string> OpenPhoneNumberCommand { get;}
        public Command<string> OpenEmailCommand { get; }
        public Command<string> OpenLinkCommand { get; }
        public Command OpenAddressCommand { get; }


        public ContactViewModel()
        {
            OpenEmailCommand = new Command<string>(OpenEmail);
            OpenPhoneNumberCommand = new Command<string>(OpenPhoneNumber);
            OpenLinkCommand = new Command<string>(OpenLink);
            OpenAddressCommand = new Command(OpenAddress);
        }

        void OpenPhoneNumber(string number)
        {
            try
            {
                PhoneDialer.Open(number);
            }
            catch (ArgumentNullException anEx)
            {
                // Number was null or white space
            }
            catch (FeatureNotSupportedException ex)
            {
                // Phone Dialer is not supported on this device.
            }
            catch (Exception ex)
            {
                // Other error has occurred.
            }
        }
        async void OpenEmail(string email)
        {
            try
            {

                List<string> recipients = new List<string>()
                {
                    email
                };
                var message = new EmailMessage
                {
                    Subject = "",
                    Body = "",
                    To = recipients,
                };
                await Email.ComposeAsync(message);
            }
            catch (FeatureNotSupportedException fbsEx)
            {
                // Email is not supported on this device
            }
            catch (Exception ex)
            {
                // Some other exception occurred
            }
        }

        async void OpenLink(string link)
        {
            await Browser.OpenAsync(link);
        }

        async void OpenAddress()
        {
            var location = new Location(41.009840, 28.930420);
            var options = new MapLaunchOptions { Name = "H. Edip Adıvar, Sultan Sk. No:22 Şişli/Istanbul, Turkey" };

            try
            {
                await Map.OpenAsync(location, options);
            }
            catch (Exception ex)
            {
                // No map application available to open
            }
        }
    }
}
