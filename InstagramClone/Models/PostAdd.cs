using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class PostAdd
    {
        public string Description { get; set; }
        public int UserId { get; set; }
        public byte[] ImageArray { get; set; }
    }
}
