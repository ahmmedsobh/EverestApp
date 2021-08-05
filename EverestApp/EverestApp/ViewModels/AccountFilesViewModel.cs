using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using EverestApp.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class AccountFilesViewModel:BaseViewModel
    {
        public IAccountFilesService<AccountFile> FilesService => DependencyService.Get<IAccountFilesService<AccountFile>>();


        private AccountFile _selectedFile;

        public ObservableCollection<AccountFile> Files { get; }
        public Command LoadFilesCommand { get; }
        public Command<AccountFile> FileTapped { get; }

        public Command AddMessageCommand { get; }

        public AccountFilesViewModel()
        {
            Title = "الحسابات";
            Files = new ObservableCollection<AccountFile>();
            LoadFilesCommand = new Command(async () => await ExecuteLoadFilesCommand());

            FileTapped = new Command<AccountFile>(OnFileSelected);
            AddMessageCommand = new Command(ExecuteAddMessageCommand);

        }

        async Task ExecuteLoadFilesCommand()
        {
            IsBusy = true;

            try
            {
                Files.Clear();
                var files = await FilesService.GetAcountFilesAsync();
                
                foreach (var item in files)
                {
                    Files.Add(item);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedFile = null;
        }

        public AccountFile SelectedFile
        {
            get => _selectedFile;
            set
            {
                SetProperty(ref _selectedFile, value);
                OnFileSelected(value);
            }
        }



        async void OnFileSelected(AccountFile file)
        {
            if (file == null)
                return;

            SelectedFile = null;
            await Shell.Current.GoToAsync($"{nameof(AccountFileDetailsPage)}?{nameof(AccountFilesDetailsViewModel.FileId)}={file.ID}");
        }

        async void ExecuteAddMessageCommand()
        {
            await Shell.Current.GoToAsync(nameof(MessagesPage));
        }

        
    }
}
