using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Customer
    {
        // Customer myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public string ID { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Info { get; set; }
    }
}
