using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    interface IShipmentService<T>
    {
        Task<IEnumerable<T>> GetShipmentsAsync(bool forceRefresh = false);
    }
}
