using System;

namespace EverestApp.Models
{
    public class Item
    {
        public string Id { get; set; }
        public string Text { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string Icon { get; set; }
        public string IconColor { get; set; }
        public string CategoryIcon { get; set; }
        public string CategoryIconColor { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
    }
}