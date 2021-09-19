using InstagramClone.Models;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.Helper
{
    public class PostSelector:DataTemplateSelector
    {
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            var loggedUserId = Preferences.Get("userId", 0);
            var list = (CollectionView)container;
            Post post = (Post)item;
            if (post.UserId == loggedUserId)
            {
                return (DataTemplate)list.Resources["OwnPost"];
            }
            else
            {
                return (DataTemplate)list.Resources["ExternalPost"];
            }
        }
    }
}
