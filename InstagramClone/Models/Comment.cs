using InstagramClone.Services;
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
        public string UserImageUrl { get; set; }
        public string FullUserImageUrl => AppSettings.ApiUrl + UserImageUrl;
    }
}
