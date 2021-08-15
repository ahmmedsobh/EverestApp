using EverestApp.Helpers;
using EverestApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
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
            IEnumerable<Order> DataWithIndex = new List<Order>();

            if (CustomerId == "")
                return Data;


            BaseApiAddress = $"https://www.everestexport.net/ems_getorders.php?id={CustomerId}&x=" + (new Random()).Next(100000000);

          

            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<Order>>(Json);
            }

            var index = 0;
            DataWithIndex = from d in Data
                            select new Order
                            { 
                                Index = ++index,
                                ID = d.ID,
                                CustomerID = d.CustomerID,
                                UploadedDate = d.UploadedDate,
                                UpdatedDate = d.UpdatedDate,
                                Satus = d.Satus,
                                StatusTitle = StatusTitle(d.Satus),
                                StatusIcon = StatusIcon(d.Satus),
                            };


            return await Task.FromResult(DataWithIndex);
        }

        public async Task<IEnumerable<Category>> GetOrdersByCategoryAsync(bool forceRefresh = false)
        {
            IEnumerable<Order> Data = new List<Order>();
            IEnumerable<Order> DataWithIndex = new List<Order>();
            IEnumerable<Category> Categories = new List<Category>();


            if (CustomerId == "")
                return Categories;


            BaseApiAddress = $"https://www.everestexport.net/ems_getorders.php?id={CustomerId}&x=" + (new Random()).Next(100000000);

            

            var Uri = BaseApiAddress;
            var Client = new HttpClient();
            HttpResponseMessage response = await Client.GetAsync(Uri);
            if (response.IsSuccessStatusCode)
            {
                var Json = await response.Content.ReadAsStringAsync();
                Data = JsonConvert.DeserializeObject<IEnumerable<Order>>(Json);
            }

            var index = 0;
            DataWithIndex = from d in Data
                            select new Order
                            {
                                Index = ++index,
                                ID = d.ID,
                                CustomerID = d.CustomerID,
                                UploadedDate = d.UploadedDate,
                                UpdatedDate = d.UpdatedDate,
                                Satus = d.Satus,
                                StatusTitle = StatusTitle(d.Satus),
                                StatusIcon = StatusIcon(d.Satus),
                            };

            Categories = (from o in DataWithIndex
                          group o by new { o.Satus, o.StatusTitle ,o.StatusIcon} into Group
                          select new Category(Group.Key.Satus, Group.Key.StatusTitle, Group.Key.StatusIcon, "", Group.ToList())).ToList();


            return await Task.FromResult(Categories);
        }
        string StatusTitle(string Status = "")
        {
            if(Status == "")
            {
                return "";
            }
            string statusTitle = "";
            switch (Status)
            {
                case "1":
                    statusTitle = "طلبيات مرسلة";
                    break;
                case "2":
                    statusTitle = "تم الاطلاع عليها";
                    break;
                case "3":
                    statusTitle = "استلام جزئي بالمخازن";
                    break;
                case "4":
                    statusTitle = "استلام كامل بالمخازن";
                    break;
                case "5":
                    statusTitle = "تم الفحص والاعداد للشحن";
                    break;
                case "6":
                    statusTitle = "تم الشحن";
                    break;
                case "7":
                    statusTitle = "وصلت مخازن بلد العميل";
                    break;
                case "8":
                    statusTitle = "تم التسليم";
                    break;
                case "9":
                    statusTitle = "تم الطلب من البائع";
                    break;
            }

            return statusTitle;

        }

        string StatusIcon(string Status = "")
        {
            if (Status == "")
            {
                return "";
            }
            string statusIcon = "";
            switch (Status)
            {
                case "1":
                    statusIcon = FontAwesomeIcons.PaperPlane;
                    break;
                case "2":
                    statusIcon = FontAwesomeIcons.Eye;
                    break;
                case "3":
                    statusIcon = FontAwesomeIcons.TruckLoading;
                    break;
                case "4":
                    statusIcon = FontAwesomeIcons.Boxes;
                    break;
                case "5":
                    statusIcon = FontAwesomeIcons.CheckCircle;
                    break;
                case "6":
                    statusIcon = FontAwesomeIcons.ShippingFast;
                    break;
                case "7":
                    statusIcon = FontAwesomeIcons.Warehouse;
                    break;
                case "8":
                    statusIcon = FontAwesomeIcons.ClipboardCheck;
                    break;
                case "9":
                    statusIcon = FontAwesomeIcons.CartArrowDown;
                    break;
            }

            return statusIcon;

        }
    }
}
