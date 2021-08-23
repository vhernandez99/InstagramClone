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
    public class MessagePageModel : FreshBasePageModel
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
        private string _externalUserImageUrl;
        public string ExternalUserImageUrl
        {
            set
            {
                _externalUserImageUrl = value;
                RaisePropertyChanged();
            }
            get
            {
                return _externalUserImageUrl;
            }
        }
        public int ExternalUserId { get; set; }
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
        public string LoggedUserImageUrl { get; set; }
        private int _conversationId;
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
        private string _message;
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
        private bool _isBusy;
        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }
            set
            {
                _isBusy = value;
                IsVisible = !value;
                RaisePropertyChanged();
            }
        }
        private bool _isVisible;
        public bool IsVisible
        {
            get
            {
                return _isVisible;
            }
            set
            {
                _isVisible = value;
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
        public Command GoBackToUserConversations
        {
            get
            {
                return new Command(async () =>
                {
                    //Push A Page Model
                    if (IsBusy) { return; }
                    IsBusy = true;
                    await CoreMethods.PopPageModel();
                    await Disconnect();
                    IsBusy = false;
                });
            }
        }
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
                hubConnection.On<string, string, int>("ReceiveGroupMessage", (user, message, loggeduserid) =>
                    {
                        Messages.Add(new MessageModel()
                        {
                            Messagee = message,
                            LoggedUserId = loggeduserid,
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
            IsBusy = true;
            await hubConnection.InvokeAsync("SendGroupMessage", ConversationId.ToString(), LoggedUserName, Message, LoggedUserId);
            await ApiService.CreateMessage(ConversationId, Message, LoggedUserId);
            await SendMessageNotification();
            Message = "";
            IsBusy = false;
        }
        async Task SendMessageNotification()
        {
            var data = new { action = "Play", userId = 5 };
            await ApiService.SendPushNotification(LoggedName, Message, data, ExternalUserTokenFirebase,LoggedUserImageUrl);
        }
        async Task GetUserLoggedInfo()
        {
            var userLogged = await ApiService.GetUserLoggedInfo();
            LoggedUserImageUrl = userLogged.FullImageUrl;
            LoggedName = userLogged.Name;
        }
        async Task GetExternalUserInfo()
        {

            if (LoggedUserId == User1Id)
            {
                ExternalUserId = User2Id;
            }
            if (LoggedUserId == User2Id)
            {
                ExternalUserId = User1Id;
            }
            var user = await ApiService.GetUserInfo(ExternalUserId);
            ExternalUserTokenFirebase.Add(user.TokenFirebase);
            ExternalName = user.Name;
            ExternalUserImageUrl = user.FullImageUrl;
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
            await GetUserLoggedInfo();
            IsBusy = true;
            ConversationsUserGet conversation = (ConversationsUserGet)initData;
            User1Id = conversation.User1Id;
            User2Id = conversation.User2Id;
            await GetExternalUserInfo();
            ConversationId = conversation.Id;
            await GetAllConversationMessages();
            await Connect();
            IsBusy = false;
        }

    }
}
