using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class UsersGetList
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string TokenFirebase { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ImageUrl;
    }
}
