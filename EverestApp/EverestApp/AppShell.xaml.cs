using EverestApp.Services;
using EverestApp.ViewModels;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EverestApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            //Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            //Routing.RegisterRoute(nameof(LoginPage), typeof(LoginPage));
            //Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
            //Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
            //Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
        }

        

        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
