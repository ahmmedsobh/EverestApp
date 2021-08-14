using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    public interface IConectionService
    {
        Task<bool> IsConnected();
    }
}
