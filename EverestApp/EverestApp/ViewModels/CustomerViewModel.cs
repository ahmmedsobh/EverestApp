using EverestApp.Models;
using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Essentials;
using EverestApp.Views;
using System.IO;

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

        public string Logo
        {
            get => "EverestApp.Resources.Images.EverestLogo.png";
        }
        async Task ExecurteLoginCommand()
        {
            if(Code == null || Code == "")
            {
                Message = "ادخل الكود الخاص بك";
                MessageColor = "Red";
                return;
            }

            if (Password == null || Password == "")
            {
                Message = "ادخل كلمة المرور";
                MessageColor = "Red";
                return;
            }
            IsBusy = true;
            var CustomerDate = await CustomerService.GetCustomerAsync(Code,Password);
            if(CustomerDate == null)
            {
                Message = "خطأ فى الكود او كلمة المرور";
                MessageColor = "Red";
            }
            else
            {
                //Message = "Login successfully";
                //MessageColor = "Green";
                Message = "";
                SaveCustomerData(CustomerDate);
                await Shell.Current.GoToAsync($"///{nameof(MainPage)}");
            }
            IsBusy = false;

        }

        async void SaveCustomerData(Customer customer)
        {
            if(customer != null)
            {
                try
                {
                    Preferences.Set("ID", customer.ID);
                    Preferences.Set("Code", customer.Code);
                    Preferences.Set("Name", customer.Name);
                    Preferences.Set("Password", customer.Password);
                    Preferences.Set("Info", customer.Info);

                    var bytes = await ImageService.DownloadImage($"https://www.everestexport.net/ems/{customer.Code}.jpg");
                    string fileName = Path.Combine(FileSystem.AppDataDirectory, $"AccountImg.jpg");
                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    File.WriteAllBytes(fileName, bytes);
                }
                catch(Exception ex)
                {
                    
                }
                
            }
        }

        

       


    }

}
