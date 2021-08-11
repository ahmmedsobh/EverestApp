using EverestApp.Helpers;
using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EverestApp.Services
{
    class MessagesFileService : IMessagesFileService<Message>
    {
        string FolderName = "MessagesFolder";
        string FileName = "MessagesFile";
        public IMessageService<Message> MessageService => DependencyService.Get<IMessageService<Message>>();

        public async Task<IEnumerable<Message>> GetMessgesFromFile()
        {
            IEnumerable<Message> Messages = new List<Message>();

            try
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName))
                {
                    string json = await PCLFileStorage.ReadAllTextAsync(FileName);
                    Messages = JsonConvert.DeserializeObject<IEnumerable<Message>>(json);
                    return Messages;
                }
            }
            catch(Exception ex)
            {

            }
            

            return Messages;
        }

        public async Task<MessageModelView> UpdateMessagesFile()
        {
            var MessagesModelView = new MessageModelView();
            if (await PCLFileStorage.IsFolderExistAsync(FolderName))
            {
                if (await PCLFileStorage.IsFileExistAsync(FileName))
                {
                    List<Message> Messages = (await GetMessgesFromFile()).ToList();

                    var LastMessage = Messages.OrderByDescending(m => int.Parse(m.ID)).FirstOrDefault();
                    var LastMessageId = 0;
                    if (LastMessage != null)
                    {
                        LastMessageId = int.Parse(LastMessage.ID);
                    }

                    IEnumerable<Message> NewMessages = await MessageService.GetMessagesAfterLastMessageAsync(LastMessageId);
                    var NewMessagesCount = 0;
                    if (NewMessages.Count() > 0)
                    {
                        foreach (var item in NewMessages)
                        {
                            Messages.Add(item);
                            NewMessagesCount = (item.Sender == null || item.Sender == "") ? NewMessagesCount : NewMessagesCount+1;
                        }
                    }

                    MessagesModelView.Messages = new List<Message>();
                    MessagesModelView.Messages = Messages;
                    MessagesModelView.NewMessagesCount = NewMessagesCount;
                    return MessagesModelView;
                }
                else
                {
                    IEnumerable<Message> NewMessages = await MessageService.GetMessagesAfterLastMessageAsync(0);
                    MessagesModelView.Messages = new List<Message>();
                    MessagesModelView.Messages = NewMessages;
                    MessagesModelView.NewMessagesCount = NewMessages.Count();
                    return MessagesModelView;
                }
            }
            else
            {
                await PCLFileStorage.CreateFolder(FolderName);
                if (await PCLFileStorage.IsFileExistAsync(FileName) == false)
                {
                    IEnumerable<Message> NewMessages = await MessageService.GetMessagesAfterLastMessageAsync(0);
                    MessagesModelView.Messages = new List<Message>();
                    MessagesModelView.Messages = NewMessages;
                    MessagesModelView.NewMessagesCount = NewMessages.Count();
                    return MessagesModelView;
                }
            }


            //int counter = 0;
            //for (int i = 0; i < 30; i++)
            //{
            //    if (await PCLFileStorage.IsFileExistAsync(FileName))
            //    {
            //        await PCLFileStorage.DeleteFile(FileName);
            //        counter++;
            //    }
            //}


            return MessagesModelView;
        }


    }
}
