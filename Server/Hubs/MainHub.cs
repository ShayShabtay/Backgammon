using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Common.Enums;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNet.SignalR;
using Server.BL;
using Server.Dal;
using Server.Models;

namespace Server.Hubs
{
    public class MainHub : Hub
    {
        //Managers
        UserManager _userManager = UserManager.Instance;
        GameManager _gameManager = GameManager.Instance;

        #region UserHub   

        public bool Register(User newUser)
        {
            bool registerSucceed = _userManager.RegisterToDb(newUser);

            if (registerSucceed)
            {
                NotifyUserStateChage();
                _userManager.AddConnectionID(Context.ConnectionId, newUser.UserName);
                return true;
            }
            return false;

        }

        public bool Login(User userToLogin)
        {
            bool loginSucceed = _userManager.LoginFrmDb(userToLogin);

            if (loginSucceed)
            {
                NotifyUserStateChage();
                try
                {
                _userManager.AddConnectionID(Context.ConnectionId, userToLogin.UserName);

                }
                catch (Exception)
                {

                    return false;
                }
                return true;
            }
            return false;

        }

        public void LogOut(string userName)
        {
            _userManager.LogOut(userName);
            NotifyUserStateChage();
            _userManager.RemoveConnectionId(userName);
        }

        public Dictionary<string, UserState> GetContactList()
        {
            return _userManager.ContactList;
        }

        public void ChangeUserStatus(string userName, UserState newState)
        {
            _userManager.UpdateContactList(userName, newState);
            NotifyUserStateChage();
        }

        private void NotifyUserStateChage()
        {
            Clients.All.NotifyUserStateChage(_userManager.ContactList);
        }

        public override Task OnDisconnected(bool stopCalled)
        {
            string ConnectionId = Context.ConnectionId;
            string currentUser = _userManager.UserConnection.FirstOrDefault(x => x.Value == ConnectionId).Key;
            _userManager.UpdateContactList(currentUser, UserState.Offline);
            _userManager.RemoveConnectionId(currentUser);
            string pairedUser = _userManager.FindPair(currentUser);

            if (pairedUser != null)
            {
                NotifyUserLeaveChat(pairedUser, currentUser);
            }
            NotifyUserStateChage();
            return base.OnDisconnected(stopCalled);
        }

        #endregion

        #region ChatHub

        public void SendRequest(string sender, string reciver, bool isGame)
        {
            string reciverConnectionId = _userManager.GetConnectionID(reciver);
            Clients.Client(reciverConnectionId).InterationRequest(sender, isGame);
        }

        public void HandleInvitationResult(bool userResponse, string sender, string reciver)
        {
            string senderConnectionId = _userManager.GetConnectionID(sender);
            if (userResponse)
            {
                _userManager.addNewPair(sender, reciver);
            }
            Clients.Client(senderConnectionId).getInvitationResult(userResponse);
        }

        public void SendMessage(string message, string receiver, string sender)
        {
            string reciverConnectionId = _userManager.GetConnectionID(receiver);
            Clients.Client(reciverConnectionId).getMessage(message, sender);
        }

        public void NotifyUserLeaveChat(string reciver, string sender)
        {
            string reciverConnectionId = _userManager.GetConnectionID(reciver);
            _userManager.RemovePair(sender, reciver);
            Clients.Client(reciverConnectionId).notifyUserLeaveChat();
        }

        #endregion

        #region GameHub

        public string GetKey(string sender, string reciver)
        {
           return _gameManager.GetKey(sender, reciver);
        }

        public void InitBoardGame(string sender, string reciver)
        {
            _gameManager.InitBoard(sender, reciver);
        }

        public IBoardState GetBoardState(string key)
        {
            return _gameManager.GetBoardState(key);
        }

        public Cube RollCube(string otherPlayer)
        {
            Cube rollResult = _gameManager.RollCubes();
            string otherPlayerConnectionId = _userManager.GetConnectionID(otherPlayer);
            Clients.Client(otherPlayerConnectionId).rollCubesResult(rollResult);
            return rollResult;
        }
        #endregion
    }
}
