using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface ICustomerService<T>
    {
        Task<T> GetCustomerAsync(string Code,string Password);
    }
}
