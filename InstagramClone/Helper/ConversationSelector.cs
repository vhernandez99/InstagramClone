using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.Helper
{
    public class ConversationSelector:DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var loggedUserId = Preferences.Get("userId", 0);
            var list = (CollectionView)container;
            ConversationsUserGet conversation = (ConversationsUserGet)item;
            if (conversation.User1Id == loggedUserId)
            {
                return (DataTemplate)list.Resources["Case1View"];
            }
            if (conversation.User2Id == loggedUserId)
            {
                return (DataTemplate)list.Resources["Case2View"];
            }
            return null;
        }
    }
}
