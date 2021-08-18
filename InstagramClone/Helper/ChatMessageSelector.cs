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
            var loggedUserId = Preferences.Get("userId", 0);
            var list = (CollectionView)container;
            MessageModel message = (MessageModel)item;
            if (message.LoggedUserId==loggedUserId)
            {
                return (DataTemplate)list.Resources["Case1View"];
            }
            else
            {
                return (DataTemplate)list.Resources["Case2View"];
            }
            
        }
    }
}
