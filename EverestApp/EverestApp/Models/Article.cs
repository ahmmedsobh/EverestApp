using System;
using System.Collections.Generic;
using System.Text;

namespace EverestApp.Models
{
    class Article
    {
        public string id { get; set; }
        public string title { get; set; }
        public string alias { get; set; }
        public string link { get; set; }
        public string catid { get; set; }
        public string introtext { get; set; }
        public string fulltext { get; set; }
        public object extra_fields { get; set; }
        public string created { get; set; }
        public string created_by_alias { get; set; }
        public string modified { get; set; }
        public string featured { get; set; }
        public string image { get; set; }
        public string imageWidth { get; set; }
        public string image_caption { get; set; }
        public string image_credits { get; set; }
        public string imageXSmall { get; set; }
        public string imageSmall { get; set; }
        public string imageMedium { get; set; }
        public string imageLarge { get; set; }
        public string imageXLarge { get; set; }
        public object video { get; set; }
        public string video_caption { get; set; }
        public string video_credits { get; set; }
        public object gallery { get; set; }
        public string hits { get; set; }
        //public Category category { get; set; }
        public List<object> tags { get; set; }
        public List<object> attachments { get; set; }
        public int votingPercentage { get; set; }
        public string numOfvotes { get; set; }
        //public Author author { get; set; }
        public string numOfComments { get; set; }
        //public Events events { get; set; }
        public string language { get; set; }
    }
}
