using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    class CommentAdd
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public int PostId { get; set; }
    }
}
