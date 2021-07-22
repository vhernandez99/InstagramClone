using FreshMvvm;
using InstagramClone.Models;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class SearchPageModel : FreshBasePageModel
    {
        public Command GoToSearchingPage { get; set; }
        public ObservableCollection<Post> Images { get; set; }


        public SearchPageModel()
        {
            GoToSearchingPage = new Command(() =>
            {
                CoreMethods.PushPageModel<SearchingPageModel>();
            });
            Images = new ObservableCollection<Post>(Post.GetAllPosts());
        }
    }
}


