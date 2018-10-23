using Client.BL;
using Client.Models;
using Client.Utils;
using Client.Views;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Client.ViewModels
{
    class ChatRoomViewModel : ViewModelBase
    {
        //Managers
        private ClientUserManager _userManager;
        private ClientChatManager _chatManager;

        //Propertis
        public string UserToView { get; set; }
        public ICommand SendMessageCommand { get; set; }
        public ICommand CloseChatCommand { get; set; }
        private string _messageToSend;
        public string MessageToSend
        {
            get { return _messageToSend; }
            set {
                _messageToSend = value;
                OnPropertyChanged();
            }
        }
        private string _messagesBlock;
        public string MessagesBlock
        {
            get
            {
                return _messagesBlock;
            }
            set
            {
                _messagesBlock = value;
                OnPropertyChanged();
            }
        }


        //Ctor
        public ChatRoomViewModel()
        {
            _userManager = new ClientUserManager();
            _chatManager = new ClientChatManager();

            UserToView = ClientUserManager.UserToChat;

            SendMessageCommand = new RelayCommand(SendMessage);
            CloseChatCommand = new RelayCommand(CloseChat);

            _chatManager.RegistersendMessageEvent(ReciveMessage);
            _chatManager.RegisterOtherUserLeaveChatEvent(NotifyOtherUserLeaveChat);
        }


        //Functions
        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(MessageToSend)) return;
            else
            {
                MessagesBlock += ChatMessageFormatter.Format(MessageToSend, ClientUserManager.CurrentUserName);
                _chatManager.InvokeSendMessage(MessageToSend);
                MessageToSend = "";
            }
        }

        private void ReciveMessage(string message, string sender)
        {
            MessagesBlock += ChatMessageFormatter.Format(message,sender);

        }

        private void CloseChat()
        {
            _userManager.ChangeUserStatus(UserState.Online);
            _chatManager.NotifyUserLeaveChat();
            Application.Current.MainWindow.Content = new ChatListView();
        }

        private void NotifyOtherUserLeaveChat()
        {

            _userManager.ChangeUserStatus(UserState.Online);

            Application.Current.Dispatcher.Invoke(new Action(() =>
            {
                MessageBox.Show("User leave chat room");
                Application.Current.MainWindow.Content = new ChatListView();
            }));
        }
    }
}
