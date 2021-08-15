using EverestApp.Services;
using EverestApp.ViewModels;
using EverestApp.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace EverestApp
{
    public partial class App : Application
    {
        MessagesViewModel ViewModel = new MessagesViewModel(); 
        public App()
        {
            InitializeComponent();

            DependencyService.Register<CustomerService>();
            DependencyService.Register<AccountService>();
            DependencyService.Register<MainService>();
            DependencyService.Register<OrderService>();
            DependencyService.Register<AccountFilesService>();
            DependencyService.Register<ShipmentService>();
            DependencyService.Register<MessageService>();
            DependencyService.Register<MessagesFileService>();

            

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
