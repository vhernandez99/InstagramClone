using FreshMvvm;
using ImageToArray;
using InstagramClone.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System.Threading.Tasks;
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
        public Command SelectImageCommand => new Command(async () => await SelectImage());
        public Command RegisterCommand => new Command(async () => await Register());
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
        private async Task Register()
        {
            if (!RegisterIsValid())
            {
                await CoreMethods.DisplayAlert("", "Favor de llenar todos los campos", "Ok");
                return;
            }
            if (!PasswordsAreEqual())
            {
                await CoreMethods.DisplayAlert("", "Las contraseñas no coinciden", "Ok");
                return;
            }
            if (ImageUrl.ToString().Contains("imagenseleccion.png"))
            {
                await CoreMethods.DisplayAlert("", "Favor de seleccionar una foto de perfil", "Ok");
                return;
            }
            var imageArray = FromFile.ToArray(file.GetStream());
            var response = await ApiService.RegisterUser(Name, User, Password, file, imageArray);
            if (response)
            {
                await CoreMethods.DisplayAlert("", "Cuenta creada correctamente", "Ok");
                await CoreMethods.PushPageModel<LoginPageModel>();
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
        private async Task SelectImage()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
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

            CoreMethods.PopPageModel();
        }
    }
}
