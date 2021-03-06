using EverestApp.Helpers;
using EverestApp.Models;
using EverestApp.Services;
using EverestApp.Views;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace EverestApp.ViewModels
{
    class MessagesViewModel:BaseViewModel
    {
        public IMessageService<Message> MessageService => DependencyService.Get<IMessageService<Message>>();
        public IMessagesFileService<Message> MessagesFileService => DependencyService.Get<IMessagesFileService<Message>>();


        private Message _selectedMessage;

        public ObservableCollection<Message> Messages { get; }
        public Command LoadMessagesCommand { get; }
        public Command AddMessagesCommand { get; }
        public Command OpenFileCommand { get; }
        public Command DeleteFileCommand { get; }
        public Command<Message> DeleteMessageCommand { get; }






        public MessagesViewModel()
        {
            Messages = new ObservableCollection<Message>();
            LoadMessagesCommand = new Command(ExecuteLoadMessagesCommand);
            AddMessagesCommand = new Command(async () => await ExecuteAddMessagesCommand());
            OpenFileCommand = new Command(ExecuteOpenFileCommand);
            DeleteFileCommand = new Command(ExecuteDeleteFileCommand);
            DeleteMessageCommand = new Command<Message>(async(m)=> await ExecuteDeleteMessageCommand(m));
            ShowNotification();
        }

        string message;
        public string Message 
        {
            get => message;
            set
            {
                SetProperty(ref message,value);
               
            }
        }

        string messageLable;
        public string MessageLable
        {
            get => messageLable;
            set
            {
                SetProperty(ref messageLable, value);
            }
        }

        FileResult file;
        public FileResult File
        {
            get => file;
            set
            {
                SetProperty(ref file, value);
            }
        }

        string fileNameLable;
        public string FileNameLable
        {
            get => fileNameLable;
            set
            {
                SetProperty(ref fileNameLable, value);
            }
        }

        string fileNameLableColor;
        public string FileNameLableColor
        {
            get => fileNameLableColor;
            set
            {
                SetProperty(ref fileNameLableColor, value);
            }
        }

        bool isFileVisible;
        public bool IsFileVisible
        {
            get => isFileVisible;
            set
            {
                SetProperty(ref isFileVisible, value);
            }
        }

        int newMessagesCount;
        public int NewMessagesCount
        {
            get => newMessagesCount;
            set
            {
                SetProperty(ref newMessagesCount, value);
            }
        }

        void ExecuteLoadMessagesCommand()
        {
            IsBusy = true;

            try
            {
                FillMessagesList();
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

        async Task ExecuteAddMessagesCommand()
        {
            

            try
            {
                if(Message == null || Message == "")
                {
                    return;
                }

                IsBusy = true;
                await Task.Run(async () =>
                {
                    // Run code here
                    await MessageService.AddMessageAsync(Message, File);
                    Device.BeginInvokeOnMainThread(() =>
                    {
                        // UI interaction goes here
                        FillMessagesList();
                        Message = "";
                        ExecuteDeleteFileCommand();

                    });
                });


                

            }
            catch(Exception ex)
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
                OnMessageSelected(value);
            }
        }

        async void FillMessagesList()
        {
            //var IsConnected = await connectionService.IsConnected();

            //if (!IsConnected)
            //    return;

            var MessagesModelView = (await MessagesFileService.UpdateMessagesFile());
            var messages = MessagesModelView.Messages;
            NewMessagesCount = MessagesModelView.NewMessagesCount;
            
            Messages.Clear();
            foreach (var item in messages)
            {
                Messages.Add(item);
            }
            if (MessagesModelView.NewMessagesCountForAll > 0)
            {
                try
                {
                    var MessagesToJson = JsonConvert.SerializeObject(messages);
                    await PCLFileStorage.WriteTextAllAsync("MessagesFile", MessagesToJson);
                }
                catch
                {
                    if (await PCLFileStorage.IsFileExistAsync("MessagesFile"))
                    {
                        await PCLFileStorage.DeleteFile("MessagesFile");
                    }
                }
                finally
                {
                    Messages.Clear();
                    foreach (var item in messages)
                    {
                        Messages.Add(item);
                    }

                }
            }

            


        }

        async void ExecuteOpenFileCommand()
        {
            var file = await FilePicker.PickAsync();
            if (file != null)
            {
                if (CheckFileExtension(file.FileName) == false)
                {
                    FileNameLable = "هذا الامتداد غير مدعوم";
                    IsFileVisible = true;
                    FileNameLableColor = "Red";
                    return;
                }

                File = file;
                var FullName = file.FileName;
                if (file.FileName.Length > 20)
                {
                    var FileNameArr = file.FileName.Split('.');
                    if(FileNameArr.Length > 0)
                    {
                        var fileName = FileNameArr[0];
                        var fileExtension = FileNameArr[1];
                        fileName = fileName.Substring(0, 17);
                        FullName = $"{fileName}...{fileExtension}";
                    }
                }
                FileNameLable = FullName;
                IsFileVisible = true;
                FileNameLableColor = "#056839";
            }
        }

        bool CheckFileExtension(string FileName)
        {
            if (FileName == "" || FileName == null)
                return false;

            if (!FileName.Contains("."))
                return false;

            var FileExtension = FileName.Split('.')[1];
            string[] AcceptedExtensions = { "jpeg", "jpg", "pdf", "docx", "xlsx" };

            if (AcceptedExtensions.Contains(FileExtension))
                return true;

            return false;

        }

        void ExecuteDeleteFileCommand()
        {
            File = null;
            FileNameLable = "";
            IsFileVisible = false;
        }

        async void OnMessageSelected(Message message)
        {
            if (message == null)
                return;

            if (message.Attachement == null || message.Attachement == "")
            {
                SelectedMessage = null;
                return;
            }
                

            var link = $"https://www.everestexport.net/ems/attachments/{message.Attachement}";
            await Browser.OpenAsync(link);
            SelectedMessage = null;


        }

        async Task ExecuteDeleteMessageCommand(Message message)
        {
            if (message == null)
                return;

            var res = await MessageService.DeleteMessageAsync(message.ID);
            if(res)
            {
                var messages = (await MessagesFileService.GetMessgesFromFile()).ToList();
                var MessageFromFile = messages.FirstOrDefault(m => m.ID == message.ID);
                messages.Remove(MessageFromFile);

                Messages.Clear();
                foreach (var item in messages)
                {
                    Messages.Add(item);
                }

                try
                {
                    var MessagesToJson = JsonConvert.SerializeObject(messages);
                    await PCLFileStorage.WriteTextAllAsync("MessagesFile", MessagesToJson);
                }
                catch
                {
                    if (await PCLFileStorage.IsFileExistAsync("MessagesFile"))
                    {
                        await PCLFileStorage.DeleteFile("MessagesFile");
                    }
                }
                finally
                {
                    Messages.Clear();
                    foreach (var item in messages)
                    {
                        Messages.Add(item);
                    }
                }
                
            }

        }

        public void ShowNotification()
        {
            Device.StartTimer(new TimeSpan(0,0, 40),() =>
            {
                FillMessagesList();
                return true;
            });

            
            
        }
    }
}
