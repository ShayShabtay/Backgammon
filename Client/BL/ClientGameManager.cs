using Client.Models;
using Common.Interfaces;
using Common.Models;
using Microsoft.AspNet.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.BL
{
    //Delegates
    public delegate void RollcubesResultEventHandler(Cube rollCubeResult);

    class ClientGameManager
    {
        //Events declaration
        private event RollcubesResultEventHandler RollCubesResultEvent;

        //Proxy connection
        private InitializeProxy _server = InitializeProxy.Instance;
        //Properties
        public string GameKey;

        //Ctor
        public ClientGameManager()
        {

            //Server invoke
            _server.Proxy.On("rollCubesResult", (Cube rollCubeResult) =>
            {
                RollCubesResultEvent?.Invoke(rollCubeResult);
            });

            _server.Connection.Start().Wait();

            Task<string> task = Task.Run(() =>
            {
                return _server.Proxy.Invoke<string>("GetKey", ClientUserManager.CurrentUserName, ClientUserManager.UserToChat);

            });
            task.Wait();
            GameKey =  task.Result;
        }

        //Client invoke
        internal Cube RollCubes()
        {
            Task<Cube> task = Task.Run(async () =>
            {
                return await _server.Proxy.Invoke<Cube>("RollCube", ClientUserManager.UserToChat);
            });

            return task.Result;
        }

        internal BoardState GetBoardState()
        {
            Task<BoardState> task = Task.Run(async () =>
              {
                  return await _server.Proxy.Invoke<BoardState>("GetBoardState", GameKey);
              });

            task.Wait();

            return task.Result;
        }

        //Events
        public void RegisterCubesResultEvent(RollcubesResultEventHandler onRollCubesResult)
        {
            RollCubesResultEvent += onRollCubesResult;
        }
    }
}
