using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;

namespace InstagramClone.PageModels
{
    public class ShopPageModel : FreshBasePageModel
    {
        private string _searchSellingItemsText;
        public string SearchSellingItemsText
        {
            get
            {
                return _searchSellingItemsText;
            }
            set
            {
                _searchSellingItemsText = value;
            }
        }
        private ObservableCollection<Post> _sellingItemsPosts;
        public ObservableCollection<Post> SellingItemsPosts
        {
            get
            {
                return _sellingItemsPosts;
            }
            set
            {
                _sellingItemsPosts = value;
            }
        }
        public ICommand RefreshSellingItemsCommand { get; }
        public ICommand DeleteSellinItemPostCommand { get; }
        public ICommand GoToDetailsSellingItemCommand { get; }
        private async Task GetAllSellingItemsPosts()
        {
            var posts=await ApiService.GetAllPosts(1,5);
            foreach(Post post in posts)
            {
                SellingItemsPosts.Add(post);
            }
        }
        public ShopPageModel()
        {
            SellingItemsPosts = new ObservableCollection<Post>();
            RefreshSellingItemsCommand = new AsyncCommand(GetAllSellingItemsPosts, allowsMultipleExecutions: false);
            DeleteSellinItemPostCommand = new AsyncCommand(DeleteSellinItemPost, allowsMultipleExecutions: true);
            GoToDetailsSellingItemCommand = new AsyncCommand<object>(GoToDetailsSelling, allowsMultipleExecutions: false);
        }

        private Task GoToDetailsSelling(object arg)
        {
            throw new NotImplementedException();
        }

        private async Task DeleteSellinItemPost()
        {
            await CoreMethods.DisplayAlert("", "Esta seguro que desea eliminar su publicacion?", "Ok");
        }

        public async override void Init(object initData)
        {
            await GetAllSellingItemsPosts();
        }
    }
}
