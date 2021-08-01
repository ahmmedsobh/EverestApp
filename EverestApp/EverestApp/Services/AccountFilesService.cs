using EverestApp.Models;
using Newtonsoft.Json;
using Plugin.XamarinFormsSaveOpenPDFPackage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    class AccountFilesService : IAccountFilesService<AccountFile>
    {

        string BaseApiAddress = "";
        string CustomerId = Preferences.Get("ID", "");

        public async Task<IEnumerable<AccountFile>> GetAcountFilesAsync()
        {
            IEnumerable<AccountFile> Data = new List<AccountFile>();
            IEnumerable<AccountFile> DataWithIndex = new List<AccountFile>();

            if (CustomerId == "")
                return Data;


            BaseApiAddress = $"https://www.everestexport.net/ems_accounts.php?id={CustomerId}&x=" + (new Random()).Next(100000000);


            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<AccountFile>>(Json);
            }

            var index = 0;
            DataWithIndex = from d in Data
                            select new AccountFile
                            { 
                                Index = index++,
                                ID = d.ID,
                                CustomerID = d.CustomerID,
                                UpdateDate = d.UpdateDate,
                                UploadDate = d.UploadDate
                            };

            return await Task.FromResult(DataWithIndex);
        }

        public async Task<bool> OpenPdf(string ID)
        {
            if (ID == "" || ID == null)
                return await Task.FromResult(false);

            HttpClient client = new HttpClient();

            var uri = new Uri($"https://www.everestexport.net/ems/accounts/{ID}.pdf");

            Stream content;
            MemoryStream stream = new MemoryStream();

            var response = await client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                content = await response.Content.ReadAsStreamAsync();
                content.CopyTo(stream);
            }
            else
            {
                return await Task.FromResult(false);
            }

            await CrossXamarinFormsSaveOpenPDFPackage.Current.SaveAndView("AccountFile"+ ID + ".pdf", "application/pdf", stream, PDFOpenContext.InApp);
            return await Task.FromResult(true);
        }
    }
}
