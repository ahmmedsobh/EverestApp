using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    class ShipmentService : IShipmentService<Shipment>
    {
        string BaseApiAddress = "";
        string CustomerId = Preferences.Get("ID", "");
        public async Task<IEnumerable<Shipment>> GetShipmentsAsync(bool forceRefresh = false)
        {
            IEnumerable<Shipment> Data = new List<Shipment>();
            IEnumerable<Shipment> DataWithIndex = new List<Shipment>();

            if (CustomerId == "")
                return Data;


            BaseApiAddress = $"https://www.everestexport.net/ems_shipments.php?id={CustomerId}&x=" + (new Random()).Next(100000000);

            

            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<Shipment>>(Json);
            }

            var index = 0;
            DataWithIndex = from d in Data
                            select new Shipment
                            {
                                Index = ++index,
                                ID = d.ID,
                                CustomerID = d.CustomerID,
                                UploadDate = d.UploadDate,
                                UpdateDate = d.UpdateDate,
                            };


            return await Task.FromResult(DataWithIndex);
        }
    }
}
