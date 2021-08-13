using EverestApp.Models;
using EverestApp.Services;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    [QueryProperty(nameof(FileId), nameof(FileId))]

    class AccountFilesDetailsViewModel :BaseViewModel
    {
        public IAccountFilesService<AccountFile> FileService => DependencyService.Get<IAccountFilesService<AccountFile>>();

        public Command OpenFileCommand { get;}
        public AccountFilesDetailsViewModel()
        {
            OpenFileCommand = new Command(ExecuteOpenFileCommand);
        }

        string fileId;
        public string FileId
        {
            get => fileId;
            set
            {
                SetProperty(ref fileId, value);
                GetFileDetails(value);
            }
        }

        AccountFile file;
        public AccountFile File
        {
            get => file;
            set
            {
                SetProperty(ref file, value);
            }
        }


        async void GetFileDetails(string FileId)
        {
            IsBusy = true;
            var files = await FileService.GetAcountFilesAsync();
            var file = files.FirstOrDefault(o => o.ID == FileId);
            File = new AccountFile()
            {
                Index = file.Index,
                ID = file.ID,
                CustomerID = file.CustomerID,
                UploadDate = file.UploadDate,
                UpdateDate = file.UpdateDate,
            };
            IsBusy = false;
        }

        async void ExecuteOpenFileCommand()
        {
            await FileService.OpenPdf(File.ID);
        }

       
    }
}
