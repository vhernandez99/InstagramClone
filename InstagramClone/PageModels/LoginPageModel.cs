using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using static InstagramClone.App;

namespace InstagramClone.PageModels
{
    public class LoginPageModel: FreshBasePageModel
    {
        public Command GoToMainPageCommand => new Command(GoToMainPage);
        private  void GoToMainPage(object obj)
        {

            CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.MainContainer);
            //var navigationPage = new FreshNavigationContainer(page);
        }
    }
}
