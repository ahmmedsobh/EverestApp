using EverestApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;

namespace EverestApp.ViewModels
{
    class MainPageViewModel:BaseViewModel
    {
        public Customer Customer { get; set; }
        public MainPageViewModel()
        {
            GetCustomerDate();
        }

        void GetCustomerDate()
        {
            Customer = new Customer();
            Customer.ID = Preferences.Get("ID", "");
            Customer.Code = Preferences.Get("Code", "");
            Customer.Name = Preferences.Get("Name", "");
            Customer.Password = Preferences.Get("Password", "");
            Customer.Info = Preferences.Get("Info", "");
        }
    }
}
