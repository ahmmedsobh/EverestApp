using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    interface IMessageService<T>
    {
        Task<bool> AddMessageAsync(string msg ,FileResult file);
        Task<bool> DeleteMessageAsync(string id);
        Task<IEnumerable<T>> GetMessagesAsync();
        Task<IEnumerable<T>> GetMessagesAfterLastMessageAsync(int id);

    }
}
