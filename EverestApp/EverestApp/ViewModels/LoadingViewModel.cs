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
        public Command LoginCommand { get; }
        public LoadingViewModel()
        {
            //reset client login data to show login page
            //Preferences.Set("Code","");
            //Preferences.Set("Password","");
            LoginCommand = new Command(IsLoggedIn);
            IsLoggedIn();
        }
        bool loginBtnVisible;
        public bool LoginBtnVisible 
        {
            get => loginBtnVisible;
            set
            {
                SetProperty(ref loginBtnVisible, value);
            }
        }

        public string Logo 
        {
            get => "EverestApp.Resources.Images.EverestLogo.png"; 
        }
        async void IsLoggedIn()
        {
            IsBusy = true;
            LoginBtnVisible = false;
            var current = Connectivity.NetworkAccess;

            if (current != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("رسالة","تأكد من الاتصال بالانترنت","موافق");
                IsBusy = false;
                LoginBtnVisible = true;
                return;
            }

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
