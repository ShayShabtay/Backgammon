using Client.BL;
using Client.Models;
using Client.Utils;
using Client.Views;
using Common.Enums;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;

namespace Client.ViewModels
{
    class ChatListViewModel : ViewModelBase
    {
        //Managers
        ClientUserManager _userManager;
        ClientChatManager _chatManager;

        //Fields
        private bool isGame;

        //Properties
        private ObservableCollection<UserForContact> _contacts { get; set; }
        public ObservableCollection<UserForContact> Contacts
        {
            get
            {
                return _contacts;
            }

            set
            {
                _contacts = value;
                OnPropertyChanged();
            }
        }
        public ICommand LogOutCommand { get; set; }
        public ICommand OpenGameCommand { get; set; }
        public ICommand OpenChatCommand { get; set; }
        public UserForContact ChosenUser { get; set; }

        //Ctor
        public ChatListViewModel()
        {
            _userManager = new ClientUserManager();
            _chatManager = new ClientChatManager();

            Contacts = Utils.UserListConverterForView.ConvertUser(_userManager.GetContactList());

            _userManager.RgisterNotifyAnyUserChangeStateEvent(UpdateContactList);
            _chatManager.RegisterResultInvationEvent(InvitationResult);
            _chatManager.RegisterReciverInvitationResultEvent(OpenReciverChatOrGame);

            LogOutCommand = new RelayCommand(LogOut);
            OpenChatCommand = new RelayCommand(OpenChat);
            OpenGameCommand = new RelayCommand(OpenGame);
        }

        
        //Sender Methods
        private void OpenChat()
        {
            isGame = false;
            SendChatOrGameRequest(isGame);
        } //Pass sender to SendChatOrGameRequest method with open chat variable.

        private void OpenGame()
        {
            isGame = true;
            SendChatOrGameRequest(isGame);
        } //Pass sender  to SendChatOrGameRequest method with open game variable.

        private void SendChatOrGameRequest (bool isGame)
        {
            if (ChosenUser != null)
            {
                if (ChosenUser.State == UserState.Offline)
                {
                    MessageBox.Show("User is offline, please choose another user");
                }
                else if (ChosenUser.State == UserState.Busy)
                {
                    MessageBox.Show("User is already in a chat, please choose another user");
                }
                else
                {
                    ClientUserManager.UserToChat = ChosenUser.UserName;
                    _chatManager.SendRequest(isGame);
                }
            }
            else
            {
                MessageBox.Show("Please choose user to chat with");
            }
        }  //Send chat or game request to other user.

        private void InvitationResult(bool userResponse)
        {
            if (userResponse)
            {
                _userManager.ChangeUserStatus(UserState.Busy);

                if(isGame)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Application.Current.MainWindow.Content = new GameView();
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        Application.Current.MainWindow.Content = new ChatView();
                    }));
                }

            }
            else
            {
                MessageBox.Show("user refused to join chat");
            }
        } //Open chat or game screen occording to the result.


        //Reciver Methods
        private void OpenReciverChatOrGame(bool reciverAnswer, bool isGame)
        {
            if (reciverAnswer)
            {
                _userManager.ChangeUserStatus(UserState.Busy);

                if (isGame)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Application.Current.MainWindow.Content = new GameView();
                    }));
                }
                else
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        Application.Current.MainWindow.Content = new ChatView();
                    }));
                }
               
            }

        } //Open chat or game screen occording to the result.


        //Common methods
        private void UpdateContactList(Dictionary<string, UserState> dictionary)
        {
            Contacts = Utils.UserListConverterForView.ConvertUser(dictionary);
        }

        private void LogOut()
        {
            if (_userManager.InvokeLogOut())
                Application.Current.MainWindow.Content = new RegisterView();
        } // Log out the user and pass him to the register screen.
    }
}
