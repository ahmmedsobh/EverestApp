using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IOrderService<T>
    {
        Task<bool> AddOrderAsync();
        Task<IEnumerable<T>> GetOrdersAsync(bool forceRefresh = false);
    }
}
