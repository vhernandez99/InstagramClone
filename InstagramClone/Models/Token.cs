using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class Token
    {
        public string Access_token { get; set; }
        public int Expires_in { get; set; }
        public string Token_type { get; set; }
        public int Creation_Time { get; set; }
        public int Expiration_Time { get; set; }
        public int User_Id { get; set; }
        public string User_Name { get; set; }
    }
}
