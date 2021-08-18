using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class ConversationsUserGet
    {
        public int Id { get; set; }
        public int User1Id { get; set; }
        public string User1Name { get; set; }
        public string User1ImageUrl { get; set; }
        public string FullUser1ImageUrl => AppSettings.ApiUrl + User1ImageUrl;
        public int User2Id { get; set; }
        public string User2Name { get; set; }
        public string User2ImageUrl { get; set; }
        public string FullUser2ImageUrl => AppSettings.ApiUrl + User2ImageUrl;
        public string LastMessage { get; set; }
    }
}
