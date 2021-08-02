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
            var id = Preferences.Get("userId", 3);
            
            
            //var accessToken = Preferences.Get("accessToken", string.Empty);
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
