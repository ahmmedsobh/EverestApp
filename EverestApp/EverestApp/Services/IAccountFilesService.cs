using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IAccountFilesService<T>
    {
        Task<IEnumerable<T>> GetAcountFilesAsync();
        Task<bool> OpenPdf(string ID);
    }
}
