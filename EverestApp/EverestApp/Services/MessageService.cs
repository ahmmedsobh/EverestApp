using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace EverestApp.Services
{
    class MessageService : IMessageService<Message>
    {
        string BaseApiAddress = "";
        string CustomerId = Preferences.Get("ID", "");
        public async Task<bool> AddMessageAsync(string msg, string cus,byte[] file)
        {

            if (CustomerId == "")
                return await Task.FromResult(false);

            BaseApiAddress = $"https://www.everestexport.net/ems_newmsg.php?msg={msg}&cus=" + CustomerId;

            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("submit", "Upload Image");
            HttpUploadFile(BaseApiAddress, file, nvc);
            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteMessageAsync(string id)
        {
            BaseApiAddress = $"https://www.everestexport.net/ems_deletemsg.php?id={id}";
            var Client = new HttpClient();
            var res = await Client.DeleteAsync(BaseApiAddress);
            if (res.IsSuccessStatusCode)
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }

        public async Task<IEnumerable<Message>> GetMessagesAsync()
        {
            IEnumerable<Message> Data = new List<Message>();
            IEnumerable<Message> DataWithIndex = new List<Message>();

            if (CustomerId == "")
                return Data;

            BaseApiAddress = $"https://www.everestexport.net/ems_messages.php?id={CustomerId}&x=" + (new Random()).Next(100000000);

            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<Message>>(Json);
            }

            var index = 0;
            DataWithIndex = from m in Data
                            select new Message
                            {
                                Index = ++index,
                                ID = m.ID,
                                CustomerID = m.CustomerID,
                                SentDate = m.SentDate,
                                MessageText = m.MessageText,
                                Sender = m.Sender,
                                Attachement = m.Attachement
                            };

            return await Task.FromResult(DataWithIndex);
        }


        public static void HttpUploadFile(string url, byte[] filebytes, NameValueCollection nvc)
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
    }
}
