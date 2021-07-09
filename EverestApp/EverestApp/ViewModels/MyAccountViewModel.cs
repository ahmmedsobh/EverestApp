using EverestApp.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class MyAccountViewModel:BaseViewModel
    {
        public Command ShareImageCommand { get; }
        public MyAccountViewModel()
        {
            ShareImageCommand = new Command(ExecuteShareImageCommand);
        }

        public string Code 
        {
            get => Preferences.Get("Code", ""); 
            set
            {
            }
        }

        public string Info
        {
            get => Preferences.Get("Info", "");
            set {}
        }

        public string AccountImage
        {
            get
            {
                var accountImage = Path.Combine(FileSystem.AppDataDirectory, "AccountImg.jpg");
                if(File.Exists(accountImage))
                {
                    return accountImage;
                }
                accountImage = $"https://www.everestexport.net/ems/{Code}.jpg";
                return accountImage;
            }
            set { }
        }

        async void ExecuteShareImageCommand()
        {
            try
            {

                string fileName = Path.Combine(FileSystem.AppDataDirectory, "AccountImg.jpg");

                if(File.Exists(fileName))
                {
                    await Share.RequestAsync(new ShareFileRequest
                    {
                        Title = "صورة الحساب",
                        File = new ShareFile(fileName)
                    });
                }
                else
                {
                    await Share.RequestAsync(new ShareTextRequest
                    {
                        Title = "صورة الحساب",
                        Uri = $"https://www.everestexport.net/ems/{Code}.jpg"
                    });
                }
                        
                
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}
