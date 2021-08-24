using FreshMvvm;
using InstagramClone.Models;
using InstagramClone.PageModels;
using InstagramClone.Services;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace InstagramClone.Pages
{
    public class MessagePageModel : FreshBasePageModel
    {
        private readonly HubConnection _connection;
        private ConnectionState _connectionState = ConnectionState.Disconnected;
        private enum ConnectionState
        {
            Connected,
            Disconnected,
            Faulted
        }
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
        public ICommand SendMessageCommand { get; }
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
                    //await Disconnect();
                    IsBusy = false;
                });
            }
        }
        public MessagePageModel()
        {
            Messages = new ObservableCollection<MessageModel>();
            SendMessageCommand = new AsyncCommand(SendMessage, allowsMultipleExecutions: false);
            ConnectCommand = new Command(async () => await Connect());
            //DisconnectCommand = new Command(async () => await Disconnect());
            var connection = new HubConnectionBuilder();
            _connection = connection.WithUrl($"https://www.vhernandezapps.info/chatHub").Build();
            // Subscribe to event
            _connection.Closed +=(ex) =>
            {
                if (ex == null)
                {
                    Trace.WriteLine("Connection terminated");
                    _connectionState = ConnectionState.Disconnected;
                }
                else
                {
                    Trace.WriteLine($"Connection terminated with error: {ex.GetType()}: {ex.Message}");
                    _connectionState = ConnectionState.Faulted;
                }
                return null;
            };
        }
        async Task Connect()
        {
            if (_connectionState == ConnectionState.Connected)
            {
                return;
            }
            try
            {
                await _connection.StartAsync();
                await _connection.InvokeAsync("JoinGroupChat", ConversationId.ToString(), LoggedUserName);
                _connection.On<string, string, int>("ReceiveGroupMessage", (user, message, loggeduserid) =>
                {
                    Messages.Add(new MessageModel()
                    {
                        Messagee = message,
                        LoggedUserId = loggeduserid,
                    });
                });
                _connectionState = ConnectionState.Connected;
            }
            catch (Exception ex)
            {
                Trace.WriteLine($"Connection.Start Failed: {ex.GetType()}: {ex.Message}");
                _connectionState = ConnectionState.Faulted;
                throw;
            }


        }
        async Task SendMessage()
        {
            if (_connectionState != ConnectionState.Connected)
            {
                await Connect();
            }
            if (_connectionState == ConnectionState.Connected)
            {
                await _connection.InvokeAsync("SendGroupMessage", ConversationId.ToString(), LoggedUserName, Message, LoggedUserId);
            }
            await ApiService.CreateMessage(ConversationId, Message, LoggedUserId);
            await SendMessageNotification();
            Message = "";
        }
        async Task Disconnect()
        {
            if (_connectionState == ConnectionState.Connected)
            {
                await _connection.InvokeAsync("LeaveGroupChat", ConversationId.ToString(), LoggedUserName);
                await _connection.StopAsync();
            }
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

            IsBusy = false;
        }
        protected async override void ViewIsAppearing(object sender, EventArgs e)
        {
            IsBusy = true;
            await Connect();
            IsBusy = false;
        }
        protected async override void ViewIsDisappearing(object sender, EventArgs e)
        {
            IsBusy = true;
            await Disconnect();
            IsBusy = false;
        }
    }
}
