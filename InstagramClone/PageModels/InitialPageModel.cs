using FreshMvvm;
using System;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class InitialPageModel : FreshBasePageModel
    {
        public Command GoToSignUpPageCommand => new Command(GoToSignUpPage);
        public Command GoToLoginPageCommand => new Command(GoToLoginPage);

        private async void GoToLoginPage(object obj)
        {
            await CoreMethods.PushPageModel<LoginPageModel>();
        }

        private async void GoToSignUpPage(object obj)
        {
            await CoreMethods.PushPageModel<SignUpPageModel>();
        }
    }
}
