using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int LoggedUserId { get; set; }
        public string Messagee { get; set; }
        public bool IsOwnMessage { get; set; }
        public bool IsSystemMessage { get; set; }
        public string User1ImageUrl { get; set; }
        public string FullUser1ImageUrl => AppSettings.ApiUrl+User1ImageUrl;
        public string FullUser2ImageUrl => AppSettings.ApiUrl + User2ImageUrl;
        public string User2ImageUrl { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public string ReceiverUserName { get; set; }
        public int ConversationId { get; set; }
    }
}
