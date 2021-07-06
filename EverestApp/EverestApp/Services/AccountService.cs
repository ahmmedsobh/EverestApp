using EverestApp.Models;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EverestApp.Services
{
    public class AccountService : IAccountService
    {
        ICustomerService<Customer> CustomerService => DependencyService.Get<ICustomerService<Customer>>();

        public async Task<bool> IsLoggedIn()
        {
            bool hasCode = Preferences.ContainsKey("Code");
            bool hasPassword = Preferences.ContainsKey("Password");

            if (hasCode && hasPassword)
            {
                var Code = Preferences.Get("Code", "");
                var Password = Preferences.Get("Password", "");
                var CustomerData = await CustomerService.GetCustomerAsync(Code, Password);
                if (CustomerData != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
