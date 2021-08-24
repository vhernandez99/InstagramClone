using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class ShopItemPost
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ImageUrl;
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
