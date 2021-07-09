using EverestApp.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EverestApp.Services
{
    class MainService : IMainService
    {
        string BaseApiAddress = "https://www.everestexport.net/ar/news-and-events?format=json";

        public async Task<List<Article>> GetArticlesAysnc(bool forceRefresh = false)
        {
            var Articls = new List<Article>();
            var Uri = $"{BaseApiAddress}";
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var Json = await response.Content.ReadAsStringAsync();
                    JObject JsonObject = JObject.Parse(Json);
                    var data = JsonObject.SelectToken("items").ToString();
                    Articls = JsonConvert.DeserializeObject<List<Article>>(data);
                }
                catch(Exception ex)
                {

                }
                
            }

            return Articls;
        }
    }
}
