using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Category:List<Order>
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public Category(string id, string name, string icon, string iconColor, List<Order> orders) : base(orders)
        {
            Id = id;
            Name = name;
            Icon = icon;
            IconColor = iconColor;
        }
    }
}
