using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    class OrderService : IOrderService<Order>
    {
        string BaseApiAddress = "";
        string CustomerId = Preferences.Get("ID", "");

        public async Task<bool> AddOrderAsync()
        {

            if(CustomerId == "")
                return await Task.FromResult(false);


            BaseApiAddress = $"https://www.everestexport.net/ems_neworder.php?id={CustomerId}";
            var file = await MediaPicker.PickPhotoAsync();

            if (file == null)
                return await Task.FromResult(false);

            var ImageExtnsion = file.FileName.Split('.')[1];
            if(ImageExtnsion != "jpg")
                return await Task.FromResult(false);


            var image = File.ReadAllBytes(file.FullPath);

            var content = new MultipartFormDataContent();
            content.Add(new StreamContent(new MemoryStream(image)), "file", "upload.jpg");

            var httpClient = new HttpClient();
            var response = await httpClient.PostAsync(BaseApiAddress, content);

            if(response.IsSuccessStatusCode)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(bool forceRefresh = false)
        {
            IEnumerable<Order> Data = new List<Order>();

            if (CustomerId == "")
                return Data;


            BaseApiAddress = $"https://www.everestexport.net/ems_getorders.php?id={CustomerId}";

            
            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<Order>>(Json);
            }

            return Data;
        }
    }
}
