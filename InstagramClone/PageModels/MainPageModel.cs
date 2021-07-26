using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class MainPageModel : FreshBasePageModel
    {
        public ObservableCollection<Story> Stories => new ObservableCollection<Story>(Story.GetAllStories());
        public ObservableCollection<Post> PostsCollection { get; set; }
        public Command ViewPostCommentsCommand => new Command<Post>(ViewPostComments);
        public Command PostCommentCommand => new Command<int>(PostComment);
        private async void PostComment(int postId)
        {
            if (TaskInProcess) { return; }
            TaskInProcess = true;
            var response = await ApiService.AddPostComment(CommentText, postId);
            if (response)
            {
                await CoreMethods.DisplayAlert("", "Comentario publicado correctamente", "Ok");
                TaskInProcess = false;
                CommentText = "";
            }
            else
            {
                await CoreMethods.DisplayAlert("", "Error al comentar", "Ok");
                TaskInProcess = false;
            }
        }

        public bool TaskInProcess { get; set; } = false;
        public string UserLoggedImageUrl { get; set; }
        public string UserLoggedUserName { get; set; }
        private string _CommentText;
        public string CommentText
        {
            set
            {
                _CommentText = value;
                RaisePropertyChanged();
            }
            get
            {
                return _CommentText;
            }
        }


        private async void ViewPostComments(Post post)
        {
            await CoreMethods.PushPageModel<PostCommentsPageModel>(post);
        }

        private int PageNumber = 0;

        public MainPageModel()
        {
            PostsCollection = new ObservableCollection<Post>();
            GetAllPosts();
            GetUserLoggedInfo();
        }

        private async void GetUserLoggedInfo()
        {
            var userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;
            UserLoggedUserName = userLoggedInfo.UserName;
        }

        public async void GetAllPosts()
        {
            PageNumber++;
            List<Post> Posts = await ApiService.GetAllPosts(PageNumber, 10);

            foreach (Post post in Posts)
            {
                PostsCollection.Add(post);
            }
        }
    }
}
