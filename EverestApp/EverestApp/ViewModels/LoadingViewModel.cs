using EverestApp.Services;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class LoadingViewModel:BaseViewModel
    {
        IAccountService AccountService = DependencyService.Get<IAccountService>(); 
        public LoadingViewModel()
        {
            IsLoggedIn();
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
