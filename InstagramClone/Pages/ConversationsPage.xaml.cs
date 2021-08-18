using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace InstagramClone.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ConversationsPage : ContentPage
    {
        public ConversationsPage()
        {
            InitializeComponent();
            UsersCv.ItemsLayout = new LinearItemsLayout(ItemsLayoutOrientation.Vertical)
            {
                ItemSpacing = 35
            };
        }
    }
}