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
    public class ConversationsPageModel: FreshBasePageModel
    {
        private ObservableCollection<ConversationsUserGet> _conversationsList;
        public ObservableCollection<ConversationsUserGet> ConversationsList
        {
            get
            {
                return _conversationsList;
            }
            set 
            {
                _conversationsList = value;
                RaisePropertyChanged();
            }
        }
        public Command GoToMessageCommand => new Command<ConversationsUserGet>(GoToMessage);
        ConversationsUserGet _conversationUserGet = null;
        public ConversationsUserGet SelectedConversation
        {
            get { return _conversationUserGet; }
            set
            {
                _conversationUserGet = value;
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
        private async void GoToMessage(ConversationsUserGet obj)
        {
            await CoreMethods.PushPageModel<MessagePageModel>(obj);
            SelectedConversation = null;
        }
        public async Task GetAllConversations()
        {
            List<ConversationsUserGet> conversations = await ApiService.GetUserConversations();
            foreach(ConversationsUserGet conversation in conversations)
            {
                ConversationsList.Add(conversation);
            }
        }
        //public async Task GetAllUsers()
        //{
        //    try
        //    {
        //        List<UsersGetList> users = await ApiService.GetAllUsers();
        //        foreach (UsersGetList user in users)
        //        {
        //            ConversationsList.Add(user);
        //        }
        //    }
        //    catch (Exception r)
        //    {
        //        await CoreMethods.DisplayAlert("GetAllUsers", r.Message, "Ok");
        //        return;
        //    }
        //}
        public ConversationsPageModel()
        {
            ConversationsList = new ObservableCollection<ConversationsUserGet>();
        }

        public async override void Init(object initData)
        {
            await GetAllConversations();
            base.Init(initData);
        }

    }

    

}
