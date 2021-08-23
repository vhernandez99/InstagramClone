using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class MessageModel
    {
        public int Id { get; set; }
        public int LoggedUserId { get; set; }
        public string Messagee { get; set; }
        public int ConversationId { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
