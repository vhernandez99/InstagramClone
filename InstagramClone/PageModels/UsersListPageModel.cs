using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.Pages;
using InstagramClone.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static InstagramClone.App;

namespace InstagramClone.PageModels
{
    public class UsersListPageModel: FreshBasePageModel
    {
        public ObservableCollection<UsersGetList> UsersList { get; set; }
        
        public Command GoToMessageCommand => new Command<UsersGetList>(GoToMessage);
        UsersGetList _selectedUser = null;
        public UsersGetList SelectedUser
        {
            get { return _selectedUser; }
            set
            {
                _selectedUser = value;
                RaisePropertyChanged();
            }
        }
        private string _user1ImageUrl;
        public string User1ImageUrl
        {
            set
            {
                _user1ImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _user1ImageUrl;
            }
        }
        private string _user2ImageUrl;
        public string User2ImageUrl
        {
            set
            {
                _user2ImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _user2ImageUrl;
            }
        }
        private string _lastConversationMessage;
        public string LastConversationMesssage
        {
            set
            {
                _lastConversationMessage = value;
                RaisePropertyChanged();
            }
            get
            {
                return _lastConversationMessage;
            }
        }
        private async void GoToMessage(UsersGetList obj)
        {
           await CoreMethods.PushPageModel<MessagePageModel>(obj);
            SelectedUser = null;
        }
        public async Task GetAllConversations()
        {

            List<ConversationAdd> conversation = await ApiService.GetUserConversations();
        }
        public async Task GetAllUsers()
        {
            try
            {
                List<UsersGetList> users = await ApiService.GetAllUsers();
                foreach (UsersGetList user in users)
                {
                    UsersList.Add(user);
                }
            }
            catch (Exception r)
            {
                await CoreMethods.DisplayAlert("GetAllUsers", r.Message, "Ok");
                return;
            }
        }
        public UsersListPageModel()
        {
            UsersList = new ObservableCollection<UsersGetList>();
        }

        public async override void Init(object initData)
        {
            await GetAllUsers();
            base.Init(initData);
        }

    }

    

}
