using FreshMvvm;
using ImageToArray;
using InstagramClone.Models;
using InstagramClone.Services;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Stormlion.ImageCropper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class AddMediaPageModel: FreshBasePageModel
    {
        public MediaFile file;
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
        private bool _TaskInProcess = false;
        public bool TaskInProcess
        {
            set
            {
                _TaskInProcess = value;
                RaisePropertyChanged();
            }
            get
            {
                return _TaskInProcess;
            }
        }
        private ImageSource _ImageUrl = "";
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
        public string ImageExtension { get; set; }
        public Command OpenGalleryCommand => new Command( async() => await OpenGallery());
        public Command OpenCameraCommand => new Command(async () => await OpenCamera());
        private async Task OpenCamera()
        {
            if (!CrossMedia.Current.IsTakePhotoSupported)
            {
                await CoreMethods.DisplayAlert("Error", "Tu dispositivo no soporta esta herramienta", "Ok");
                return;
            }
            file = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions
            {
                PhotoSize = PhotoSize.Small,
                CompressionQuality = 35,
                AllowCropping = true
            }) ;
            if (file == null)
                return;
            ImageUrl = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            ImageExtension = Path.GetExtension(file.Path);
        }
        public Command AddPostCommand => new Command(async () => await AddPost());
        private async Task OpenGallery()
        {
            if (!CrossMedia.Current.IsPickPhotoSupported)
            {
                await CoreMethods.DisplayAlert("Error", "Tu dispositivo no soporta esta herramienta", "Ok");
                return;
            }
            file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
            {
                //MaxWidthHeight=300,
                PhotoSize=PhotoSize.Small,
                CompressionQuality = 35
            });
            if (file == null)
                return;
            ImageUrl = ImageSource.FromStream(() =>
            {
                var stream = file.GetStream();
                return stream;
            });
            ImageExtension = Path.GetExtension(file.Path);
            
        }
        private async Task GetUserLoggedInfo()
        {
            UserLogged userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;
            UserNameLogged = userLoggedInfo.UserName;
        }
        public  override async void Init(object initData)
        {
            await GetUserLoggedInfo();
        }
        private async Task AddPost()
        {
            if (TaskInProcess) { return; }
            TaskInProcess = true;
            try
            {
                if (string.IsNullOrEmpty(ImageUrl.ToString())||(string.IsNullOrEmpty(Description)))
                {
                    await CoreMethods.DisplayAlert("", "Favor de agregar una imagen y descripcion a la publicacion", "Ok");
                    TaskInProcess = false;
                    return;
                }
                var imageArray = FromFile.ToArray(file.GetStream());
                var response = await ApiService.AddPost(Description, file, imageArray, ImageExtension);
                if (response)
                {
                    await CoreMethods.DisplayAlert("", "Publicacion creada con exito", "Ok");
                    ImageUrl = "";
                    TaskInProcess = false;
                }
                else
                {
                    await CoreMethods.DisplayAlert("", "Error al subir la publicacion", "Ok");
                    TaskInProcess = false;
                    return;
                }
            }
            catch (Exception e)
            {
                await CoreMethods.DisplayAlert("", e.Message, "Ok");
                TaskInProcess = false;
            }
            finally
            {
                TaskInProcess = false;
            }
        }
    }

}
