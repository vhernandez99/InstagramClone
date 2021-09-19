using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Services;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    
    public class PostCommentsPageModel: FreshBasePageModel
    {
        public Command PostCommentCommand => new Command(PostComment);
        public ICommand RemainingItemsThresholdReachedCommand { get; }
        public ObservableCollection<Comment> PostCommentsCollection { get; set; }
        public int PostId { get; set; }
        public string UserImageUrl { get; set; }
        private string _CommentUserImageUrl;
        public string CommentUserImageUrl
        {
            set
            {
                _CommentUserImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _CommentUserImageUrl;
            }
        }
        public string PostDescription { get; set; }
        public bool TaskInProcess { get; set; } = false;
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
        public string PostUserName { get; set; }
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
        private int PageNumber { get; set; }
        private bool IsBusy { get; set; }
        public async Task GetPostComments()
        {
            PageNumber = 1;

            List<Comment> comments = await ApiService.GetPostComments(PostId, PageNumber,5);
            
            foreach (Comment comment in comments)
            {
                PostCommentsCollection.Add(comment);
            }
            //CommentUserImageUrl = PostCommentsCollection.
        }
        public PostCommentsPageModel()
        {
            PostCommentsCollection = new ObservableCollection<Comment>();
            RemainingItemsThresholdReachedCommand = new AsyncCommand(RemainingItemsThresholdReached, allowsMultipleExecutions: false);

        }

        private async Task RemainingItemsThresholdReached()
        {
            if (IsBusy) { return; }
            PageNumber++;
            List<Comment> comments = await ApiService.GetPostComments(PostId, PageNumber, 5);
            foreach (Comment comment in comments)
            {
                PostCommentsCollection.Add(comment);
            }
        }

        private async void GetUserLoggedInfo()
        {
            var userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;

        }
        private async void PostComment(object obj)
        {

            if (TaskInProcess||string.IsNullOrEmpty(CommentText)) { return; }
            TaskInProcess = true;
            var response = await ApiService.AddPostComment(CommentText, PostId);
            if (response)
            {
                PostCommentsCollection.Clear();
                //await CoreMethods.DisplayAlert("", "Comentario publicado correctamente", "Ok");
                await GetPostComments();
                TaskInProcess = false;
                CommentText = "";
            }
            else
            {
                await CoreMethods.DisplayAlert("", "Error al comentar", "Ok");
                TaskInProcess = false;
            }
        }


        public  async override void Init(object initData)
        {
            IsBusy = true;  
            GetUserLoggedInfo();
            //casting
            Post post = (Post)initData;
            UserImageUrl = post.FullUserImageUrl;
            PostId = post.Id;
            PostDescription = post.Description;
            PostUserName = post.UserName;
            await GetPostComments();
            IsBusy = false;
        }

    }
}
