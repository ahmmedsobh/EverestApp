using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Shipment
    {
        public string ID { get; set; }
        public int Index { get; set; }
        public string CustomerID { get; set; }
        public string UploadDate { get; set; }
        public string UpdateDate { get; set; }
    }
}
