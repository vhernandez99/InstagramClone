using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;

using InstagramClone.PageModels;

namespace InstagramClone
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            var page = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
            var navigationPage = new FreshNavigationContainer(page);
            MainPage = navigationPage;
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {

        }
    }
}
