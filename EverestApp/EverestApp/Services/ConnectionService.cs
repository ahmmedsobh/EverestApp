using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    class ConnectionService : IConectionService
    {
        public Task<bool> IsConnected()
        {
            var current = Connectivity.NetworkAccess;

            if (current == NetworkAccess.Internet)
                return Task.FromResult(true);

            return Task.FromResult(false);

        }
    }
}
