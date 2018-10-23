
using Client.Models;
using Common.Enums;
using Common.Interfaces;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Client.BL
{
    //Delegates
    public delegate void NotifiystateEventHandler(Dictionary<string, UserState> dic);

    class ClientUserManager
    {
        //Events declaration
        private event NotifiystateEventHandler NotifyEvent;

        //Proxy connection
        private InitializeProxy _proxy = InitializeProxy.Instance;

        //Fields
        public static string CurrentUserName { get; set; }
        public static string UserToChat { get; set; }

        //Ctor
        public ClientUserManager()
        {
            //Server invoke
            _proxy.Proxy.On("NotifyUserStateChage", (Dictionary<string, UserState> updateContactList) =>
            {
                if(CurrentUserName != null)
                {
                updateContactList.Remove(CurrentUserName);
                }
                NotifyEvent?.Invoke(updateContactList);
            });
            _proxy.Connection.Start().Wait();
            
        }

        //Client invoke functions
        internal bool InvokeRegistration(User user)
        {
            try
            {
                CurrentUserName = user.UserName;
                Task<bool> registerTask = Task.Run(async () =>
                {
                    return await _proxy.Proxy.Invoke<bool>("Register", user);
                });
                registerTask.ConfigureAwait(false);
                registerTask.Wait();
                return registerTask.Result;
            }
            catch (Exception)
            {
                MessageBox.Show("Server error");
                return false;
            }
        }

        internal bool InvokeLogin(User user)
        {
            try
            {
                Task<bool> loginTask = Task.Run(async () =>
                {
                    return await _proxy.Proxy.Invoke<bool>("Login", user);
                });
                loginTask.ConfigureAwait(false);
                loginTask.Wait();
                CurrentUserName = user.UserName;
                return loginTask.Result;
            }
            catch (Exception)
            {
                MessageBox.Show("Server error");
                return false;
            }
        }

        internal Dictionary<string, UserState> GetContactList()
        {
            Task<Dictionary<string, UserState>> Contacts = Task.Run(async () =>
            {
                return await _proxy.Proxy.Invoke<Dictionary<string, UserState>>("GetContactList");
            });

            Contacts.ConfigureAwait(false);
            Contacts.Wait();

            Contacts.Result.Remove(CurrentUserName);
            return Contacts.Result;
        }

        internal void ChangeUserStatus(UserState newState)
        {
            Task ChangeStateTask = Task.Run(async () =>
            {
                await _proxy.Proxy.Invoke("ChangeUserStatus", CurrentUserName, newState);
            });
            ChangeStateTask.ConfigureAwait(false);
        }

        internal bool InvokeLogOut()
        {
            try
            {
                Task logOutTask = Task.Run(async () =>
                {
                    await _proxy.Proxy.Invoke("LogOut", CurrentUserName);
                });
                logOutTask.ConfigureAwait(false);
                logOutTask.Wait();

                return true;
            }
            catch (Exception)
            {
                MessageBox.Show("Server error");
                return false;
            }

        }

        //Events
        public void RgisterNotifyEvent(NotifiystateEventHandler onNotifyEvent)
        {
            NotifyEvent += onNotifyEvent;
        }

    }
}
