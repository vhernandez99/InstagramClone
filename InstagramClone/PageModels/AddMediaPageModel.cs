using FreshMvvm;
using ImageToArray;
using InstagramClone.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class AddMediaPageModel: FreshBasePageModel
    {
        public MediaFile file;
        private ImageSource _ImageUrl = "imagenseleccion.png";
        private string _Description;
        public string Description
        {
            set
            {
                _Description = value;
                RaisePropertyChanged();
            }
            get
            {
                return _Description;
            }
        }
        private string _UserLoggedImageUrl;
        public string UserLoggedImageUrl
        {
            set
            {
                _UserLoggedImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _UserLoggedImageUrl;
            }
        }
        private string _UserNameLogged;
        public string UserNameLogged
        {
            set
            {
                _UserNameLogged = value;
                RaisePropertyChanged();
            }
            get
            {
                return _UserNameLogged;
            }
        }
        public bool TaskInProcess { get; set; } = false;
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
        public Command SelectImageCommand => new Command(async () => await SelectImage());
        public Command AddPostCommand => new Command(async () => await AddPost());

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
        private async void GetUserLoggedInfo()
        {
            var userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;
            UserNameLogged = userLoggedInfo.UserName;

        }
        public  override void Init(object initData)
        {
            GetUserLoggedInfo();
        }
         
        private async Task AddPost()
        {
            if (TaskInProcess) { return; }
            TaskInProcess = true;
            if (ImageUrl.ToString().Contains("imagenseleccion.png"))
            {
                await CoreMethods.DisplayAlert("", "Favor de agregar una imagen a la publicacion", "Ok");
                return;
            }
            if(String.IsNullOrEmpty(Description))
            {
                await CoreMethods.DisplayAlert("", "Favor de agregar una descripcion a tu publicacion", "Ok");
                return;
            }
            var imageArray = FromFile.ToArray(file.GetStream());
            
            var response = await ApiService.AddPost(Description, file, imageArray);
            if (response)
            {
                await CoreMethods.DisplayAlert("", "Publicacion creada con exito", "Ok");
                TaskInProcess = false;
            }
            else
            {
                await CoreMethods.DisplayAlert("", "Error al subir la publicacion", "Ok");
                TaskInProcess = false;
            }
        }
        
    }

}
