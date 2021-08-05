using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;

using InstagramClone.PageModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Essentials;
using InstagramClone.Services;

namespace InstagramClone
{
    public partial class App : Xamarin.Forms.Application
    {
        public class NavigationContainerNames
        {
            public static string AuthenticationContainer = "InitialPage";
            public static string MainContainer = "MainPage";
        }
        public App()
        {
            InitializeComponent();

            //var tabbedPageContainer = new FreshTabbedNavigationContainer(NavigationContainerNames.MainContainer);
            //tabbedPageContainer.SelectedTabColor = Color.Black;
            //tabbedPageContainer.BarBackgroundColor = Color.White;
            //tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            //tabbedPageContainer.AddTab<MainPageModel>("", "home.png");
            //tabbedPageContainer.AddTab<SearchPageModel>("", "search.png");
            //tabbedPageContainer.AddTab<AddMediaPageModel>("", "add.png");
            //tabbedPageContainer.AddTab<ShopPageModel>("", "shop.png");
            //tabbedPageContainer.AddTab<ProfilePageModel>("", "user.png");
            
            var initialPage = FreshPageModelResolver.ResolvePageModel<InitialPageModel>();
     

            var mainPageContainer = new FreshNavigationContainer(initialPage, NavigationContainerNames.AuthenticationContainer);
            
            //if(string.IsNullOrEmpty(accessToken))
            MainPage = mainPageContainer;
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
