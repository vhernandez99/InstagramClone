using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class MainPageModel : FreshBasePageModel
    {
        public ObservableCollection<Story> Stories => new ObservableCollection<Story>(Story.GetAllStories());
        private ObservableCollection<Post> _postCollection;
        public ObservableCollection<Post> PostsCollection
        {
            get
            {
                return _postCollection;
            }
            set
            {
                _postCollection = value;
            }
        }
        public Command ViewPostCommentsCommand => new Command<Post>(ViewPostComments);
        public Command PostCommentCommand => new Command<int>(PostComment);
        public Command RefreshPostsCommand => new Command(RefreshPosts);
        public Command RemainingItemsThresholdReachedCommand => new Command(async () => await RemainingItemsThresholdReached());
        public Command GoToUsersListPageCommand
        {
            get
            {
                return new Command(async () => {
                    //Push A Page Model
                    await CoreMethods.PushPageModel<ConversationsPageModel>();
                });
            }
        }
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
        public string UserNameLogged { get; set; }
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
        private bool _isRefreshing;
        public bool IsRefreshing
        {
            set
            {
                _isRefreshing = value;
                RaisePropertyChanged();
            }
            get => _isRefreshing;
        }
        public bool _isBusy;
        public bool IsBusy
        {
            set
            {
                _isBusy = value;
                IsVisible = !value;
                RaisePropertyChanged();
            }
            get => _isBusy;


        }
        public bool _isVisible;
        public bool IsVisible
        {
            set
            {
                _isVisible = value;
                RaisePropertyChanged();
            }
            get => _isVisible;
            
        }
        public int PageNumber { get; set; } = 0;
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
        private async void ViewPostComments(Post post)
        {
            await CoreMethods.PushPageModel<PostCommentsPageModel>(post);
        }
        private async void RefreshPosts(object obj)
        {
            IsRefreshing = true;
            PageNumber=1;
            PostsCollection.Clear();
            List<Post> Posts = await ApiService.GetAllPosts(PageNumber, 5);
            foreach (Post post in Posts)
            {
                PostsCollection.Add(post);
            }
            IsRefreshing = false;
        }
        private async Task RemainingItemsThresholdReached()
        {
            
            if (IsBusy) { return; }
            PageNumber++;
            IsBusy = true;
            List<Post> Posts = await ApiService.GetAllPosts(PageNumber, 5);
            foreach (Post post in Posts)
            {
                PostsCollection.Add(post);
            }
            IsBusy = false;
        }
        public async Task GetAllPosts()
        {
            PageNumber = 1;
            PostsCollection.Clear();
            List<Post> Posts = await ApiService.GetAllPosts(PageNumber, 5);
            foreach (Post post in Posts)
            {
                PostsCollection.Add(post);
            }
        }
        public MainPageModel()
        {
            PostsCollection = new ObservableCollection<Post>();
        }
        public async Task GetUserLoggedInfo()
        {
            var userLoggedInfo = await ApiService.GetUserLoggedInfo();
            UserLoggedImageUrl = userLoggedInfo.FullImageUrl;
            UserNameLogged = userLoggedInfo.UserName;
        }
        
        public override async void Init(object initData)
        {
            await GetUserLoggedInfo();
            IsBusy = true;
            await GetAllPosts();
            IsBusy = false;
            base.Init(initData);
        }
    }
}
