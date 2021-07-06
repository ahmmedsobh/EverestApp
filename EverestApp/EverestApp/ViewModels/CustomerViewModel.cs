using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using EverestApp.Views;

namespace EverestApp.ViewModels
{
    class CustomerViewModel:BaseViewModel
    {
        public ICustomerService<Customer> CustomerService => DependencyService.Get<ICustomerService<Customer>>();

        public Command LoginCommand { get; }
        public CustomerViewModel()
        {
            LoginCommand = new Command(async() => await ExecurteLoginCommand());
        }


        string code;
        public string Code
        {
            get => code;
            set
            {
                SetProperty(ref code, value);
            }
        }

        string password;
        public string Password
        {
            get => password;
            set
            {
                SetProperty(ref password, value);
            }
        }

        string message;
        public string Message 
        {
            get => message;
            set
            {
                SetProperty(ref message, value);
            }
        }

        string messageColor;
        public string MessageColor
        {
            get => messageColor;
            set
            {
                SetProperty(ref messageColor, value);
            }
        }

        async Task ExecurteLoginCommand()
        {
            if(Code == null || Code == "")
            {
                Message = "Enter Your Code";
                messageColor = "Red";
                return;
            }

            if (Password == null || Password == "")
            {
                Message = "Enter Your Password";
                messageColor = "Red";
                return;
            }
            IsBusy = true;
            var CustomerDate = await CustomerService.GetCustomerAsync(Code,Password);
            if(CustomerDate == null)
            {
                Message = "Wrong code or password";
                MessageColor = "Red";
            }
            else
            {
                Message = "Login successfully";
                MessageColor = "Green";
                SaveCustomerData(CustomerDate);
                await Shell.Current.GoToAsync(nameof(MainPage));
            }
            IsBusy = false;

        }

        void SaveCustomerData(Customer customer)
        {
            if(customer != null)
            {
                Preferences.Set("ID", customer.ID);
                Preferences.Set("Code", customer.Code);
                Preferences.Set("Name", customer.Name);
                Preferences.Set("Password", customer.Password);
                Preferences.Set("Info", customer.Info);
            }
        }

        

       


    }

}
