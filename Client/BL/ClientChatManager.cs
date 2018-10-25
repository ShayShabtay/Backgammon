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
    public delegate void InvationResultEventHandler(bool userResponse);
    public delegate void ReciverInivitationResultEventHandler(bool reciverAnswer, bool isGame);
    public delegate void GetMessageEventHandler(string message, string sender);
    public delegate void NotifyUserLeaveChatRoomEventHandler();

    class ClientChatManager
    {
        //Events declaration
        private event InvationResultEventHandler InvatationResultEvent;
        private event ReciverInivitationResultEventHandler ReciverInivtationResultEvent;
        private event GetMessageEventHandler GetMessageEvent;
        private event NotifyUserLeaveChatRoomEventHandler UserLeaveChatEvent;

        //Proxy connection
        private InitializeProxy _server = InitializeProxy.Instance;

        //Ctor
        public ClientChatManager()
        {
            //Reciver get request and send both of them the result, invoke server to init game board.
            _server.Proxy.On("InterationRequest", (string sender, bool isGame) =>
            {
                MessageBoxResult res = MessageBox.Show($"{sender} invite you to chat", "Chat request", MessageBoxButton.YesNo);
                bool reciverAnswer = res == MessageBoxResult.Yes ? true : false;
                ClientUserManager.UserToChat = sender;

                if (reciverAnswer && isGame)
                {
                    Task task = Task.Run(() =>
                    {
                        _server.Proxy.Invoke<string>("InitBoardGame", sender, ClientUserManager.CurrentUserName);
                    });
                } //Invoke the server to init the game board. 

                ReciverInivtationResultEvent?.Invoke(reciverAnswer, isGame); //Send reciver invitation result.

                Task ChatResponse = Task.Run(async () =>
                {
                    await _server.Proxy.Invoke("HandleInvitationResult", reciverAnswer, sender, ClientUserManager.CurrentUserName);
                }); //Send sender invitation result.
                ChatResponse.ConfigureAwait(false);

            });

            //Sender Get the invitation result.
            _server.Proxy.On("getInvitationResult", (bool invationResult) =>
            {
                InvatationResultEvent?.Invoke(invationResult);
            });

            //Reciver get message.
            _server.Proxy.On("getMessage", (string message, string sender) =>
            {
                GetMessageEvent?.Invoke(message, sender);
            });

            //Reciver get notified thet sender leave the chat room.
            _server.Proxy.On("notifyUserLeaveChat", () =>
            {
                Task UserLeaveTask = Task.Run(() =>
                {
                    UserLeaveChatEvent?.Invoke();
                });
            });

            _server.Connection.Start().Wait();
        }

        //Sender methods
        internal void SendRequest(bool isGame)
        {
            Task ChatRequest = Task.Run(async () =>
            {
                await _server.Proxy.Invoke("SendRequest", ClientUserManager.CurrentUserName, ClientUserManager.UserToChat, isGame);
            });
            ChatRequest.ConfigureAwait(false);
            ChatRequest.Wait();
        } //Sender chat or game request.

        internal void InvokeSendMessage(string messageToSend)
        {
            Task sendMessageTask = Task.Run(async () =>
            {
                await _server.Proxy.Invoke("SendMessage", messageToSend, ClientUserManager.UserToChat, ClientUserManager.CurrentUserName);
            });
            sendMessageTask.ConfigureAwait(false);
            sendMessageTask.Wait();
        }//Sender send message.

        internal void NotifyUserLeaveChat()
        {
            Task CloseChatTask = Task.Run(async () =>
            {
                await _server.Proxy.Invoke("NotifyUserLeaveChat", ClientUserManager.UserToChat, ClientUserManager.CurrentUserName);
            });
            CloseChatTask.ConfigureAwait(false);
            //CloseChatTask.Wait();
        } //Sender leave chat room.


        //Sender events
        public void RegisterResultInvationEvent(InvationResultEventHandler onInvationResult)
        {
            InvatationResultEvent += onInvationResult;
        } 

        //Reciver events
        public void RegisterReciverInvitationResultEvent(ReciverInivitationResultEventHandler onRequestEvent)
        {
            ReciverInivtationResultEvent += onRequestEvent;
        } 

        public void RegisterGetMessageEvent(GetMessageEventHandler onSendEvent)
        {
            GetMessageEvent += onSendEvent;
        } 

        public void RegisterOtherUserLeaveChatEvent(NotifyUserLeaveChatRoomEventHandler onOtherUserLeave)
        {
            UserLeaveChatEvent += onOtherUserLeave;
        } 
    }
}
