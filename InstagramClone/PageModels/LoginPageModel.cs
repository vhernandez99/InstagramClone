using FreshMvvm;
using InstagramClone.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using static InstagramClone.App;

namespace InstagramClone.PageModels
{
    public class LoginPageModel: FreshBasePageModel
    {
        public Command LoginCommand => new Command(Login);
        public Command GoToSignUpPageCommand => new Command(GoToSignUpPage);
        private string _UserName;
        public string UserName
        {
            set
            {
                _UserName = value;
                RaisePropertyChanged();
            }
            get
            {
                return _UserName;
            }
        }
        private string _Password;
        public string Password
        {
            set
            {
                _Password = value;
                RaisePropertyChanged();
            }
            get
            {
                return _Password;
            }
        }
        public bool TaskInProcess { get; set; } = false;
        private void GoToSignUpPage(object obj)
        {
            CoreMethods.PushPageModel<SignUpPageModel>();
        }

        private async void Login(object obj)
        {
            if (TaskInProcess)
                return;
            TaskInProcess = true;
            var response = await ApiService.Login(UserName, Password);
            if (response)
            {
                Preferences.Set("UserName", _UserName);
                Preferences.Set("password", Password);
                var tabbedPageContainer = new FreshTabbedNavigationContainer(NavigationContainerNames.MainContainer);
                tabbedPageContainer.SelectedTabColor = Color.Black;
                tabbedPageContainer.BarBackgroundColor = Color.White;
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
                tabbedPageContainer.On<Xamarin.Forms.PlatformConfiguration.Android>().DisableSwipePaging();
                tabbedPageContainer.AddTab<MainPageModel>("", "home.png");
                //tabbedPageContainer.AddTab<UsersListPageModel>("", "grupo.png");
                tabbedPageContainer.AddTab<AddMediaPageModel>("", "add.png");
                tabbedPageContainer.AddTab<SellingItemsPageModel>("", "shop.png");
                tabbedPageContainer.AddTab<ProfilePageModel>("", "user.png");
                App.Current.MainPage = tabbedPageContainer;
                //CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
                TaskInProcess = false;
            }
            else
            {
                await CoreMethods.DisplayAlert("", "Favor de verificar la informacion", "Ok");
                TaskInProcess = false;
            }
            //var navigationPage = new FreshNavigationContainer(page);
        }
    }
}
