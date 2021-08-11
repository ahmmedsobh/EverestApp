using EverestApp.Models;
using PCLStorage;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IMessagesFileService<T>
    {
        Task<MessageModelView> UpdateMessagesFile();
        Task<IEnumerable<T>> GetMessgesFromFile();
    }
}
