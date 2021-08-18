using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.PageModels;
using InstagramClone.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.Pages
{
    public class MessagePageModel: FreshBasePageModel
    {
        
        private string _loggedUserFullImageUrl;
        public string LoggedUserFullImageUrl
        {
            set
            {
                _loggedUserFullImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _loggedUserFullImageUrl;
            }
        }
        public string LoggedName { get; set; }

        private string _externalUserFullImageUrl;
        public string ExternalUserFullImageUrl
        {
            set
            {
                _externalUserFullImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _externalUserFullImageUrl;
            }
        }        
        private string _externalUserName;
        public string ExternalUserName
        {
            set
            {
                _externalUserName = value;
                RaisePropertyChanged();
            }
            get
            {
                return _externalUserName;
            }
        }
        private string _externalName;
        public List<string> ExternalUserTokenFirebase = new List<string>();
        public string ExternalName
        {
            set
            {
                _externalName = value;
                RaisePropertyChanged();
            }
            get
            {
                return _externalName;
            }
        }
        private int _conversationId;
        private string _message;
        private ObservableCollection<MessageModel> _messages;
        private bool _isConnected;
        //Actually unique UserName
        private string _loggedUserName = Preferences.Get("UserName", string.Empty);
        public string LoggedUserName
        {
            get
            {
                return _loggedUserName;
            }
            set
            {
                _loggedUserName = value;
                RaisePropertyChanged();
            }
        }
        private int _loggedUserId = Preferences.Get("userId", 0);
        public int LoggedUserId
        {
            get
            {
                return _loggedUserId;
            }
            set
            {
                _loggedUserId = value;
                RaisePropertyChanged();
            }
        }
        public int ConversationId
        {
            get
            {
                return _conversationId;
            }
            set
            {
                _conversationId = value;
                RaisePropertyChanged();
            }
        }
        public string Message
        {
            get
            {
                return _message;
            }
            set
            {
                _message = value;
                RaisePropertyChanged();
            }
        }
        public int User1Id { get; set; }
        public int User2Id { get; set; }
        public bool IsConnected
        {
            get
            {
                return _isConnected;
            }
            set
            {
                _isConnected = value;
                RaisePropertyChanged();
            }
        }
        private bool _isBusy { get; set; }
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                RaisePropertyChanged();
            }
        }


        




        public ObservableCollection<MessageModel> Messages
        {
            get
            {
                return _messages;
            }
            set
            {
                _messages = value;
                RaisePropertyChanged();
            }
        }
        private HubConnection hubConnection;
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        public Command GoBackToUsersListCommand
        {
            get
            {
                return new Command(async () => {
                    //Push A Page Model
                    if (IsBusy) { return; }
                    IsBusy = true;
                    await CoreMethods.PopPageModel();
                    await Disconnect();
                    IsBusy = false;
                });
            }
        }
        public bool IsOwn { get; set; }

        public MessagePageModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => { await SendMessage(); });
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());
            IsConnected = false;
        }
        async Task Connect()
        {
            try
            {
                hubConnection = new HubConnectionBuilder()
         .WithUrl($"https://www.vhernandezapps.info/chatHub")
         .Build();
                //hubConnection.On<string>("JoinGroupMessage", (user) =>
                //{
                //    Messages.Add(new MessageModel() { UserName = UserName, Message = $"{user} has joined the chat group", IsSystemMessage = true });
                //});

                //hubConnection.On<string>("LeaveGroupMessage", (user) =>
                //{
                //    Messages.Add(new MessageModel() { UserName = UserName, Message = $"{user} has left the chat group", IsSystemMessage = true });
                //});
                await hubConnection.StartAsync();
                await hubConnection.InvokeAsync("JoinGroupChat", ConversationId.ToString(), LoggedUserName);
                IsConnected = true;
                hubConnection.On<string, string,int,int,int>("ReceiveGroupMessage", (user, message,user1Id,user2Id,loggeduserid) =>
                {
                    Messages.Add(new MessageModel() { 
                        UserName = user, 
                        Messagee = message,
                        User1Id= user1Id,
                        User2Id=user2Id,
                        IsSystemMessage = false,
                        IsOwnMessage = IsOwn= LoggedUserName == user,
                        LoggedUserId = loggeduserid
                    });
                });
                if (IsConnected)
                {
                    return;
                }
                
            }
            catch (Exception r)
            {
                await CoreMethods.DisplayAlert("", r.Message, "Ok");
            }      
        }
        async Task SendMessage()
        {
            await hubConnection.InvokeAsync("SendGroupMessage", ConversationId.ToString(), LoggedUserName, Message,User1Id,User2Id, LoggedUserId);
            await ApiService.CreateMessage(ConversationId, Message, User1Id, User2Id,IsOwn, LoggedUserId);
            //await SendMessageNotification();
            Message = "";
        }
        async Task GetAllConversationMessages()
        {
            List<MessageModel> messages = await ApiService.GetConversationMessages(ConversationId);
            foreach (MessageModel message in messages)
            {
                Messages.Add(message);
            }
        }
        async Task Disconnect()
        {
            await hubConnection.InvokeAsync("LeaveGroupChat", ConversationId.ToString(), LoggedUserName);
            await hubConnection.StopAsync();
            IsConnected = false;

        }
        
        public async override void Init(object initData)
        {

            ConversationsUserGet conversation = (ConversationsUserGet)initData;
            User1Id = conversation.User1Id;
            User2Id = conversation.User2Id;
            ConversationId = conversation.Id;
            await GetAllConversationMessages();
            await Connect();
        }

    }
}
