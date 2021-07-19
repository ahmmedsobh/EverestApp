using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
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

        public async Task<bool> AddOrderAsync(byte[] file)
        {
            if(CustomerId == "")
                return await Task.FromResult(false);

            BaseApiAddress = $"https://www.everestexport.net/ems_neworder.php?id=" + CustomerId;

            if (file.Length == 0)
                return await Task.FromResult(false);

            

            

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("submit", "Upload Image");
            HttpUploadFile(BaseApiAddress, file, nvc);

            //var content = new MultipartFormDataContent();
            //content.Add(new ByteArrayContent(image, 0, image.Length), "fileToUpload", "upload.jpg");

            //var httpClient = new HttpClient();
            //httpClient.DefaultRequestHeaders.Accept.Clear();
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("text/html"));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xhtml+xml"));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/xml"));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("image/webp"));
            //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("*/*"));

            //var response = await httpClient.PostAsync(BaseApiAddress, content);

            //if (response.IsSuccessStatusCode)
            //    return await Task.FromResult(true);

            return await Task.FromResult(true);
        }

        public static void HttpUploadFile(string url,byte[] filebytes, NameValueCollection nvc)
        {
            string boundary = "---------------------------" + DateTime.Now.Ticks.ToString("x");
            byte[] boundarybytes = System.Text.Encoding.ASCII.GetBytes("\r\n--" + boundary + "\r\n");

            HttpWebRequest wr = (HttpWebRequest)WebRequest.Create(url);
            wr.ContentType = "multipart/form-data; boundary=" + boundary;
            wr.Method = "POST";
            wr.KeepAlive = true;
            wr.Credentials = System.Net.CredentialCache.DefaultCredentials;

            Stream rs = wr.GetRequestStream();

            string formdataTemplate = "Content-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            foreach (string key in nvc.Keys)
            {
                rs.Write(boundarybytes, 0, boundarybytes.Length);
                string formitem = string.Format(formdataTemplate, key, nvc[key]);
                byte[] formitembytes = System.Text.Encoding.UTF8.GetBytes(formitem);
                rs.Write(formitembytes, 0, formitembytes.Length);
            }
            rs.Write(boundarybytes, 0, boundarybytes.Length);

            string header = "Content-Disposition: form-data; name=\"fileToUpload\"; filename=\"upload.jpg\"\r\nContent-Type: image/jpeg\r\n\r\n";
            byte[] headerbytes = Encoding.UTF8.GetBytes(header);
            rs.Write(headerbytes, 0, headerbytes.Length);

            rs.Write(filebytes, 0, filebytes.Length);

            byte[] trailer = Encoding.ASCII.GetBytes("\r\n--" + boundary + "--\r\n");
            rs.Write(trailer, 0, trailer.Length);
            rs.Close();

            WebResponse wresp = null;
            try
            {
                wresp = wr.GetResponse();
                Stream stream2 = wresp.GetResponseStream();
                StreamReader reader2 = new StreamReader(stream2);

            }
            catch (Exception ex)
            {

                if (wresp != null)
                {
                    wresp.Close();
                    wresp = null;
                }
            }
            finally
            {
                wr = null;
            }
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
