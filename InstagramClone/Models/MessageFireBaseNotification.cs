using System;
using System.Collections.Generic;
using System.Text;

namespace InstagramClone.Models
{
    public class MessageFireBaseNotification
    {
        public string[] registration_ids { get; set; }
        public NotificationFireBaseNotification notification { get; set; }
        public object data { get; set; }
    }
}
