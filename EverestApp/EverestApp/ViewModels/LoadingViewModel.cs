using EverestApp.Services;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Essentials;

namespace EverestApp.ViewModels
{
    class LoadingViewModel:BaseViewModel
    {
        IAccountService AccountService = DependencyService.Get<IAccountService>(); 
        public LoadingViewModel()
        {
            //reset client login data to show login page
            //Preferences.Set("Code","");
            //Preferences.Set("Password","");

            IsLoggedIn();
        }

        public string Logo 
        {
            get => "EverestApp.Resources.Images.EverestLogo.png"; 
        }
        async void IsLoggedIn()
        {
            var isLoggedIn = await AccountService.IsLoggedIn();
            if(isLoggedIn)
            {
               await Shell.Current.GoToAsync($"//{nameof(MainPage)}");
            }
            else
            {
                await Shell.Current.GoToAsync($"//{nameof(LoginPage)}");
            }
        }
    }
}
