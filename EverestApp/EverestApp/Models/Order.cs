using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Order
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string CustomerID { get; set; }
        public string UploadedDate { get; set; }
        public string UpdatedDate { get; set; }
        public string Satus { get; set; }
        public string StatusTitle { get; set; }
        public string StatusColor { get; set; }
        public string StatusIcon { get; set; }
    }
}
