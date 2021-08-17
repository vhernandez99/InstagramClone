using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class ConversationAdd
    {
        public int ConversationId { get; set; }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
    }
}
