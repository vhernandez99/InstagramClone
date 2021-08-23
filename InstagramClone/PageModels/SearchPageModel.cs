using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Pages;
using InstagramClone.Services;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;

namespace InstagramClone.PageModels
{
    public class SearchPageModel : FreshBasePageModel
    {
        private string _searchBarText;
        public string SearchBarText
        {
            set
            {
                _searchBarText = value;
                RaisePropertyChanged();
            }
            get
            {
                return _searchBarText;
            }
        }
        public int SelectedUserId { get; set; }
        public bool IsRefreshing { get; set; }
        public ICommand RefreshUsersCommand { get; }
        private int ConversationId { get; set; }
        public Command GoToSearchingPage { get; set; }
        public Command SearchUsersCommand => new Command(async()=>await SearchUsers());
        public ICommand GoToMessagesCommand { get; }
        UsersGetList _userSelected = null;
        public UsersGetList UserSelected
        {
            get { return _userSelected; }
            set
            {
                _userSelected = value;
                RaisePropertyChanged();
            }
        }
        private async Task GoToMessages(UsersGetList obj)
        {
            SelectedUserId = obj.Id;
            await VerifyIfConversationExists();
            if (ConversationId == 0)
            {
                await CreateConversation();
                await VerifyIfConversationExists();
            }
            if (ConversationId != 0)
            {
                ConversationsUserGet conversation = await ApiService.GetConversationById(ConversationId);
                await CoreMethods.PushPageModel<MessagePageModel>(conversation);
            }
        }
        private async Task CreateConversation()
        {
            await ApiService.CreateConversation(SelectedUserId);
        }
        private async Task VerifyIfConversationExists()
        {
            ConversationId = await ApiService.VerifyIfConversationExists(SelectedUserId);
        }
        private async Task SearchUsers()
        {
            var searchUsers = await ApiService.SearchUsers(SearchBarText);
            if (searchUsers.Count == 0)
            {
                await CoreMethods.DisplayAlert("", "No se encontraron usuarios", "Ok");
            }
            if(searchUsers.Count!=0)
            {
                Users.Clear();
            }
            foreach (UsersGetList user in searchUsers)
            {
                Users.Add(user);
            }

        }

        private ObservableCollection<UsersGetList> _users;
        public ObservableCollection<UsersGetList> Users
        {
            get
            {
                return _users;
            }
            set
            {
                _users = value;
                RaisePropertyChanged();
            }
        }

        public SearchPageModel()
        {
            Users = new ObservableCollection<UsersGetList>();
            GoToMessagesCommand = new AsyncCommand<UsersGetList>(GoToMessages,allowsMultipleExecutions:false);
            RefreshUsersCommand = new AsyncCommand(GetAllUsers,allowsMultipleExecutions:false);
        }

        async Task GetAllUsers()
        {
            Users.Clear();
            var users = await ApiService.GetAllUsers();
            foreach(UsersGetList user in users)
            {
                Users.Add(user);
            }
            SearchBarText = " ";
        }
        public async override void Init(object initData)
        {
            await GetAllUsers();
            base.Init(initData);
        }
    }
}


