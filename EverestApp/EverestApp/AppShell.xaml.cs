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
            Routing.RegisterRoute(nameof(OrdersPage), typeof(OrdersPage));
            Routing.RegisterRoute(nameof(AddOrderPage), typeof(AddOrderPage));
            Routing.RegisterRoute(nameof(OrderDetailsPage), typeof(OrderDetailsPage));
            Routing.RegisterRoute(nameof(MyAccountPage), typeof(MyAccountPage));
            Routing.RegisterRoute(nameof(AccountFilesPage), typeof(AccountFilesPage));
            Routing.RegisterRoute(nameof(AccountFileDetailsPage), typeof(AccountFileDetailsPage));
            Routing.RegisterRoute(nameof(ShipmentsPage), typeof(ShipmentsPage));
            Routing.RegisterRoute(nameof(ShipmentDetailsPage), typeof(ShipmentDetailsPage));
            Routing.RegisterRoute(nameof(MessagesPage), typeof(MessagesPage));
            Routing.RegisterRoute(nameof(ShipmentInfoPage), typeof(ShipmentInfoPage));
            Routing.RegisterRoute(nameof(BankAccountsPage), typeof(BankAccountsPage));
            Routing.RegisterRoute(nameof(ContactPage), typeof(ContactPage));



        }



        private async void OnMenuItemClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//LoginPage");
        }
    }
}
