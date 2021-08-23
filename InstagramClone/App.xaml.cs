using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;
using Syncfusion.Licensing;
using InstagramClone.PageModels;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Essentials;
using InstagramClone.Services;
using InstagramClone.Pages;

namespace InstagramClone
{
    public partial class App : Xamarin.Forms.Application
    {
        
        public class NavigationContainerNames
        {
            public static string AuthenticationContainer = "LoginPage";
            public static string MainContainer = "MainPage";
            public static string UsersListPage = "UsersListPage";

        }
        public App(bool shallNavigate)
        {
            SyncfusionLicenseProvider.RegisterLicense("NDkxNTk2QDMxMzYyZTM0MmUzMEtvV1hJeEJMdkN4MElKUWRyM09LaTJSa2oxR0NPK1V5OVgvdnZzZGRrTk09");
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

                //var messagePage = FreshPageModelResolver.ResolvePageModel<MessagePageModel>();

                //MessagePageModel messagePageModel = new MessagePageModel();
                //tabbedPageContainer.PushPage(messagePage, messagePageModel);
                MainPage = tabbedPageContainer;
            }
            if (shallNavigate && !string.IsNullOrEmpty(accessToken))
            {
                var tabbedPageContainer = new FreshTabbedNavigationContainer(NavigationContainerNames.UsersListPage);
                tabbedPageContainer.SelectedTabColor = Color.Black;
                tabbedPageContainer.BarBackgroundColor = Color.White;
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().DisableSwipePaging();
                tabbedPageContainer.AddTab<MainPageModel>("", "home.png");
                //tabbedPageContainer.AddTab<UsersListPageModel>("", "grupo.png");
                tabbedPageContainer.AddTab<AddMediaPageModel>("", "add.png");
                tabbedPageContainer.AddTab<ShopPageModel>("", "shop.png");
                tabbedPageContainer.AddTab<ProfilePageModel>("", "user.png");
                
                var usersListPage = FreshPageModelResolver.ResolvePageModel<ConversationsPageModel>();
                ConversationsPageModel usersListPageModel = new ConversationsPageModel();
                tabbedPageContainer.PushPage(usersListPage, usersListPageModel);

                //var messagePage = FreshPageModelResolver.ResolvePageModel<MessagePageModel>();
                //MessagePageMode.l messagePageModel = new MessagePageModel();
                //tabbedPageContainer.PushPage(messagePage, messagePageModel);
                MainPage = tabbedPageContainer;
            }
            else if(string.IsNullOrEmpty(accessToken))
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
