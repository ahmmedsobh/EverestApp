using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Order
    {
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string UploadedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Status { get; set; }
    }
}
