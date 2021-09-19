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
using Xamarin.CommunityToolkit.ObjectModel;
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
        public ICommand GoToMessageCommand { get; }
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
        private async Task GoToMessage(ConversationsUserGet obj)
        {
            await CoreMethods.PushPageModel<MessagePageModel>(obj);
            SelectedConversation = null;
        }
        public async Task GetAllConversations()
        {
            ConversationsList.Clear();
            List<ConversationsUserGet> conversations = await ApiService.GetUserConversations();
            foreach(ConversationsUserGet conversation in conversations)
            {
                ConversationsList.Add(conversation);
            }
        }
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            await GetAllConversations();
        }
        public ConversationsPageModel()
        {
            GoToMessageCommand = new AsyncCommand<ConversationsUserGet>(GoToMessage, allowsMultipleExecutions: false);
            ConversationsList = new ObservableCollection<ConversationsUserGet>();
        }

    }

    

}
