using FreshMvvm;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class SignUpPageModel : FreshBasePageModel

    {
        public MediaFile file;
        private ImageSource _ImageUrl = "imagenseleccion.png";
        public ImageSource ImageUrl
        {
            set
            {
                _ImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _ImageUrl;
            }
        }
        public Command GoToInitialPageCommand => new Command(GoToInitialPage);
        public Command SelectImageCommand => new Command(SelectImage);
        public Command LoginCommand => new Command(Login);
        private void Login(object obj)
        {
            if (!RegisterIsValid())
            {
                CoreMethods.DisplayAlert("", "Favor de llenar todos los campos", "Ok");
            }
            if (!PasswordsAreEqual())
            {
                CoreMethods.DisplayAlert("", "Las contraseñas no coinciden", "Ok");
            }
            if (ImageUrl.ToString().Contains("imagenseleccion.png"))
            {
                CoreMethods.DisplayAlert("", "Favor de seleccionar una foto de perfil", "Ok");
            }
        }
        private bool RegisterIsValid()
        {
            if (!(string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(User) || string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(ConfirmPassword)))
            {
                return true;
            }
            return false;
        }
        private bool PasswordsAreEqual()
        {
            if (Password == ConfirmPassword)
            {
                return true;
            }
            return false;
        }

        private string _Name;
        public string Name
        {
            set
            {
                _Name = value;
                RaisePropertyChanged();
            }
            get
            {
                return _Name;
            }
        }
        private string _User;
        public string User
        {
            set
            {
                _User = value;
                RaisePropertyChanged();
            }
            get
            {
                return _User;
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
        private string _ConfirmPassword;
        public string ConfirmPassword
        {
            set
            {
                _ConfirmPassword = value;
                RaisePropertyChanged();
            }
            get
            {
                return _ConfirmPassword;
            }
        }


        private async void SelectImage(object obj)
        {
            if(!CrossMedia.Current.IsPickPhotoSupported)
            {
                await CoreMethods.DisplayAlert("Error", "Tu dispositivo no soporta esta herramienta", "Ok");
                return;
            }
            file = await CrossMedia.Current.PickPhotoAsync();
            if (file == null)
                return;
            ImageUrl = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
        }

        private void GoToInitialPage(object obj)
        {
            
            CoreMethods.PushPageModel<InitialPageModel>();
        }
    }
}
