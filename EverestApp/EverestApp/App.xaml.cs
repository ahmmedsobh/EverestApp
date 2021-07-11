using EverestApp.Services;
using EverestApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EverestApp
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            DependencyService.Register<CustomerService>();
            DependencyService.Register<AccountService>();
            DependencyService.Register<MainService>();
            DependencyService.Register<OrderService>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
