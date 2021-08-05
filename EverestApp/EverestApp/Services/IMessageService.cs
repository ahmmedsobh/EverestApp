using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IMessageService<T>
    {
        Task<bool> AddMessageAsync(string msg , string cus,byte[] file);
        Task<bool> DeleteMessageAsync(string id);
        Task<IEnumerable<T>> GetMessagesAsync();
    }
}
