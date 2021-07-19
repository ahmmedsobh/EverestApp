using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    interface IOrderService<T>
    {
        Task<bool> AddOrderAsync(byte[] file);
        Task<IEnumerable<T>> GetOrdersAsync(bool forceRefresh = false);
    }
}
