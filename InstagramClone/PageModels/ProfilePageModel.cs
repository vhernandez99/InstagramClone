using FreshMvvm;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using static InstagramClone.App;

namespace InstagramClone.PageModels
{
    public class ProfilePageModel : FreshBasePageModel
    {
        public Command GoToInitialPageCommand => new Command(GoToInitialPage);
        private  void GoToInitialPage(object obj)
        {
            //CoreMethods.PopPageModel(this);
            CoreMethods.SwitchOutRootNavigation(NavigationContainerNames.AuthenticationContainer);
        }   
    }
}
