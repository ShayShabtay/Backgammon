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
    public delegate void SendMessageEventHandler(string message, string sender);
    public delegate void InvationResultEventHandler(bool userResponse);
    public delegate void SendRequestEventHandler(string sender, bool res, bool isGame);
    public delegate void NotifyUserLeaveChatRoomEventHandler();

    class ClientChatManager
    {
        //Events declaration
        private event SendMessageEventHandler SendMessageEvent;
        private event InvationResultEventHandler InvatationResultEvent;
        private event SendRequestEventHandler SendRequestEvent;
        private event NotifyUserLeaveChatRoomEventHandler UserLeaveChatEvent;

        //Proxy connection
        private InitializeProxy _proxy = InitializeProxy.Instance;

        //Ctor
        public ClientChatManager()
        {
            //Server inovke
            _proxy.Proxy.On("sendMessage", (string message, string sender) =>
            {
                SendMessageEvent?.Invoke(message, sender);
            });
            _proxy.Proxy.On("InterationRequest", (string sender, bool isGame) =>
            {
                MessageBoxResult res = MessageBox.Show($"{sender} invite you to chat", "Chat request", MessageBoxButton.YesNo);
                bool boolRes = res == MessageBoxResult.Yes ? true : false;
                ClientUserManager.UserToChat = sender;

                SendRequestEvent?.Invoke(sender, boolRes, isGame); // <---- remove sender

                Task ChatResponse = Task.Run(async () =>
                {
                    await _proxy.Proxy.Invoke("HandleInvitationResult", boolRes, sender, ClientUserManager.CurrentUserName);
                });
                ChatResponse.ConfigureAwait(false);

            });
            _proxy.Proxy.On("getInvitationResult", (bool invationResult) =>
            {
                InvatationResultEvent?.Invoke(invationResult);
            });
            _proxy.Proxy.On("notifyUserLeaveChat", () =>
            {
                Task UserLeaveTask = Task.Run(() =>
                {
                  UserLeaveChatEvent?.Invoke();
                });
            });
            _proxy.Connection.Start().Wait();
        }

        //Client invoke functions
        internal void SendRequest(bool isGame)
        {
            Task ChatRequest = Task.Run(async () =>
            {
                await _proxy.Proxy.Invoke("SendRequest", ClientUserManager.CurrentUserName, ClientUserManager.UserToChat, isGame);
            });
            ChatRequest.ConfigureAwait(false);
            ChatRequest.Wait();
        }

        internal void NotifyUserLeaveChat()
        {
            Task CloseChatTask = Task.Run(async () =>
            {
                await _proxy.Proxy.Invoke("NotifyUserLeaveChat", ClientUserManager.UserToChat, ClientUserManager.CurrentUserName);
            });
            CloseChatTask.ConfigureAwait(false);
            //CloseChatTask.Wait();
        }

        internal void InvokeSendMessage(string messageToSend)
        {
            Task sendMessageTask = Task.Run(async () =>
            {
                await _proxy.Proxy.Invoke("SendMessage", messageToSend, ClientUserManager.UserToChat, ClientUserManager.CurrentUserName);
            });
            sendMessageTask.ConfigureAwait(false);
            sendMessageTask.Wait();
        }

        //Events
        public void RegistersendMessageEvent(SendMessageEventHandler onSendEvent)
        {
            SendMessageEvent += onSendEvent;
        }

        public void RegistersendRequestEvent(SendRequestEventHandler onRequestEvent)
        {
            SendRequestEvent += onRequestEvent;
        }

        public void RegisterResultInvationEvent(InvationResultEventHandler onInvationResult)
        {
            InvatationResultEvent += onInvationResult;
        }

        public void RegisterOtherUserLeaveChatEvent(NotifyUserLeaveChatRoomEventHandler onOtherUserLeave)
        {
            UserLeaveChatEvent += onOtherUserLeave;
        }
    }
}
