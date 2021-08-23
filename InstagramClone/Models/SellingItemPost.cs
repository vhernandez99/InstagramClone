using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class SellingItemPost
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ImageUrl;
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}
