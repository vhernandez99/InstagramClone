using System;
using System.Collections.Generic;
using System.IO;

namespace InstagramClone.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string PostImageUrl { get; set; }
        public string UserName { get; set; }
        public string UserImageUrl { get; set; }
        public string FullUserImageUrl => AppSettings.ApiUrl + UserImageUrl;
        public string FullPostImageUrl => AppSettings.ApiUrl + PostImageUrl;
        public DateTime DateCreated { get; set; }
        public int UserId { get; set; }

    }


}
