using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Message
    {
        public int Index { get; set; }
        public string ID { get; set; }
        public string MessageText { get; set; }
        public string CustomerID { get; set; }
        public string Sender { get; set; }
        public string Attachement { get; set; }
        public bool HaveAttachement { get; set; }
        public string SentDate { get; set; }
        public string MessageColor { get; set; }
    }
}
