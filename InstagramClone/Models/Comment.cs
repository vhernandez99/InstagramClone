using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
        public string FullComment => Description + " " + UserName;
        public string PostDescription { get; set; }
        public string UserImageUrl { get; set; }

    }
}
