using FreshMvvm;
using InstagramClone.Models;
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
        private string _userName=Preferences.Get("UserName", string.Empty);
        private int _conversationId;
        private string _message;
        private ObservableCollection<MessageModel> _messages;
        private bool _isConnected;
        //Actually unique UserName
        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                _userName = value;
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

        private HubConnection hubConnection;
        public Command SendMessageCommand { get; }
        public Command ConnectCommand { get; }
        public Command DisconnectCommand { get; }
        public MessagePageModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new Command(async () => { await SendMessage(UserName, Message); });
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
                await hubConnection.InvokeAsync("JoinGroupChat", ConversationId.ToString(), UserName);
                IsConnected = true;
                hubConnection.On<string, string>("ReceiveGroupMessage", (user, message) =>
                {
                    Messages.Add(new MessageModel() { UserName = user, Messagee = message, IsSystemMessage = false, IsOwnMessage = UserName == user });
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
        async Task Disconnect()
        {
            await hubConnection.InvokeAsync("LeaveGroupChat", ConversationId.ToString(), UserName);
            await hubConnection.StopAsync();
            IsConnected = false;
        }
        async Task SendMessage(string userName, string message)
        {
            await hubConnection.InvokeAsync("SendGroupMessage", ConversationId.ToString(), userName, message);
            await ApiService.CreateMessage(ConversationId, ExternalUserName, message);
        }
        public async Task GetAllConversationMessages()
        {
            List<MessageModel> messages = await ApiService.GetConversationMessages(ConversationId);
            foreach(MessageModel message in messages)
            {
                Messages.Add(message);
            }
        }
        async Task CreateConversation(int userid2)
        {
            try
            {
                await ApiService.CreateConversation(userid2);
            }
            catch (Exception r)
            {
                await CoreMethods.DisplayAlert("CreateConversation", r.Message, "Ok");
                return;
            }
        }
        public async void GetUserInfo(int id)
        {
            var userLoggedInfo = await ApiService.GetUserInfo(id);
            UserName = userLoggedInfo.UserName;
        }
        public async Task<int> VerifyIfConversationExists(int userid2)
        {
            try
            {
                ConversationId = await ApiService.VerifyIfConversationExists(userid2);
                return ConversationId;
            }
            catch (Exception r)
            {
                await CoreMethods.DisplayAlert("VerifyIfConversationExists", r.Message, "Ok");
                return 0;
            }
            
        }
        //Obtener valor con Preferences llamado en MainPage
        //public async void GetLoggedUserInfo()
        //{
        //    var userLoggedInfo = await ApiService.GetUserLoggedInfo();
        //    UserName = userLoggedInfo.UserName;
        //}
        protected override async void ViewIsDisappearing(object sender, EventArgs e)
        {
            await Disconnect();
            base.ViewIsDisappearing(sender, e);
        }
        public async override void Init(object initData)
        {
            UsersGetList user = (UsersGetList)initData;
            ExternalUserFullImageUrl = user.FullImageUrl;
            ExternalUserName = user.UserName;
            ExternalName = user.Name;
            await VerifyIfConversationExists(user.Id);
            if (ConversationId == 0)
            {
                await CreateConversation(user.Id);
            }
            await GetAllConversationMessages();
            await Connect();
            
            //GetLoggedUserInfo();
            base.Init(initData);
        }

    }
}
