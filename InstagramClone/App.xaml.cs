using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;

using InstagramClone.PageModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;

namespace InstagramClone
{
    public partial class App : Xamarin.Forms.Application
    {
        public App()
        {
            InitializeComponent();
            var tabbedNavigation = new FreshTabbedNavigationContainer();
            tabbedNavigation.SelectedTabColor = Color.Black;
            tabbedNavigation.BarBackgroundColor = Color.White;
           
            tabbedNavigation.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            tabbedNavigation.AddTab<MainPageModel>("", "home.png");
            tabbedNavigation.AddTab<SearchPageModel>("", "search.png");
            tabbedNavigation.AddTab<AddMediaPageModel>("", "add.png");
            tabbedNavigation.AddTab<ShopPageModel>("", "shop.png");
            tabbedNavigation.AddTab<ProfilePageModel>("", null);
            //var page = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
            //var navigationPage = new FreshNavigationContainer(page);
            MainPage = tabbedNavigation;
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
