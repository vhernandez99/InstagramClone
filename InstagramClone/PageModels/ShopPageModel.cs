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
        public ICommand GoToShopItemDetailsCommand { get; }
        public ICommand GoToAddShopItemPageCommand { get; }
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
            GoToAddShopItemPageCommand = new AsyncCommand(GoToAddShopItemPage, allowsMultipleExecutions: false);
            GoToShopItemDetailsCommand = new AsyncCommand<Post>(GoToShopItemDetails, allowsMultipleExecutions: false);
        }


        private async Task GoToShopItemDetails(object obj)
        {
            await CoreMethods.PushPageModel<ShopItemDetailsPageModel>(obj);
        }

        private async Task GoToAddShopItemPage()
        {
            await CoreMethods.PushPageModel<AddShopItemPageModel>(); 
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
