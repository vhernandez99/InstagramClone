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
            public static string AuthenticationContainer = "LoginPage";
            public static string MainContainer = "MainPage";
        }
        public App()
        {
            InitializeComponent();
            var accessToken = Preferences.Get("accessToken", string.Empty);
            
            var loginPage = FreshPageModelResolver.ResolvePageModel<LoginPageModel>();
            var mainPageContainer = new FreshNavigationContainer(loginPage, NavigationContainerNames.AuthenticationContainer);
            //if(string.IsNullOrEmpty(accessToken))
            if (!string.IsNullOrEmpty(accessToken))
            {
                var tabbedPageContainer = new FreshTabbedNavigationContainer(NavigationContainerNames.MainContainer);
                tabbedPageContainer.SelectedTabColor = Color.Black;
                tabbedPageContainer.BarBackgroundColor = Color.White;
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().DisableSwipePaging();
                tabbedPageContainer.AddTab<MainPageModel>("", "home.png");
                //tabbedPageContainer.AddTab<UsersListPageModel>("", "grupo.png");
                tabbedPageContainer.AddTab<AddMediaPageModel>("", "add.png");
                tabbedPageContainer.AddTab<ShopPageModel>("", "shop.png");
                tabbedPageContainer.AddTab<ProfilePageModel>("", "user.png");
                MainPage = tabbedPageContainer;
            }
            else
            {
                MainPage = mainPageContainer;
            }
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
