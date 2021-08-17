using InstagramClone.Services;
using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class UserLogged
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string FullImageUrl => AppSettings.ApiUrl + ImageUrl;
    }
}
