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
    class MessagesViewModel:BaseViewModel
    {
        public IMessageService<Message> MessageService => DependencyService.Get<IMessageService<Message>>();


        private Message _selectedMessage;

        public ObservableCollection<Message> Messages { get; }
        public Command LoadMessagesCommand { get; }

        public MessagesViewModel()
        {
            Messages = new ObservableCollection<Message>();
            LoadMessagesCommand = new Command(async () => await ExecuteLoadMessagesCommand());
        }

        async Task ExecuteLoadMessagesCommand()
        {
            IsBusy = true;

            try
            {
                Messages.Clear();
                var messages = await MessageService.GetMessagesAsync();
                foreach (var item in messages)
                {
                    Messages.Add(item);
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
            SelectedMessage = null;
        }

        public Message SelectedMessage
        {
            get => _selectedMessage;
            set
            {
                SetProperty(ref _selectedMessage, value);
            }
        }



        
    }
}
