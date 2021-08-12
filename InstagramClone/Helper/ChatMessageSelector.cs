using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.Helper
{
    public class ChatMessageSelector : DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var loggerUserId = Preferences.Get("userId", 0);
            var list = (CollectionView)container;
            MessageModel message = (MessageModel)item;
            if (message.IsOwnMessage||message.SenderId== loggerUserId)
            {
                return (DataTemplate)list.Resources["OwnText"];
            }
            else if (message.IsOwnMessage == false)
            {
                return (DataTemplate)list.Resources["ExternalText"];
            }
            return null;
        }
    }
}
