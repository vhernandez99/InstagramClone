using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Messagee { get; set; }
        public bool IsOwnMessage { get; set; }
        public bool IsSystemMessage { get; set; }
        public int SenderUserId { get; set; }
        public int SenderId { get; set; }
        public string ReceiverUserName { get; set; }
        public int ConversationId { get; set; }
    }
}
