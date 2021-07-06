using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    class CustomerService : ICustomerService<Customer>
    {
        string BaseApiAddress = "https://www.everestexport.net/emslogin.php";

        public async Task<Customer> GetCustomerAsync(string Code, string Password)
        {
            var Data = new Customer();
            var Uri = $"{BaseApiAddress}?code={Code}&pass={Password}";
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<Customer>(Json);
            }

            return Data;
        }
    }
}
