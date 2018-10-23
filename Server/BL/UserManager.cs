
using Common.Enums;
using Server.Dal;
using Server.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Server.BL
{
    public class UserManager
    {
        //Properties
        internal Dictionary<string, string> UserConnection { get; private set; }
        internal Dictionary<string, UserState> ContactList { get; private set; }
        internal Dictionary<string, string> UsersInteraction { get; private set; }

        //Singleton
        private static UserManager _instance;
        public static UserManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new UserManager();
                }
                 return _instance;
            }
        }

        //Ctor
        private UserManager()
        {
            ContactList = new Dictionary<string, UserState>();
            UserConnection = new Dictionary<string, string>();
            UsersInteraction = new Dictionary<string, string>();

            using (var context = new Db())
            {
                var users = context.UserTable.Where((u) => u.UserName != null);
                foreach (var user in users)
                {
                    ContactList.Add(user.UserName, UserState.Offline);
                }
            }
        }

        //Methods
        internal void AddConnectionID(string connectionId, string userName)
        {
            try
            {
                UserConnection.Add(userName, connectionId);
            }
            catch (Exception)
            {

                throw new Exception("Cant add user to connection list");
            }
        }

        internal string GetConnectionID(string receiver)
        {
            return UserConnection[receiver];
        }

        internal void RemoveConnectionId(string userName)
        {
            try
            {
                UserConnection.Remove(userName);
            }
            catch (Exception)
            {

                throw new Exception("Cant remove user from connection list");
            }
        }

        internal bool RegisterToDb(User newUser)
        {
            if (newUser == null || string.IsNullOrWhiteSpace(newUser.UserName) || string.IsNullOrWhiteSpace(newUser.Password)) return false;
            else
            {
                using (var context = new Db())
                {
                    var foundUser = context.UserTable.FirstOrDefault((u) => u.UserName == newUser.UserName);
                    if (foundUser != null) return false;
                    context.UserTable.Add(newUser);
                    context.SaveChanges();
                }
                UpdateContactList(newUser.UserName, UserState.Online);
                return true;
            }
        }

        internal bool LoginFrmDb(User userToLogin)
        {
            if (userToLogin == null || string.IsNullOrWhiteSpace(userToLogin.UserName) || string.IsNullOrWhiteSpace(userToLogin.Password)) return false;
            else
            {
                using (var context = new Db())
                {
                    var foundUser = context.UserTable.FirstOrDefault((u) => u.UserName == userToLogin.UserName && u.Password == userToLogin.Password);
                    if (foundUser == null) return false;
                }

                UpdateContactList(userToLogin.UserName, UserState.Online);
                return true;
            }
        }

        internal void addNewPair(string sender, string reciver)
        {
            UsersInteraction.Add(sender, reciver);
        }

        internal string FindPair(string userName)
        {
            if (UsersInteraction.ContainsKey(userName))
            {
                return UsersInteraction[userName];
            }
            else if (UsersInteraction.ContainsValue(userName))
            {
                return UsersInteraction.FirstOrDefault(x => x.Value == userName).Key;
            }
            return null;
        }

        internal void RemovePair(string sender, string reciver)
        {
            if(UsersInteraction.ContainsKey(sender))
            {
                UsersInteraction.Remove(sender);
            }
            else
            {
                UsersInteraction.Remove(reciver);
            }
        }

        internal void LogOut(string userName)
        {
            UpdateContactList(userName, UserState.Offline);
        }

        public void UpdateContactList(string userName, UserState newState)
        {

            if (ContactList.ContainsKey(userName))
            {
                ContactList[userName] = newState;
            }
            else
            {
                ContactList.Add(userName, newState);
            }
        }

    }
}