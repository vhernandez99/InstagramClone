using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class SellingItemPostAdd
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public int UserId { get; set; }
    }
}
